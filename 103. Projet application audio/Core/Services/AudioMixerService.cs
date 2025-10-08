using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using AudioEditor.Core.Interfaces;
using AudioEditor.Core.Models;

namespace AudioEditor.Core.Services
{
    public class AudioMixerService : IAudioMixer
    {
        private readonly WaveFormat _outputFormat;

        public AudioMixerService()
        {
            // Format de sortie standard : 44.1kHz, stéréo, 32 bits float
            _outputFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        }

        public void MixTracks(List<AudioTrack> tracks, string outputPath)
        {
            if (tracks == null || !tracks.Any())
                throw new ArgumentException("Aucune piste à mixer.");

            if (string.IsNullOrEmpty(outputPath))
                throw new ArgumentException("Chemin de sortie invalide.");

            try
            {
                var sampleProviders = new List<ISampleProvider>();

                // Traiter chaque piste audio
                foreach (var track in tracks)
                {
                    if (track.IsMuted || !track.Clips.Any())
                        continue;

                    // Créer un mixeur pour cette piste
                    var trackMixer = CreateTrackMixer(track);
                    if (trackMixer != null)
                    {
                        sampleProviders.Add(trackMixer);
                    }
                }

                if (!sampleProviders.Any())
                    throw new InvalidOperationException("Aucune piste audio valide à mixer.");

                // Mixer toutes les pistes avec MixingSampleProvider
                var finalMix = sampleProviders.Count == 1
                    ? sampleProviders[0]
                    : new MixingSampleProvider(sampleProviders);

                // Sauvegarder le mix final
                SaveMixToFile(finalMix, outputPath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors du mixage: {ex.Message}", ex);
            }
        }

        public void AdjustVolume(AudioTrack track, float volume)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            // Valider le volume (0.0 à 1.0)
            track.Volume = Math.Max(0.0f, Math.Min(1.0f, volume));
        }

        public void ApplyPan(AudioTrack track, float pan)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            // Valider le pan (-1.0 à 1.0, où -1 = gauche, 0 = centre, 1 = droite)
            track.Pan = Math.Max(-1.0f, Math.Min(1.0f, pan));
        }

        private ISampleProvider? CreateTrackMixer(AudioTrack track)
        {
            try
            {
                var clipProviders = new List<ISampleProvider>();

                // Traiter chaque clip de la piste
                foreach (var clip in track.Clips)
                {
                    var clipProvider = CreateClipSampleProvider(clip);
                    if (clipProvider != null)
                    {
                        clipProviders.Add(clipProvider);
                    }
                }

                if (!clipProviders.Any())
                    return null;

                // Mixer les clips de cette piste
                var trackMix = clipProviders.Count == 1
                    ? clipProviders[0]
                    : new MixingSampleProvider(clipProviders);

                // Appliquer le volume de la piste avec VolumeSampleProvider
                var volumeProvider = new VolumeSampleProvider(trackMix)
                {
                    Volume = track.Volume
                };

                // Appliquer le panoramique si nécessaire
                if (Math.Abs(track.Pan) > 0.001) // Éviter les calculs inutiles pour pan = 0
                {
                    return new PanningSampleProvider(volumeProvider)
                    {
                        Pan = track.Pan
                    };
                }

                return volumeProvider;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la création du mixeur de piste: {ex.Message}");
                return null;
            }
        }

        private ISampleProvider? CreateClipSampleProvider(AudioClip clip)
        {
            if (string.IsNullOrEmpty(clip?.FilePath))
                return null;

            try
            {
                // Créer un lecteur pour le fichier audio du clip
                var audioFileReader = new AudioFileReader(clip.FilePath);

                // Positionner au début du clip si nécessaire
                if (clip.StartTime > TimeSpan.Zero)
                {
                    audioFileReader.CurrentTime = clip.StartTime;
                }

                // Convertir au format de sortie si nécessaire
                var sampleProvider = audioFileReader.ToSampleProvider();

                if (sampleProvider.WaveFormat.SampleRate != _outputFormat.SampleRate ||
                    sampleProvider.WaveFormat.Channels != _outputFormat.Channels)
                {
                    // Conversion de format si nécessaire
                    if (sampleProvider.WaveFormat.Channels == 1 && _outputFormat.Channels == 2)
                    {
                        sampleProvider = new MonoToStereoSampleProvider(sampleProvider);
                    }

                    if (sampleProvider.WaveFormat.SampleRate != _outputFormat.SampleRate)
                    {
                        // Note: Pour un vrai resampling, utiliser WdlResamplingSampleProvider
                        // Ici on utilise une approche simplifiée
                        System.Diagnostics.Debug.WriteLine($"Attention: Sample rate différent détecté ({sampleProvider.WaveFormat.SampleRate} vs {_outputFormat.SampleRate})");
                    }
                }

                return sampleProvider;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la création du provider pour le clip: {ex.Message}");
                return null;
            }
        }

        private void SaveMixToFile(ISampleProvider sampleProvider, string outputPath)
        {
            try
            {
                // Convertir en WaveProvider pour l'écriture
                var waveProvider = new SampleToWaveProvider(sampleProvider);

                using (var writer = new WaveFileWriter(outputPath, waveProvider.WaveFormat))
                {
                    var buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = waveProvider.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la sauvegarde du fichier mixé: {ex.Message}", ex);
            }
        }

        public void SetMasterVolume(float volume)
        {
            // Méthode utilitaire pour ajuster le volume maître
            // Peut être utilisée avec un VolumeSampleProvider global
        }

        public TimeSpan GetMixDuration(List<AudioTrack> tracks)
        {
            if (tracks == null || !tracks.Any())
                return TimeSpan.Zero;

            var maxDuration = TimeSpan.Zero;

            foreach (var track in tracks)
            {
                if (track.Clips?.Any() == true)
                {
                    var trackDuration = track.Clips.Max(c => c.StartTime.Add(c.Duration));
                    if (trackDuration > maxDuration)
                        maxDuration = trackDuration;
                }
            }

            return maxDuration;
        }
    }
}
using NAudio.Wave;
using AudioEditor.Core.Enums;
using AudioEditor.Core.Interfaces;
using AudioEditor.Core.Models;
using System.IO;
using MyPlaybackState = AudioEditor.Core.Enums.PlaybackState;

namespace AudioEditor.Core.Services
{
    /// <summary>
    /// Service principal pour le moteur audio utilisant NAudio
    /// </summary>
    public class AudioEngineService : IAudioEngine, IDisposable
    {
        private IWavePlayer? wavePlayer;
        private WaveFileReader? waveFileReader;
        private MyPlaybackState currentState = MyPlaybackState.Stopped;
        private AudioProject? currentProject;
        private float masterVolume = 0.75f;

        public MyPlaybackState State => currentState;

        public double Position
        {
            get => waveFileReader?.CurrentTime.TotalSeconds ?? 0;
            set
            {
                if (waveFileReader != null && value >= 0 && value <= Duration)
                {
                    waveFileReader.CurrentTime = TimeSpan.FromSeconds(value);
                }
            }
        }

        public double Duration => waveFileReader?.TotalTime.TotalSeconds ?? 0;

        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = Math.Max(0, Math.Min(1, value));
                // TODO: Appliquer le volume au mixer
            }
        }

        public event EventHandler<MyPlaybackState>? StateChanged;
        public event EventHandler<double>? PositionChanged;

        public AudioEngineService()
        {
            InitializeAudio();
        }

        private void InitializeAudio()
        {
            try
            {
                // Initialiser le player audio
                wavePlayer = new WaveOutEvent();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de l'initialisation audio: {ex.Message}");
            }
        }

        public void Play()
        {
            if (wavePlayer != null && waveFileReader != null)
            {
                if (currentState == MyPlaybackState.Paused)
                {
                    wavePlayer.Play();
                }
                else if (currentState == MyPlaybackState.Stopped)
                {
                    wavePlayer.Init(waveFileReader);
                    wavePlayer.Play();
                }

                SetState(MyPlaybackState.Playing);
            }
        }

        public void Pause()
        {
            if (wavePlayer != null && currentState == MyPlaybackState.Playing)
            {
                wavePlayer.Pause();
                SetState(MyPlaybackState.Paused);
            }
        }

        public void Stop()
        {
            if (wavePlayer != null)
            {
                wavePlayer.Stop();
                if (waveFileReader != null)
                {
                    waveFileReader.Position = 0;
                }
                SetState(MyPlaybackState.Stopped);
            }
        }

        public void LoadProject(string projectPath)
        {
            try
            {
                // TODO: Implémenter le chargement de projet
                currentProject = new AudioProject
                {
                    ProjectName = Path.GetFileNameWithoutExtension(projectPath),
                    SavePath = projectPath
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors du chargement du projet: {ex.Message}");
            }
        }

        public void SaveProject(string projectPath)
        {
            if (currentProject != null)
            {
                try
                {
                    // TODO: Implémenter la sauvegarde de projet
                    currentProject.SavePath = projectPath;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erreur lors de la sauvegarde: {ex.Message}");
                }
            }
        }

        public void LoadAudioFile(string filePath)
        {
            try
            {
                Stop();

                waveFileReader?.Dispose();
                waveFileReader = new WaveFileReader(filePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors du chargement du fichier audio: {ex.Message}");
            }
        }

        private void SetState(MyPlaybackState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                StateChanged?.Invoke(this, currentState);
            }
        }

        public void Dispose()
        {
            Stop();
            wavePlayer?.Dispose();
            waveFileReader?.Dispose();
        }
    }
}
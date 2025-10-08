using System;
using NAudio.Wave;
using AudioEditor.Core.Interfaces;

namespace AudioEditor.Core.Services
{
    public class AudioPlayerService : IAudioPlayer, IDisposable
    {
        private WaveOutEvent? _waveOut;
        private AudioFileReader? _audioFileReader;
        private string? _currentFilePath;
        private Timer? _progressTimer;

        public event EventHandler? PlaybackStarted;
        public event EventHandler? PlaybackStopped;
        public event EventHandler? PlaybackPaused;
        public event EventHandler<TimeSpan>? PositionChanged;

        public PlaybackState PlaybackState => _waveOut?.PlaybackState ?? PlaybackState.Stopped;
        public TimeSpan CurrentPosition => _audioFileReader?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalTime => _audioFileReader?.TotalTime ?? TimeSpan.Zero;
        public float Volume
        {
            get => _waveOut?.Volume ?? 0f;
            set
            {
                if (_waveOut != null)
                    _waveOut.Volume = Math.Max(0f, Math.Min(1f, value));
            }
        }

        public void LoadFile(string filePath)
        {
            try
            {
                // Nettoyer les ressources précédentes
                CleanupResources();

                // Créer AudioFileReader pour lire le fichier
                _audioFileReader = new AudioFileReader(filePath);
                _currentFilePath = filePath;

                // Créer WaveOutEvent pour la lecture
                _waveOut = new WaveOutEvent();
                _waveOut.Init(_audioFileReader);

                // Événements de lecture
                _waveOut.PlaybackStopped += OnPlaybackStopped;

                // Timer pour la barre de progression
                _progressTimer = new Timer(UpdateProgress, null, Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors du chargement du fichier {filePath}: {ex.Message}", ex);
            }
        }

        public void Play()
        {
            if (_waveOut == null || _audioFileReader == null)
                throw new InvalidOperationException("Aucun fichier audio chargé.");

            try
            {
                if (PlaybackState == PlaybackState.Stopped || PlaybackState == PlaybackState.Paused)
                {
                    _waveOut.Play();

                    // Démarrer le timer de progression
                    _progressTimer?.Change(0, 100); // Mise à jour toutes les 100ms

                    PlaybackStarted?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la lecture: {ex.Message}", ex);
            }
        }

        public void Pause()
        {
            if (_waveOut == null)
                return;

            try
            {
                if (PlaybackState == PlaybackState.Playing)
                {
                    _waveOut.Pause();

                    // Arrêter le timer de progression
                    _progressTimer?.Change(Timeout.Infinite, Timeout.Infinite);

                    PlaybackPaused?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la pause: {ex.Message}", ex);
            }
        }

        public void Stop()
        {
            if (_waveOut == null)
                return;

            try
            {
                _waveOut.Stop();

                // Remettre la position au début
                if (_audioFileReader != null)
                {
                    _audioFileReader.CurrentTime = TimeSpan.Zero;
                }

                // Arrêter le timer de progression
                _progressTimer?.Change(Timeout.Infinite, Timeout.Infinite);

                PlaybackStopped?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de l'arrêt: {ex.Message}", ex);
            }
        }

        public void SetPosition(TimeSpan position)
        {
            if (_audioFileReader == null)
                return;

            try
            {
                // Valider la position
                if (position < TimeSpan.Zero)
                    position = TimeSpan.Zero;
                else if (position > _audioFileReader.TotalTime)
                    position = _audioFileReader.TotalTime;

                _audioFileReader.CurrentTime = position;

                // Notifier le changement de position
                PositionChanged?.Invoke(this, position);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors du positionnement: {ex.Message}", ex);
            }
        }

        private void UpdateProgress(object? state)
        {
            if (_audioFileReader != null && PlaybackState == PlaybackState.Playing)
            {
                PositionChanged?.Invoke(this, _audioFileReader.CurrentTime);
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            // Arrêter le timer de progression
            _progressTimer?.Change(Timeout.Infinite, Timeout.Infinite);

            PlaybackStopped?.Invoke(this, EventArgs.Empty);

            if (e.Exception != null)
            {
                throw new InvalidOperationException($"Erreur durant la lecture: {e.Exception.Message}", e.Exception);
            }
        }

        private void CleanupResources()
        {
            try
            {
                _progressTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                _progressTimer?.Dispose();
                _progressTimer = null;

                if (_waveOut != null)
                {
                    _waveOut.PlaybackStopped -= OnPlaybackStopped;
                    _waveOut.Stop();
                    _waveOut.Dispose();
                    _waveOut = null;
                }

                _audioFileReader?.Dispose();
                _audioFileReader = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du nettoyage: {ex.Message}");
            }
        }

        public void Dispose()
        {
            CleanupResources();
        }
    }
}
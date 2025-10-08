using System;
using System.IO;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using AudioEditor.Core.Interfaces;

namespace AudioEditor.Core.Services
{
    public class AudioRecorderService : IAudioRecorder, IDisposable
    {
        private WasapiCapture? _waveIn;
        private WaveFileWriter? _writer;
        private string? _outputFileName;
        private bool _isRecording;

        // Format audio : 44.1kHz, 16 bits, stéréo
        private readonly WaveFormat _recordingFormat = new WaveFormat(44100, 16, 2);

        public bool IsRecording => _isRecording;

        public event EventHandler<WaveInEventArgs>? DataAvailable;
        public event EventHandler? RecordingStopped;

        public void StartRecording()
        {
            if (_isRecording)
                return;

            try
            {
                // Initialiser WasapiCapture pour capture microphone
                _waveIn = new WasapiCapture();
                _waveIn.WaveFormat = _recordingFormat;

                // Générer nom de fichier unique
                _outputFileName = Path.Combine(Path.GetTempPath(),
                    $"Recording_{DateTime.Now:yyyyMMdd_HHmmss}.wav");

                // Créer WaveFileWriter pour sauvegarder
                _writer = new WaveFileWriter(_outputFileName, _recordingFormat);

                // Événements
                _waveIn.DataAvailable += OnDataAvailable;
                _waveIn.RecordingStopped += OnRecordingStopped;

                // Démarrer l'enregistrement
                _waveIn.StartRecording();
                _isRecording = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors du démarrage de l'enregistrement: {ex.Message}", ex);
            }
        }

        public void StopRecording()
        {
            if (!_isRecording || _waveIn == null)
                return;

            try
            {
                _waveIn.StopRecording();
                _isRecording = false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de l'arrêt de l'enregistrement: {ex.Message}", ex);
            }
        }

        public void PauseRecording()
        {
            if (!_isRecording || _waveIn == null)
                return;

            try
            {
                _waveIn.StopRecording();
                _isRecording = false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la pause de l'enregistrement: {ex.Message}", ex);
            }
        }

        private void OnDataAvailable(object? sender, WaveInEventArgs e)
        {
            if (_writer != null && e.BytesRecorded > 0)
            {
                // Écrire les données audio dans le fichier
                _writer.Write(e.Buffer, 0, e.BytesRecorded);
                _writer.Flush();

                // Notifier les listeners
                DataAvailable?.Invoke(this, e);
            }
        }

        private void OnRecordingStopped(object? sender, StoppedEventArgs e)
        {
            // Nettoyer les ressources
            CleanupResources();

            RecordingStopped?.Invoke(this, EventArgs.Empty);

            if (e.Exception != null)
            {
                throw new InvalidOperationException($"Erreur durant l'enregistrement: {e.Exception.Message}", e.Exception);
            }
        }

        private void CleanupResources()
        {
            try
            {
                _writer?.Dispose();
                _writer = null;

                if (_waveIn != null)
                {
                    _waveIn.DataAvailable -= OnDataAvailable;
                    _waveIn.RecordingStopped -= OnRecordingStopped;
                    _waveIn.Dispose();
                    _waveIn = null;
                }
            }
            catch (Exception ex)
            {
                // Log l'erreur mais ne pas la propager lors du cleanup
                System.Diagnostics.Debug.WriteLine($"Erreur lors du nettoyage: {ex.Message}");
            }
        }

        public string? GetLastRecordingPath()
        {
            return _outputFileName;
        }

        public void Dispose()
        {
            if (_isRecording)
            {
                StopRecording();
            }
            CleanupResources();
        }
    }
}
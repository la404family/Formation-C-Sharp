namespace AudioEditor.Core.Interfaces
{
    public interface IAudioRecorder
    {
        void StartRecording();
        void StopRecording();
        void PauseRecording();
        bool IsRecording { get; }
    }
}
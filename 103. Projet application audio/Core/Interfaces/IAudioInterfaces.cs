using AudioEditor.Core.Enums;

namespace AudioEditor.Core.Interfaces
{
    /// <summary>
    /// Interface pour le moteur de lecture audio
    /// </summary>
    public interface IAudioEngine
    {
        AudioEditor.Core.Enums.PlaybackState State { get; }
        double Position { get; set; }
        double Duration { get; }
        float MasterVolume { get; set; }

        void Play();
        void Pause();
        void Stop();
        void LoadProject(string projectPath);
        void SaveProject(string projectPath);
    }

    /// <summary>
    /// Interface pour une piste audio
    /// </summary>
    public interface IAudioTrack
    {
        string Name { get; set; }
        TrackType Type { get; }
        bool IsMuted { get; set; }
        bool IsSolo { get; set; }
        float Volume { get; set; }
        float Pan { get; set; }

        void AddAudioClip(string filePath, double startTime);
        void RemoveAudioClip(int clipIndex);
    }

    /// <summary>
    /// Interface pour un clip audio
    /// </summary>
    public interface IAudioClip
    {
        string FilePath { get; }
        double StartTime { get; set; }
        double Duration { get; }
        float Volume { get; set; }

        void Load();
        void Unload();
    }

    /// <summary>
    /// Interface pour les effets audio
    /// </summary>
    public interface IAudioEffect
    {
        string Name { get; }
        EffectType Type { get; }
        bool IsEnabled { get; set; }

        void Process(float[] inputBuffer, float[] outputBuffer);
        void SetParameter(string paramName, float value);
        float GetParameter(string paramName);
    }
}
using System;

namespace AudioEditor.Core.Interfaces
{
    public interface IAudioPlayer
    {
        void Play();
        void Pause();
        void Stop();
        void SetPosition(TimeSpan position);
    }
}
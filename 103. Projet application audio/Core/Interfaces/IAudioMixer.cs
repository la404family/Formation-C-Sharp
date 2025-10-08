using System.Collections.Generic;
using AudioEditor.Core.Models;

namespace AudioEditor.Core.Interfaces
{
    public interface IAudioMixer
    {
        void MixTracks(List<AudioTrack> tracks, string outputPath);
        void AdjustVolume(AudioTrack track, float volume);
        void ApplyPan(AudioTrack track, float pan);
    }
}
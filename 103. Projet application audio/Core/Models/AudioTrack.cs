using System;
using System.Collections.Generic;

namespace AudioEditor.Core.Models
{
    /// <summary>
    /// Représente un clip audio dans une piste
    /// </summary>
    public class AudioClip
    {
        public string FilePath { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public float Volume { get; set; } = 1.0f;
    }

    /// <summary>
    /// Représente une piste audio contenant des clips
    /// </summary>
    public class AudioTrack
    {
        public string Name { get; set; } = string.Empty;
        public List<AudioClip> Clips { get; set; } = new List<AudioClip>();
        public float Volume { get; set; } = 1.0f;
        public float Pan { get; set; } = 0.0f;
        public bool IsMuted { get; set; } = false;
    }
}
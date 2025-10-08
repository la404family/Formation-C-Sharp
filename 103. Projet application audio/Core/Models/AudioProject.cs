using System;
using System.Collections.Generic;

namespace AudioEditor.Core.Models
{
    /// <summary>
    /// Représente un projet audio complet
    /// </summary>
    public class AudioProject
    {
        public string ProjectName { get; set; } = string.Empty;
        public List<AudioTrack> Tracks { get; set; } = new List<AudioTrack>();
        public string SavePath { get; set; } = string.Empty;
        public TimeSpan TotalDuration { get; set; }
    }
}
using System;

namespace AudioEditor.Core.Models
{
    /// <summary>
    /// Représente un fichier audio avec ses métadonnées
    /// </summary>
    public class AudioFile
    {
        public string FilePath { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
    }
}
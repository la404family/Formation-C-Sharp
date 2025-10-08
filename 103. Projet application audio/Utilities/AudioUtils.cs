using AudioEditor.Core.Enums;
using System.IO;

namespace AudioEditor.Utilities
{
    /// <summary>
    /// Utilitaires pour les fichiers audio
    /// </summary>
    public static class AudioFileUtils
    {
        private static readonly Dictionary<string, AudioFormat> SupportedExtensions = new()
        {
            { ".wav", AudioFormat.WAV },
            { ".mp3", AudioFormat.MP3 },
            { ".flac", AudioFormat.FLAC },
            { ".aac", AudioFormat.AAC },
            { ".ogg", AudioFormat.OGG }
        };

        /// <summary>
        /// Vérifie si un fichier est un format audio supporté
        /// </summary>
        public static bool IsAudioFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return SupportedExtensions.ContainsKey(extension);
        }

        /// <summary>
        /// Obtient le format audio d'un fichier
        /// </summary>
        public static AudioFormat? GetAudioFormat(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return SupportedExtensions.TryGetValue(extension, out AudioFormat format) ? format : null;
        }

        /// <summary>
        /// Obtient une description lisible de la taille du fichier
        /// </summary>
        public static string GetFileSizeDescription(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Formate une durée en format MM:SS.fff
        /// </summary>
        public static string FormatDuration(TimeSpan duration)
        {
            return $"{duration.Minutes:D2}:{duration.Seconds:D2}.{duration.Milliseconds:D3}";
        }

        /// <summary>
        /// Formate une durée en secondes au format MM:SS.fff
        /// </summary>
        public static string FormatDuration(double seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            return FormatDuration(timeSpan);
        }
    }

    /// <summary>
    /// Utilitaires pour les calculs audio
    /// </summary>
    public static class AudioMath
    {
        /// <summary>
        /// Convertit un niveau linéaire en décibels
        /// </summary>
        public static double LinearToDecibels(double linear)
        {
            return linear > 0 ? 20 * Math.Log10(linear) : -96.0;
        }

        /// <summary>
        /// Convertit des décibels en niveau linéaire
        /// </summary>
        public static double DecibelsToLinear(double decibels)
        {
            return Math.Pow(10, decibels / 20.0);
        }

        /// <summary>
        /// Applique une courbe de volume logarithmique
        /// </summary>
        public static float ApplyVolumecurve(float linear)
        {
            // Courbe logarithmique pour un contrôle plus naturel du volume
            return linear * linear;
        }

        /// <summary>
        /// Limite une valeur audio pour éviter la saturation
        /// </summary>
        public static float Clamp(float value, float min = -1.0f, float max = 1.0f)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}
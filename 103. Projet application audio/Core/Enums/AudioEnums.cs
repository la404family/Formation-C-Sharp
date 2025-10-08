namespace AudioEditor.Core.Enums
{
    /// <summary>
    /// États de lecture du moteur audio
    /// </summary>
    public enum PlaybackState
    {
        Stopped,
        Playing,
        Paused,
        Recording
    }

    /// <summary>
    /// Types de pistes audio
    /// </summary>
    public enum TrackType
    {
        Audio,
        MIDI,
        Bus,
        Return
    }

    /// <summary>
    /// Types d'effets audio
    /// </summary>
    public enum EffectType
    {
        Volume,
        Pan,
        Equalizer,
        Compressor,
        Reverb,
        Delay,
        Distortion,
        Filter
    }

    /// <summary>
    /// Formats de fichiers audio supportés
    /// </summary>
    public enum AudioFormat
    {
        WAV,
        MP3,
        FLAC,
        AAC,
        OGG
    }
}
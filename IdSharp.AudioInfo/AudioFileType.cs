using System.ComponentModel;

namespace IdSharp.AudioInfo
{
    /// <summary>
    /// Audio File Type
    /// </summary>
    public enum AudioFileType
    {
        /// <summary>
        /// FLAC (*.flac, *.fla)
        /// </summary>
        [Description("FLAC")]
        Flac,
        /// <summary>
        /// Monkey's Audio (*.ape)
        /// </summary>
        [Description("Monkey's Audio")]
        MonkeysAudio,
        /// <summary>
        /// MPEG (*.mp3, *.mp2)
        /// </summary>
        [Description("MPEG")]
        Mpeg,
        /// <summary>
        /// MPEG-4 (*.m4a)
        /// </summary>
        [Description("MPEG-4")]
        Mpeg4,
        /// <summary>
        /// Musepack (*.mpc, *.mpp, *.mp+)
        /// </summary>
        [Description("Musepack")]
        Musepack,
        /// <summary>
        /// Ogg-Vorbis (*.ogg)
        /// </summary>
        [Description("Ogg-Vorbis")]
        OggVorbis,
        /// <summary>
        /// OptimFROG (*.ofr)
        /// </summary>
        [Description("OptimFROG")]
        OptimFrog,
        /// <summary>
        /// RIFF-WAVE (*.wav)
        /// </summary>
        [Description("WAV")]
        RiffWave,
        /// <summary>
        /// Shorten (*.shn)
        /// </summary>
        [Description("Shorten")]
        Shorten,
        /// <summary>
        /// Windows Media Audio (*.wma)
        /// </summary>
        [Description("WMA")]
        WindowsMedia,
        /// <summary>
        /// WavPack (*.wv)
        /// </summary>
        [Description("WavPack")]
        WavPack,
        /// <summary>
        /// True Audio (*.tta)
        /// </summary>
        [Description("True Audio")]
        TrueAudio
    }
}

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Unsynchronized lyrics/text transcription</para>
    /// <para>From the ID3v2 specification:</para>
    /// <para>
    /// This frame contains the lyrics of the song or a text transcription of
    /// other vocal activities. The head includes an encoding descriptor and
    /// a content descriptor. The body consists of the actual text. The
    /// 'Content descriptor' is a terminated string. If no descriptor is
    /// entered, 'Content descriptor' is $00 (00) only. Newline characters
    /// are allowed in the text. There may be more than one 'Unsynchronized
    /// lyrics/text transcription' frame in each tag, but only one with the
    /// same language and content descriptor.
    /// </para><para>
    ///     [Header for 'Unsynchronized lyrics/text transcription', ID: "USLT"]<br />
    ///     Text encoding        $xx<br />
    ///     Language             $xx xx xx<br />
    ///     Content descriptor   [text string according to encoding] $00 (00)<br />
    ///     Lyrics/text          [full text string according to encoding]
    /// </para>
    /// </summary>
    public interface IUnsynchronizedText : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the ISO-639-2 language code.
        /// </summary>
        /// <value>The ISO-639-2 language code.</value>
        string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the content descriptor.
        /// </summary>
        /// <value>The content descriptor.</value>
        string ContentDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }
    }
}

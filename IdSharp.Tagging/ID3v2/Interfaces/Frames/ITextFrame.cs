namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Text information frame</para>
    /// <para>From the ID3v2 specification:</para>
    /// <para>
    /// The text information frames are the most important frames, containing
    /// information like artist, album and more. There may only be one text
    /// information frame of its kind in a tag. If the textstring is
    /// followed by a termination ($00 (00)) all the following information
    /// should be ignored and not be displayed. All text frame identifiers
    /// begin with "T". Only text frame identifiers begin with "T", with the
    /// exception of the "TXXX" frame. All the text information frames have
    /// the following format:</para>
    ///
    /// <para>[Header for 'Text information frame', ID: "T000" - "TZZZ", excluding "TXXX"]<br />
    /// Text encoding                $xx<br />
    /// Information                  [text string according to encoding]</para>
    /// </summary>
    public interface ITextFrame : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        string Value { get; set; }
    }
}

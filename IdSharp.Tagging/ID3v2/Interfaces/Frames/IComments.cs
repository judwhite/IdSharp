namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Comments</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>
    ///    This frame is intended for any kind of full text information that
    ///    does not fit in any other frame. It consists of a frame header
    ///    followed by encoding, language and content descriptors and is ended
    ///    with the actual comment as a text string. Newline characters are
    ///    allowed in the comment text string. There may be more than one
    ///    comment frame in each tag, but only one with the same language and
    ///    content descriptor.
    /// </para>
    ///      <para>[Header for 'Comment', ID: "COMM"]<br />
    ///      Text encoding          $xx<br />
    ///      Language               $xx xx xx<br />
    ///      Short content descrip. [text string according to encoding] $00 (00)<br />
    ///      The actual text        [full text string according to encoding]</para>
    /// </summary>
    public interface IComments : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the ISO-639-2 language code.
        /// </summary>
        /// <value>The ISO-639-2 language code.</value>
        string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the comment description.
        /// </summary>
        /// <value>The comment description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        string Value { get; set; }
    }
}

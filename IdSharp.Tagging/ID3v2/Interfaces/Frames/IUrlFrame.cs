namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>URL link frames</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>
    ///    With these frames dynamic data such as webpages with touring
    ///    information, price information or plain ordinary news can be added to
    ///    the tag. There may only be one URL [URL] link frame of its kind in an
    ///    tag, except when stated otherwise in the frame description. If the
    ///    textstring is followed by a termination ($00 (00)) all the following
    ///    information should be ignored and not be displayed. All URL link
    ///    frame identifiers begins with "W". Only URL link frame identifiers
    ///    begins with "W". All URL link frames have the following format:
    /// </para><para>
    ///      [Header for 'URL link frame', ID: "W000" - "WZZZ", excluding "WXXX"]<br />
    ///      URL              [text string]
    /// </para>
    /// </summary>
    public interface IUrlFrame : IFrame
    {
        /// <summary>
        /// Gets or sets the URL value.
        /// </summary>
        /// <value>The URL value.</value>
        string Value { get; set; }
    }
}

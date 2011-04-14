namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>User defined URL link frame</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>
    ///    This frame is intended for URL links concerning the audiofile
    ///    in a similar way to the other "W"-frames. The frame body consists
    ///    of a description of the string, represented as a terminated string,
    ///    followed by the actual URL. The URL is always encoded with ISO-8859-1.
    ///    There may be more than one "WXXX" frame in each tag,
    ///    but only one with the same description.
    /// </para><para>
    ///      [Header for 'User defined URL link frame', ID: "WXXX"]<br />
    ///      Text encoding     $xx<br />
    ///      Description       [text string according to encoding] $00 (00)<br />
    ///      URL               [text string]
    /// </para>
    /// </summary>
    public interface IWXXXFrame : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the URL description.
        /// </summary>
        /// <value>The URL description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL value.
        /// </summary>
        /// <value>The URL value.</value>
        string Value { get; set; }
    }
}

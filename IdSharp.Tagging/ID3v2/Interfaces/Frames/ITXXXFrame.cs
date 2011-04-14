namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>User defined text information frame</para>
    /// <para>From the ID3v2 specification:</para>
    /// <para>
    ///    This frame is intended for one-string text information concerning the
    ///    audiofile in a similar way to the other "T"-frames. The frame body
    ///    consists of a description of the string, represented as a terminated
    ///    string, followed by the actual string. There may be more than one
    ///    "TXXX" frame in each tag, but only one with the same description.
    /// </para><para>
    ///      [Header for 'User defined text information frame', ID: "TXXX"]<br />
    ///      Text encoding     $xx<br />
    ///      Description       [text string according to encoding] $00 (00)<br />
    ///      Value             [text string according to encoding]
    /// </para>
    /// </summary>
    public interface ITXXXFrame : ITextFrame
    {
        /// <summary>
        /// Gets or sets the user-defined text frame description.
        /// </summary>
        /// <value>The user-defined text frame description.</value>
        string Description { get; set; }
    }
}

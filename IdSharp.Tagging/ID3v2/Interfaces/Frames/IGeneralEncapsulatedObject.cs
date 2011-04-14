namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>General encapsulated object</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>
    ///    In this frame any type of file can be encapsulated. After the header,
    ///    'Frame size' and 'Encoding' follows 'MIME type' [MIME] represented as
    ///    as a terminated string encoded with ISO 8859-1 [ISO-8859-1]. The
    ///    filename is case sensitive and is encoded as 'Encoding'. Then follows
    ///    a content description as terminated string, encoded as 'Encoding'.
    ///    The last thing in the frame is the actual object. The first two
    ///    strings may be omitted, leaving only their terminations. MIME type is
    ///    always an ISO-8859-1 text string. There may be more than one "GEOB"
    ///    frame in each tag, but only one with the same content descriptor.
    /// </para>
    ///      <para>[Header for 'General encapsulated object', ID: "GEOB"]<br />
    ///      Text encoding          $xx<br />
    ///      MIME type              [text string] $00<br />
    ///      FileName               [text string according to encoding] $00 (00)<br />
    ///      Content description    [text string according to encóding] $00 (00)<br />
    ///      Encapsulated object    [binary data]</para>
    /// </summary>
    public interface IGeneralEncapsulatedObject : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the MIME type.
        /// </summary>
        /// <value>The MIME type.</value>
        string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the encapsulated object.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>A copy of the encapsulated object.</value>
        byte[] EncapsulatedObject { get; set; }
    }
}

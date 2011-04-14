namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Private frame</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>This frame is used to contain information from a software producer
    ///    that its program uses and does not fit into the other frames. The
    ///    frame consists of an 'Owner identifier' string and the binary data.
    ///    The 'Owner identifier' is a null-terminated string with a URL [URL]
    ///    containing an email address, or a link to a location where an email
    ///    address can be found, that belongs to the organisation responsible
    ///    for the frame. Questions regarding the frame should be sent to the
    ///    indicated email address. The tag may contain more than one "PRIV"
    ///    frame but only with different contents. It is recommended to keep the
    ///    number of "PRIV" frames as low as possible.</para>
    /// 
    ///      <para>[Header for 'Private frame', ID: "PRIV"]<br />
    ///      Owner identifier      [text string] $00<br />
    /// 	 The private data      [binary data]</para>
    /// </summary>
    public interface IPrivateFrame : IFrame
    {
        /// <summary>
        /// Gets or sets the owner identifier URL.
        /// </summary>
        /// <value>The owner identifier URL.</value>
        string OwnerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the private data.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>The private data.</value>
        byte[] PrivateData { get; set; }
    }
}

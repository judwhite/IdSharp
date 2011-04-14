namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Linked information</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>To keep space waste as low as possible this frame may be used to link
    ///    information from another ID3v2 tag that might reside in another audio
    ///    file or alone in a binary file. It is recommended that this method is
    ///    only used when the files are stored on a CD-ROM or other
    ///    circumstances when the risk of file seperation is low. The frame
    ///    contains a frame identifier, which is the frame that should be linked
    ///    into this tag, a URL field, where a reference to the file where
    ///    the frame is [is] given, and additional ID data, if needed. Data should be
    ///    retrieved from the first tag found in the file to which this link
    ///    points. There may be more than one "LINK" frame in a tag, but only
    ///    one with the same contents. A linked frame is to be considered as
    ///    part of the tag and has the same restrictions as if it was a physical
    ///    part of the tag (i.e. only one "RVRB" frame allowed, whether it's
    ///    linked or not).</para>
    /// 
    ///      <para>[Header for 'Linked information', ID: "LINK"]<br />
    ///      Frame identifier        $xx xx xx xx<br />
    ///      URL                     [text string] $00<br />
    ///      ID and additional data  [text string(s)]</para>
    /// 
    ///    <para>Frames that may be linked and need no additional data are "IPLS",
    ///    "MCID", "ETCO", "MLLT", "SYTC", "RVAD", "EQUA", "RVRB", "RBUF", the
    ///    text information frames and the URL link frames.</para>
    /// 
    ///    <para>The "TXXX", "APIC", "GEOB" and "AENC" frames may be linked with
    ///    the content descriptor as additional ID data.</para>
    /// 
    ///    <para>The "COMM", "SYLT" and "USLT" frames may be linked with three bytes
    ///    of language descriptor directly followed by a content descriptor as
    ///    additional ID data.</para>
    /// 
    /// TODO: Did this change with ID3v2.4?
    /// </summary>
    public interface ILinkedInformation : IFrame
    {
        /// <summary>
        /// Gets or sets the linked frame ID (ie, "TALB").
        /// </summary>
        /// <value>The linked frame ID.</value>
        string FrameIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the URL where a reference to the file where the frame is is given.
        /// </summary>
        /// <value>The URL where a reference to the file where the frame is is given..</value>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets the additional data.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>The additional data.</value>
        byte[] AdditionalData { get; set; }
    }
}

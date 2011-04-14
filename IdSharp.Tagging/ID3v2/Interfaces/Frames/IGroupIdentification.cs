namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Group identification registration</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>This frame enables grouping of otherwise unrelated frames. This can
    ///    be used when some frames are to be signed. To identify which frames
    ///    belongs to a set of frames a group identifier must be registered in
    ///    the tag with this frame. The 'Owner identifier' is a null-terminated
    ///    string with a URL containing an email address, or a link to a
    ///    location where an email address can be found, that belongs to the
    ///    organisation responsible for this grouping. Questions regarding the
    ///    grouping should be sent to the indicated email address. The 'Group
    ///    symbol' contains a value that associates the frame with this group
    ///    throughout the whole tag. Values below $80 are reserved. The 'Group
    ///    symbol' may optionally be followed by some group specific data, e.g.
    ///    a digital signature. There may be several "GRID" frames in a tag but
    ///    only one containing the same symbol and only one containing the same
    ///    owner identifier. The group symbol must be used somewhere in the tag.
    ///    See section 3.3.1, flag j for more information.</para>
    /// 
    ///      <para>[Header for 'Group ID registration', ID: "GRID"]<br />
    ///      Owner identifier      [text string] $00<br />
    ///      Group symbol          $xx<br />
    /// 	 Group dependent data  [binary data]</para>
    /// </summary>
    public interface IGroupIdentification : IFrame
    {
        /// <summary>
        /// Gets or sets the owner identifier URL.
        /// </summary>
        /// <value>The owner identifier URL.</value>
        string OwnerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the group symbol.
        /// </summary>
        /// <value>The group symbol.</value>
        byte GroupSymbol { get; set; }

        /// <summary>
        /// Gets or sets the group dependent data.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>The group dependent data.</value>
        byte[] GroupDependentData { get; set; }
    }
}

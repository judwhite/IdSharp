namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Music CD identifier</para>
    /// 
    ///    <para>This frame is intended for music that comes from a CD, so that the CD
    ///    can be identified in databases such as the CDDB [CDDB]. The frame
    ///    consists of a binary dump of the Table Of Contents, TOC, from the CD,
    ///    which is a header of 4 bytes and then 8 bytes/track on the CD plus 8
    ///    bytes for the 'lead out' making a maximum of 804 bytes. The offset to
    ///    the beginning of every track on the CD should be described with a
    ///    four bytes absolute CD-frame address per track, and not with absolute
    ///    time. This frame requires a present and valid "TRCK" frame, even if
    ///    the CD only has one track. There may only be one "MCDI" frame in
    ///    each tag.</para>
    /// 
    ///      <para>[Header for 'Music CD identifier', ID: "MCDI"]<br />
    ///      CD TOC                [binary data]</para>
    /// </summary>
    public interface IMusicCDIdentifier : IFrame
    {
        // TODO: This is reportedly implemented differently in many applications (including WMP)

        /// <summary>
        /// <para>CD TOC (Table of Contents) which is a header of 4 bytes and then 8 bytes/track on the CD plus 8
        ///    bytes for the 'lead out' making a maximum of 804 bytes. The offset to
        ///    the beginning of every track on the CD should be described with a
        ///    four bytes absolute CD-frame address per track, and not with absolute
        ///    time.</para>
        /// <para>Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.</para>
        /// </summary>
        byte[] TOC { get; set; }
    }
}

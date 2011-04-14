namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// This frame's purpose is to be able to identify the audio file in a
    /// database that may contain more information relevant to the content.
    /// Since standardisation of such a database is beyond this document, all
    /// frames begin with a null-terminated string with a URL [URL]
    /// containing an email address, or a link to a location where an email
    /// address can be found, that belongs to the organisation responsible
    /// for this specific database implementation. Questions regarding the
    /// database should be sent to the indicated email address. The URL
    /// should not be used for the actual database queries. The string
    /// "http://www.id3.org/dummy/ufid.html" should be used for tests.
    /// Software that isn't told otherwise may safely remove such frames. The
    /// 'Owner identifier' must be non-empty (more than just a termination).
    /// The 'Owner identifier' is then followed by the actual identifier,
    /// which may be up to 64 bytes. There may be more than one "UFID" frame
    /// in a tag, but only one with the same 'Owner identifier'.
    /// 
    /// [Header for 'Unique file identifier', ID: "UFID"]
    /// Owner identifier        [text string] $00
    /// Identifier              [up to 64 bytes binary data]
    /// </summary>
    public interface IUniqueFileIdentifier : IFrame
    {
        /// <summary>
        /// Gets or sets the owner identifier URL encoded in ISO-8859-1.  This value must be unique in the list of unique file identifiers.  See <see cref="IFrameContainer.UniqueFileIdentifierList"/>.
        /// </summary>
        /// <value>The owner identifier URL.</value>
        string OwnerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier data.  The maximum length for this field is 64 bytes.  Returns a copy of the identifier data, 
        /// therefore the byte array cannot be modified directly.  Use the set property to update the identifier data.
        /// </summary>
        /// <value>A copy of the identifier data.</value>
        byte[] Identifier { get; set; }
    }
}

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Encrypted meta frame.  Only supported in ID3v2.2.
    /// </summary>
    public interface IEncryptedMetaFrame : IFrame
    {
        /// <summary>
        /// Gets or sets the owner identifier, a URL containing an email address, or a
        /// link to a location where an email adress can be found, that belongs to
        /// the organisation responsible for this specific encrypted meta frame.
        /// </summary>
        /// <value>The owner identifier.</value>
        string OwnerIdentifier { get; set; }
        
        /// <summary>
        /// Gets or sets a short content description and explanation as to why the frame is encrypted.
        /// </summary>
        /// <value>A short content description and explanation as to why the frame is encrypted.</value>
        string ContentExplanation { get; set; }
        
        /// <summary>
        /// Gets or sets the encrypted data.
        /// </summary>
        /// <value>The encrypted data.</value>
        byte[] EncryptedData { get; set; }
    }
}

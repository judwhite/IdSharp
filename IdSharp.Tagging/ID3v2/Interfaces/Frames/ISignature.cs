namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Signature frame</para>
    /// <para>
    /// This frame enables a group of frames, grouped with the 'Group
    /// identification registration', to be signed. Although signatures can
    /// reside inside the registration frame, it might be desired to store
    /// the signature elsewhere, e.g. in watermarks. There may be more than
    /// one 'signature frame' in a tag, but no two may be identical.
    /// </para>
    /// </summary>
    public interface ISignature : IFrame
    {
        /// <summary>
        /// Gets or sets the group symbol.  Related to Group Identification Registration.
        /// </summary>
        /// <value>The group symbol.  Related to Group Identification Registration.</value>
        byte GroupSymbol { get; set; }

        /// <summary>
        /// Gets or sets the signature.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>The signature.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.</value>
        byte[] SignatureData { get; set; }
    }
}

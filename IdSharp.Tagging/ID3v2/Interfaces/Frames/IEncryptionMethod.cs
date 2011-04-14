namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Encryption method registration</para>
    /// 
    ///    <para>To identify with which method a frame has been encrypted the
    ///    encryption method must be registered in the tag with this frame. The
    ///    'Owner identifier' is a null-terminated string with a URL
    ///    containing an email address, or a link to a location where an email
    ///    address can be found, that belongs to the organisation responsible
    ///    for this specific encryption method. Questions regarding the
    ///    encryption method should be sent to the indicated email address. The
    ///    'Method symbol' contains a value that is associated with this method
    ///    throughout the whole tag. Values below $80 are reserved. The 'Method
    ///    symbol' may optionally be followed by encryption specific data. There
    ///    may be several "ENCR" frames in a tag but only one containing the
    ///    same symbol and only one containing the same owner identifier. The
    ///    method must be used somewhere in the tag. See section 3.3.1, flag j
    ///    for more information.  TODO: Was this updated in ID3v2.4?</para>
    /// 
    ///      <para>[Header for 'Encryption method registration', ID: "ENCR"]<br />
    ///      Owner identifier    [text string] $00<br />
    ///      Method symbol       $xx<br />
    ///      Encryption data     [binary data]</para>
    /// </summary>
    public interface IEncryptionMethod : IFrame
    {
        /// <summary>
        /// Gets or sets the owner identifier URL or email address.
        /// </summary>
        /// <value>The owner identifier URL or email address.</value>
        string OwnerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the encryption method symbol referenced by encrypted frames.  Values below 0x80 are reserved.
        /// </summary>
        /// <value>The encryption method symbol referenced by encrypted frames.</value>
        byte MethodSymbol { get; set; }

        /// <summary>
        /// Gets or sets the encryption data.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>The encryption data.</value>
        byte[] EncryptionData { get; set; }
    }
}

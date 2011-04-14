namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Audio encryption</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>
    ///    This frame indicates if the actual audio stream is encrypted, and by
    ///    whom. Since standardisation of such encrypion scheme is beyond this
    ///    document, all "AENC" frames begin with a terminated string with a
    ///    URL containing an email address, or a link to a location where an
    ///    email address can be found, that belongs to the organisation
    ///    responsible for this specific encrypted audio file. Questions
    ///    regarding the encrypted audio should be sent to the email address
    ///    specified. If a $00 is found directly after the 'Frame size' and the
    ///    audiofile indeed is encrypted, the whole file may be considered
    ///    useless.
    /// <para></para>
    ///    After the 'Owner identifier', a pointer to an unencrypted part of the
    ///    audio can be specified. The 'Preview start' and 'Preview length' is
    ///    described in frames. If no part is unencrypted, these fields should
    ///    be left zeroed. After the 'preview length' field follows optionally a
    ///    datablock required for decryption of the audio. There may be more
    ///    than one "AENC" frames in a tag, but only one with the same 'Owner
    ///    identifier'.
    /// </para>
    ///      <para>[Header for 'Audio encryption', ID: "AENC"]<br />
    ///      Owner identifier   [text string] $00<br />
    ///      Preview start      $xx xx<br />
    ///      Preview length     $xx xx<br />
    ///      Encryption info    [binary data]</para>
    /// </summary>
    public interface IAudioEncryption : IFrame
    {
        /// <summary>
        /// Gets or sets the owner identifier URL.
        /// </summary>
        /// <value>The owner identifier URL.</value>
        string OwnerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the start of the preview section in frames.
        /// </summary>
        /// <value>The start of the preview section in frames.</value>
        short PreviewStart { get; set; }

        /// <summary>
        /// Gets or sets the length of the preview section in frames.
        /// </summary>
        /// <value>The length of the preview section in frames.</value>
        short PreviewLength { get; set; }

        /// <summary>
        /// Gets or sets the encryption info.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>A copy of the encryption info.</value>
        byte[] EncryptionInfo { get; set; }
    }
}

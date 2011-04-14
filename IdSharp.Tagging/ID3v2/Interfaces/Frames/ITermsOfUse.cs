namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Terms of use frame</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>This frame contains a brief description of the terms of use and
    ///    ownership of the file. More detailed information concerning the legal
    ///    terms might be available through the "WCOP" frame. Newlines are
    ///    allowed in the text. There may only be one "USER" frame in a tag.</para>
    /// 
    ///      <para>[Header for 'Terms of use frame', ID: "USER"]<br />
    ///      Text encoding        $xx<br />
    ///      Language             $xx xx xx<br />
    ///      The actual text      [text string according to encoding]</para>
    /// </summary>
    public interface ITermsOfUse : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the ISO-639-2 language code.
        /// </summary>
        /// <value>The ISO-639-2 language code.</value>
        string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the terms of use/ownership information.  Newlines are allowed.
        /// </summary>
        /// <value>The terms of use/ownership information.  Newlines are allowed.</value>
        string Value { get; set; }
    }
}

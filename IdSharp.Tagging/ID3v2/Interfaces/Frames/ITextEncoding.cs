namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Interface used by frames that support the TextEncoding property.
    /// </summary>
    public interface ITextEncoding
    {
        /// <summary>
        /// Gets or sets the text encoding.
        /// </summary>
        /// <value>The text encoding.</value>
        EncodingType TextEncoding { get; set; }
    }
}

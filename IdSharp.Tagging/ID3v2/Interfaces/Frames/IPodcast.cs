namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Play Count frame.
    /// </summary>
    public interface IPodcast : IFrame
    {
        /// <summary>
        /// Gets or sets if podcast
        /// </summary>
        /// <value>True if podcast.</value>
        bool Value { get; set; }
    }
}

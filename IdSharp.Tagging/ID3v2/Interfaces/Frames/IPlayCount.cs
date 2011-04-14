namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Play Count frame.
    /// </summary>
    public interface IPlayCount : IFrame
    {
        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        /// <value>The play count.</value>
        long? Value { get; set; }
    }
}

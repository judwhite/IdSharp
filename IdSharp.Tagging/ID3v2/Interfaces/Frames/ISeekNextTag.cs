namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Seek Next Tag.
    /// </summary>
    public interface ISeekNextTag : IFrame
    {
        /// <summary>
        /// Gets or sets the minimum offset to the next tag.  Calculated from the end of this
        /// tag to the beginning of the next. 
        /// </summary>
        /// <value>The minimum offset to the next tag.  Calculated from the end of this
        /// tag to the beginning of the next.</value>
        int MinimumOffsetToNextTag { get; set; }
    }
}

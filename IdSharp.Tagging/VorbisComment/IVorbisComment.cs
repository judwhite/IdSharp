namespace IdSharp.Tagging.VorbisComment
{
    /// <summary>
    /// Vorbis Comment
    /// </summary>
    public interface IVorbisComment
    {
        /// <summary>
        /// Writes the tag.
        /// </summary>
        /// <param name="path">The path.</param>
        void Write(string path);

        /// <summary>
        /// Reads the tag.
        /// </summary>
        /// <param name="path">The path.</param>
        void Read(string path);

        /// <summary>
        /// Gets or sets the vendor.
        /// </summary>
        /// <value>The vendor.</value>
        string Vendor { get; set; }

        /*/// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        IEnumerable<KeyValuePair<String, String>> Items { get; }*/
    }
}

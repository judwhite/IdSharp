namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// Specifies version and options for reading ID3 tags.
    /// </summary>
    public sealed class TagReadingInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagReadingInfo"/> class.
        /// </summary>
        /// <param name="tagVersion">The tag version.</param>
        /// <param name="tagVersionOptions">The tag version options.</param>
        public TagReadingInfo(ID3v2TagVersion tagVersion, TagVersionOptions tagVersionOptions)
        {
            TagVersion = tagVersion;
            TagVersionOptions = tagVersionOptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagReadingInfo"/> class.
        /// </summary>
        /// <param name="tagVersion">The tag version.</param>
        public TagReadingInfo(ID3v2TagVersion tagVersion)
            : this(tagVersion, TagVersionOptions.None)
        {
        }

        /// <summary>
        /// Tag version.
        /// </summary>
        public ID3v2TagVersion TagVersion { get; set; }

        /// <summary>
        /// Tag version options.  Designates special directions for reading frames.
        /// </summary>
        public TagVersionOptions TagVersionOptions { get; set; }
    }
}

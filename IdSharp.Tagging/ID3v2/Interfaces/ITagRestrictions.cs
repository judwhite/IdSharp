namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// Tag restrictions.  See <see cref="IID3v2ExtendedHeader.TagRestrictions"/>.  Only valid in ID3v2.4.
    /// </summary>
    public interface ITagRestrictions
    {
        /// <summary>
        /// Gets or sets the tag size restriction.
        /// </summary>
        /// <value>The tag size restriction.</value>
        TagSizeRestriction TagSizeRestriction { get; set; }

        /// <summary>
        /// Gets or sets the text encoding restriction.
        /// </summary>
        /// <value>The text encoding restriction.</value>
        TextEncodingRestriction TextEncodingRestriction { get; set; }

        /// <summary>
        /// Gets or sets the text fields size restriction.
        /// </summary>
        /// <value>The text fields size restriction.</value>
        TextFieldsSizeRestriction TextFieldsSizeRestriction { get; set; }

        /// <summary>
        /// Gets or sets the image encoding restriction.
        /// </summary>
        /// <value>The image encoding restriction.</value>
        ImageEncodingRestriction ImageEncodingRestriction { get; set; }

        /// <summary>
        /// Gets or sets the image size restriction.
        /// </summary>
        /// <value>The image size restriction.</value>
        ImageSizeRestriction ImageSizeRestriction { get; set; }
    }
}

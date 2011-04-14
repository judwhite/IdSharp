namespace IdSharp.Tagging.VorbisComment
{
    /// <summary>
    /// Name/Value item used by some tagging methods.  See <see cref="NameValueList"/>.
    /// </summary>
    public class NameValueItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValueItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public NameValueItem(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}

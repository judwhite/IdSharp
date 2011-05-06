namespace IdSharp.Common.Utils
{
    /// <summary>
    /// Represents data to be posted to a URI.
    /// </summary>
    public class PostData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostData"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public PostData(string field, string value)
        {
            Field = field;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}

using System.ComponentModel;

namespace IdSharp.Tagging.ID3v1
{
    /// <summary>
    /// ID3v1 tag version.
    /// </summary>
    public enum ID3v1TagVersion
    {
        /// <summary>
        /// ID3v1.0
        /// </summary>
        [Description("ID3v1.0")]
        ID3v10,
        /// <summary>
        /// ID3v1.1
        /// </summary>
        [Description("ID3v1.1")]
        ID3v11
    }
}

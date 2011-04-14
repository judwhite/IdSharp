using System.IO;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// ID3v2 tag header.
    /// </summary>
    public interface IID3v2Header
    {
        /// <summary>
        /// Gets or sets the version of the ID3v2 tag.
        /// </summary>
        /// <value>The version of the ID3v2 tag.</value>
        ID3v2TagVersion TagVersion { get; set; }

        /// <summary>
        /// Gets or sets the tag version revision number.
        /// </summary>
        /// <value>The tag version revision number.</value>
        byte TagVersionRevision { get; set; }

        /// <summary>
        /// The ID3v2 tag size is the size of the complete tag after unsychronisation, 
        /// including padding, excluding the header (and footer, if present) but not excluding the extended header.
        /// <para>If a footer is present this equals to ('total size' - 20) bytes, otherwise ('total size' - 10) bytes.</para>
        /// </summary>
        /// <value>The tag size, excluding the header and footer.
        /// <para>If a footer is present this equals to ('total size' - 20) bytes, otherwise ('total size' - 10) bytes.</para>
        /// </value>
        int TagSize { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the tag uses unsynchronization.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value><c>true</c> if unsynchronization is used; otherwise, <c>false</c>.  Only valid in ID3v2.3 and higher.</value>
        bool UsesUnsynchronization { get; set; }

        /// <summary>
        /// Indicates whether or not the header is followed by an extended header.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value><c>true</c> if extended header is used; otherwise, <c>false</c>.  Only valid in ID3v2.3 and higher.</value>
        bool HasExtendedHeader { get; set; }

        /// <summary>
        /// Should be used as an 'experimental indicator'. This flag should always be set when the tag is in an
        /// experimental stage.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value><c>true</c> if experimental; otherwise, <c>false</c>.  Only valid in ID3v2.3 and higher.</value>
        bool IsExperimental { get; set; }

        /// <summary>
        /// Indicates whether or not compression is used on the entire tag.  Only valid in ID3v2.2.
        /// </summary>
        /// <value><c>true</c> if compression is used on the entire tag; otherwise <c>false</c>.  Only valid in ID3v2.2.</value>
        bool IsCompressed { get; set; }

        /// <summary>
        /// Indicates whether or not a footer is present in the tag.  Only valid in ID3v2.4.
        /// </summary>
        /// <value><c>true</c> if a footer is present in the tag; otherwise <c>false</c>.  Only valid in ID3v2.4.</value>
        bool IsFooterPresent { get; set; }

        /// <summary>
        /// Reads the raw data from a specified stream.  The Position property of the stream must be set to the position to begin reading.
        /// </summary>
        /// <param name="stream">The stream to read from.  The Position property of the stream must be set to the position to begin reading.</param>
        void Read(Stream stream);

        /// <summary>
        /// Gets the bytes of the raw data.
        /// </summary>
        /// <returns>The bytes of the raw data.</returns>
        byte[] GetBytes();
    }
}

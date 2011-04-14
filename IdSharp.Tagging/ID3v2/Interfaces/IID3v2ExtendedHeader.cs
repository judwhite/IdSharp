using System.IO;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// An ID3v2 extended tag header.  Only valid in ID3v2.3 and higher.  However, the <see cref="IID3v2ExtendedHeader.PaddingSize"/>
    /// property should be used if you wish to explicitly set the padding size in any version, including ID3v2.2.
    /// </summary>
    public interface IID3v2ExtendedHeader
    {
        /// <summary>
        /// The size of the extended header, excluding the bytes used to represent the size of the extended header.
        /// </summary>
        /// <value>The size of the extended header, excluding the bytes used to represent the size of the extended header.</value>
        int SizeExcludingSizeBytes { get; }

        /// <summary>
        /// The size of the extended header, including the bytes used to represent the size of the extended header.
        /// </summary>
        /// <value>The size of the extended header, including the bytes used to represent the size of the extended header.</value>
        int SizeIncludingSizeBytes { get; }

        /// <summary>
        /// Gets or sets whether CRC-32 data is present in the extended header.
        /// </summary>
        /// <value><c>true</c> if CRC-32 data is present; otherwise, <c>false</c>.</value>
        bool IsCRCDataPresent { get; set; }

        /// <summary>
        /// Gets or sets the size of the padding.  
        /// This property should be used to explicitly set the padding size for all tag versions, whether or not
        /// an extended header is to be written in the tag.
        /// Note: the padding size itself is only (potentially) stored in ID3v2.3 tags.  TODO: Should this
        /// value reflect actual padding size, or the stored padding size?
        /// </summary>
        /// <value>The size of the padding.</value>
        int PaddingSize { get; set; }

        /// <summary>
        /// Gets the CRC.  In ID3v2.3, the CRC is calculated before
        /// unsynchronization on the data between the extended header and the
        /// padding, i.e. the frames and only the frames.  In ID3v2.4, the padding
        /// is included in the CRC calculation.
        /// If <see cref="IsCRCDataPresent"/> is <c>false</c>, this value has no meaning.
        /// </summary>
        /// <value>The calculated CRC-32 value.  If <see cref="IsCRCDataPresent"/> is <c>false</c>, this value has no meaning.</value>
        uint CRC32 { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the present tag is an update of a tag found
        /// earlier in the present file or stream.  Only valid in ID3v2.4.
        /// </summary>
        /// <value>
        /// <c>true</c> if the present tag is an update of a tag found earlier in the present file or stream; otherwise, <c>false</c>.
        /// Only valid in ID3v2.4.
        /// </value>
        bool IsTagAnUpdate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this tag was restricted when it was written.
        /// Only valid in ID3v2.4.
        /// </summary>
        /// <value>
        /// <c>true</c> if this tag was restricted when it was written.; otherwise, <c>false</c>.
        /// Only valid in ID3v2.4.
        /// </value>
        bool IsTagRestricted { get; set; }

        /// <summary>
        /// Gets the tag restrictions.
        /// Only valid in ID3v2.4.
        /// </summary>
        /// <value>The tag restrictions.  Only valid in ID3v2.4.</value>
        ITagRestrictions TagRestrictions { get; }

        /// <summary>
        /// Reads the raw data from a specified stream.  The Position property of the stream must be set to the position to begin reading.
        /// </summary>
        /// <param name="tagReadingInfo">The reading options.  Includes the version of the ID3v2 tag and how to perform reads.</param>
        /// <param name="stream">The stream to read from.  The Position property of the stream must be set to the position to begin reading.</param>
        void ReadFrom(TagReadingInfo tagReadingInfo, Stream stream);

        /// <summary>
        /// Gets the bytes of the raw data.
        /// </summary>
        /// <returns>The bytes of the raw data.</returns>
        byte[] GetBytes(ID3v2TagVersion tagVersion);
    }
}

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// ID3v2 frame header.
    /// </summary>
    public interface IFrameHeader
    {
        /*/// <summary>
        /// Gets the frame ID.
        /// </summary>
        /// <value>The frame ID.</value>
        Byte[] FrameID { get; }*/

        /*/// <summary>
        /// Gets or sets the tag version.
        /// </summary>
        /// <value>The tag version.</value>
        ID3v2TagVersion TagVersion { get; set; }*/

        /// <summary>
        /// Gets the size of the frame, excluding the (10 or 6 byte) header, but not excluding additions to the header.
        /// </summary>
        /// <value>The size of the frame, excluding the (10 or 6 byte) header, but not excluding additions to the header.</value>
        int FrameSize { get; }

        /// <summary>
        /// Gets the total size of the frame, including the (10 or 6 byte) header and additions to the header.
        /// </summary>
        /// <value>The total size of the frame, including the (10 or 6 byte) header and additions to the header.</value>
        int FrameSizeTotal { get; }

        /// <summary>
        /// Gets the size of the frame, excluding the header and excluding additions to the header.
        /// </summary>
        /// <value>The the size of the frame, excluding the header and excluding additions to the header.</value>
        int FrameSizeExcludingAdditions { get; }

        /// <summary>
        /// This flag tells the software what to do with this frame if it is
        /// unknown and the tag is altered in any way. This applies to all
        /// kinds of alterations, including adding more padding and reordering
        /// the frames.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if tag should be preserved if unknown and any part of the ID3v2 tag is altered; otherwise, <c>false</c>.
        /// Only valid in ID3v2.3 and higher.
        /// </value>
        bool IsTagAlterPreservation { get; set; }

        /// <summary>
        /// This flag tells the software what to do with this frame if it is
        /// unknown and the file, excluding the tag, is altered. This does not
        /// apply when the audio is completely replaced with other audio data.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the tag should be preserved when the audio data is altered (excluding a complete replace); otherwise, <c>false</c>.
        /// Only valid in ID3v2.3 and higher.
        /// </value>
        bool IsFileAlterPreservation { get; set; }

        /// <summary>
        /// Tells the software that the contents of this
        /// frame is intended to be read only. Changing the contents might
        /// break something, e.g. a signature. If the contents are changed,
        /// without knowledge in why the frame was flagged read only and
        /// without taking the proper means to compensate, e.g. recalculating
        /// the signature, the bit should be cleared.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value><c>true</c> if frame is inteded to be read only; otherwise, <c>false</c>.  Only valid in ID3v2.3 and higher.</value>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this frame is compressed.
        /// If true, frame is compressed using "zlib" with 4 bytes for
        /// 'decompressed size' appended to the frame header.  See http://www.zlib.net.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value><c>true</c> if compressed; otherwise, <c>false</c>.  Only valid in ID3v2.3 and higher.</value>
        bool IsCompressed { get; set; }

        /// <summary>
        /// This flag indicates whether or not the frame is encrypted. If set
        /// one byte indicating with which method it was encrypted will be
        /// appended to the frame header. Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value><c>value</c> if encrypted; otherwise, <c>null</c>.  Only valid in ID3v2.3 and higher.</value>
        byte? EncryptionMethod { get; set; }

        /// <summary>
        /// This flag indicates whether or not this frame belongs in a group
        /// with other frames. If set a group identifier byte is added to the
        /// frame header. Every frame with the same group identifier belongs
        /// to the same group.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>Contains the grouping identity byte if grouping identity is used; otherwise, <c>null</c>.  Only valid in ID3v2.3 and higher.</value>
        byte? GroupingIdentity { get; set; }

        /// <summary>
        /// Gets or sets the size of the decompressed frame.  
        /// Value has no meaning if <see cref="IFrameHeader.IsCompressed"/> is <c>false</c>.
        /// This value is set automatically when the frame is written.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>
        /// The size of the decompressed frame.  Value has no meaning if <see cref="IFrameHeader.IsCompressed"/> is <c>false</c>.
        /// Only valid in ID3v2.3 and higher.
        /// </value>
        int DecompressedSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this frame uses unsynchronization.
        /// Only valid in ID3v2.4.
        /// </summary>
        /// <value>
        /// <c>true</c> if this frame uses unsynchronization; otherwise, <c>false</c>.
        /// Only valid in ID3v2.4.
        /// </value>
        bool UsesUnsynchronization { get; set; }

        /*/// <summary>
        /// Gets or sets the data length indicator.  The data length indicator is the value one would write
        /// as the 'Frame length' if all of the frame format flags were
        /// zeroed.  In other words, <see cref="FrameSizeExcludingAdditions"/>.
        /// Only valid in ID3v2.4.
        /// </summary>
        /// <value>
        /// <c>value</c> if a data length indicator is present; otherwise, <c>null</c>.
        /// Only valid in ID3v2.4.
        /// </value>
        int? DataLengthIndicator { get; set; }*/

        /*/// <summary>
        /// Reads the raw data from a specified stream.  The Position property of the stream must be set to the position to begin reading.
        /// </summary>
        /// <param name="tagReadingInfo">The reading options.  Includes the version of the ID3v2 tag and how to perform reads.</param>
        /// <param name="stream">The stream to read from.  The Position property of the stream must be set to the position to begin reading.  If the frame is compressed an uncompressed stream will be written to the stream argument.</param>
        void ReadFrom(TagReadingInfo tagReadingInfo, ref Stream stream);*/

        /*/// <summary>
        /// Gets the bytes of the raw data.
        /// </summary>
        /// <returns>The bytes of the raw data.</returns>
        Byte[] GetBytes(TagVersion tagVersion, String frameID);*/
    }
}

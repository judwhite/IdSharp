using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Audio Seek Point Index.
    /// Note: The presence of an ASPI frame requires the existence of a TLEN frame.
    /// </summary>
    public interface IAudioSeekPointIndex : IFrame
    {
        /// <summary>
        /// Gets or sets the start of the indexed data, which is a byte offset from the beginning of the file.
        /// </summary>
        /// <value>The start of the indexed data, which is a byte offset from the beginning of the file.</value>
        int IndexedDataStart { get; set; }

        /// <summary>
        /// Gets or sets the length of the indexed audio data in bytes.
        /// </summary>
        /// <value>The length of the indexed audio data in bytes.</value>
        int IndexedDataLength { get; set; }

        /// <summary>
        /// Gets or sets the number of bits to use per index point.  This value must be 8 or 16.
        /// </summary>
        /// <value>The number of bits to use per index point.  This value must be 8 or 16.</value>
        byte BitsPerIndexPoint { get; set; }

        /// <summary>
        /// FractionAtIndex = (Offset at index)/(IndexedDataLength) * (2^BitsPerIndexPoint)    (rounded down to the nearest integer)
        /// TODO: Add a new type and methods to make this frame easier to use
        /// </summary>
        BindingList<short> FractionAtIndex { get; }
    }
}

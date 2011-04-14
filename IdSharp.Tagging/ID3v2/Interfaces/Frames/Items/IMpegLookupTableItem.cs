using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// An MPEG lookup table item.  See <see cref="IMpegLookupTable"/>.
    /// </summary>
    public interface IMpegLookupTableItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the deviation in bytes.  This value describes the difference between the actual
        /// byte offset of this <see cref="IMpegLookupTableItem"/> instance and the offset indicated by its position
        /// and <see cref="IMpegLookupTable.BytesBetweenReference"/>.
        /// </summary>
        /// <value>The deviation in bytes.</value>
        long DeviationInBytes { get; set; }

        /// <summary>
        /// Gets or sets the deviation in milliseconds.  This value describes the difference between the actual
        /// millisecond offset of this <see cref="IMpegLookupTableItem"/> instance and the offset indicated by its position
        /// and <see cref="IMpegLookupTable.MillisecondsBetweenReference"/>.
        /// </summary>
        /// <value>The deviation in milliseconds.</value>
        long DeviationInMilliseconds { get; set; }
    }
}

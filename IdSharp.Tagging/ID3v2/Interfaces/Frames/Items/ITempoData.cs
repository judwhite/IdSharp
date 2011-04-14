using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// A tempo item containing a TempoCode and Timestamp.  <see cref="ISynchronizedTempoCodes"/> is the container for <see cref="ITempoData"/> items.
    /// </summary>
    public interface ITempoData : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the tempo code. 0x00 is used to describe a beat-free time period, which is
        /// not the same as a music-free time period.  0x01 is used to indicate one
        /// single beat-stroke followed by a beat-free period.  The maximum value of this field is 510.
        /// </summary>
        /// <value>The tempo code.</value>
        short TempoCode { get; set; }

        /// <summary>
        /// Gets or sets the timestamp in either milliseconds or frames according to <see cref="ISynchronizedTempoCodes"/>).
        /// </summary>
        /// <value>The timestamp in either milliseconds or frames according to <see cref="ISynchronizedTempoCodes"/>).</value>
        int Timestamp { get; set; }
    }
}

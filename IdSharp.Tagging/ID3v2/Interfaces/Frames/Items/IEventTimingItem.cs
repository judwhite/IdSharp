using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// Event timing item.  See <see cref="IEventTiming"/>.
    /// </summary>
    public interface IEventTimingItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        MusicEvent EventType { get; set; }

        /// <summary>
        /// Gets or sets the timestamp in milliseconds or frames according to <see cref="IEventTiming.TimestampFormat"/>.
        /// </summary>
        /// <value>The timestamp in milliseconds or frames according to <see cref="IEventTiming.TimestampFormat"/>.</value>
        int Timestamp { get; set; }
    }
}

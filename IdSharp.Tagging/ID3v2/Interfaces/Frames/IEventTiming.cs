using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Event timing codes</para>
    ///
    /// <para>This frame allows synchronization with key events in a song or sound.</para>
    /// </summary>
    public interface IEventTiming : IFrame
    {
        /// <summary>
        /// Gets or sets the time stamp format.
        /// </summary>
        /// <value>The time stamp format.</value>
        TimestampFormat TimestampFormat { get; set; }

        /// <summary>
        /// Gets the BindingList of <see cref="IEventTimingItem"/> items.
        /// </summary>
        /// <value>The BindingList of <see cref="IEventTimingItem"/> items.</value>
        BindingList<IEventTimingItem> Items { get; }
    }
}

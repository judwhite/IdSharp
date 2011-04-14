using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// Synchronized lyrics/text item.  See <see cref="ISynchronizedText"/>.
    /// </summary>
    public interface ISynchronizedTextItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the timestamp in milliseconds or frames according to <see cref="ISynchronizedText.TimestampFormat"/>.
        /// </summary>
        /// <value>The timestamp in milliseconds or frames according to <see cref="ISynchronizedText.TimestampFormat"/>.</value>
        int Timestamp { get; set; }
    }
}

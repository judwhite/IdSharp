using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// Musician credits item.  See <see cref="IMusicianCreditsList"/>.
    /// </summary>
    public interface IMusicianCreditsItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the instrument.
        /// </summary>
        /// <value>The instrument.</value>
        string Instrument { get; set; }

        /// <summary>
        /// Gets or sets the artists.
        /// </summary>
        /// <value>The artists.</value>
        string Artists { get; set; }
    }
}

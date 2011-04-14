using System.ComponentModel;

namespace IdSharp.Tagging.SimpleTag
{
    /// <summary>
    /// ISimpleTag interface.  See <see cref="SimpleTag" />.
    /// </summary>
    public interface ISimpleTag : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>The artist.</value>
        string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        /// <value>The album.</value>
        string Album { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        string Year { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        string Comment { get; set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        /// <value>The track number.</value>
        string TrackNumber { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>The genre.</value>
        string Genre { get; set; }

        /// <summary>
        /// Gets the tag type and version.
        /// </summary>
        /// <value>The tag type and version.</value>
        string TagVersion { get; }

        /// <summary>
        /// Saves the tag.
        /// </summary>
        void Save();
    }
}

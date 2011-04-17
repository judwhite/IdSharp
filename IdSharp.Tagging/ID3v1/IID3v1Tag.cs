using System.ComponentModel;
using System.IO;

namespace IdSharp.Tagging.ID3v1
{
    /// <summary>
    /// ID3v1 interface.
    /// </summary>
    public interface IID3v1Tag : INotifyPropertyChanged
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
        int? TrackNumber { get; set; }

        /// <summary>
        /// Gets or sets the index of the genre.
        /// </summary>
        /// <value>The index of the genre.</value>
        int GenreIndex { get; set; }

        /// <summary>
        /// Reads the ID3v1 tag from the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        void Read(string path);

        /// <summary>
        /// Reads the ID3v1 tag from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        void Read(Stream stream);

        /// <summary>
        /// Saves the ID3v1 tag to the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        void Save(string path);

        /// <summary>
        /// Resets the properties of the ID3v1 tag to their default values.
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets or sets the ID3v1 tag version.
        /// </summary>
        /// <value>The ID3v1 tag version.</value>
        ID3v1TagVersion TagVersion { get; set; }
    }
}

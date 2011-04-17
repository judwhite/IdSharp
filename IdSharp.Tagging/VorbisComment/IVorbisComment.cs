using System.IO;

namespace IdSharp.Tagging.VorbisComment
{
    /// <summary>
    /// Vorbis Comment
    /// </summary>
    public interface IVorbisComment
    {
        /// <summary>Saves the tag.</summary>
        /// <param name="path">The path.</param>
        void Save(string path);

        /// <summary>Reads the tag.</summary>
        /// <param name="path">The path.</param>
        void Read(string path);

        /// <summary>Reads the tag.</summary>
        /// <param name="stream">The stream.</param>
        void Read(Stream stream);

        /// <summary>
        /// Gets the vendor specified in the Vorbis Comment header.
        /// </summary>
        /// <value>The vendor specified in the Vorbis Comment header..</value>
        string Vendor { get; }

        /// <summary>Gets or sets the artist.</summary>
        /// <value>The artist.</value>
        string Artist { get; set; }

        /// <summary>Gets or sets the album.</summary>
        /// <value>The album.</value>
        string Album { get; set; }

        /// <summary>Gets or sets the title.</summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>Gets or sets the year.</summary>
        /// <value>The year.</value>
        string Year { get; set; }

        /// <summary>Gets or sets the genre.</summary>
        /// <value>The genre.</value>
        string Genre { get; set; }

        /// <summary>Gets or sets the track number.</summary>
        /// <value>The track number.</value>
        string TrackNumber { get; set; }

        /// <summary>Gets or sets the comment.</summary>
        /// <value>The comment.</value>
        string Comment { get; set; }

        /// <summary>Gets the Name/Value item list.</summary>
        /// <value>The Name/Value item list.</value>
        NameValueList Items { get; }
    }
}

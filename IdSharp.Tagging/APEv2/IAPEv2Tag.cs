using System;
using System.ComponentModel;
using System.IO;

namespace IdSharp.Tagging.APEv2
{
    /// <summary>
    /// Provides methods for reading, writing, and updating APEv2 tags.
    /// </summary>
    public interface IAPEv2Tag : INotifyPropertyChanged
    {
        /// <summary>Reads the raw data from a specified file.</summary>
        /// <param name="path">The file to read from.</param>
        void Read(string path);

        /// <summary>Reads the raw data from a specified stream.</summary>
        /// <param name="stream">The stream to read from.</param>
        void Read(Stream stream);

        /// <summary>Gets the bytes of the current APEv2 tag.</summary>
        /// <returns>The bytes of the current APEv2 tag.</returns>
        byte[] GetBytes();

        /// <summary>Gets or sets the title.</summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>Gets or sets the artist.</summary>
        /// <value>The artist.</value>
        string Artist { get; set; }

        /// <summary>Gets or sets the album.</summary>
        /// <value>The album.</value>
        string Album { get; set; }

        /// <summary>Gets or sets the publisher.</summary>
        /// <value>The publisher.</value>
        string Publisher { get; set; }

        /// <summary>Gets or sets the track number.</summary>
        /// <value>The track number.</value>
        string TrackNumber { get; set; }

        /// <summary>Gets or sets the comment.</summary>
        /// <value>The comment.</value>
        string Comment { get; set; }

        /// <summary>Gets or sets the catalog.</summary>
        /// <value>The catalog.</value>
        string Catalog { get; set; }

        /// <summary>Gets or sets the year.</summary>
        /// <value>The year.</value>
        string Year { get; set; }

        /// <summary>Gets or sets the record date.</summary>
        /// <value>The record date.</value>
        string RecordDate { get; set; }

        /// <summary>Gets or sets the genre.</summary>
        /// <value>The genre.</value>
        string Genre { get; set; }

        /// <summary>Gets or sets the media.</summary>
        /// <value>The media.</value>
        string Media { get; set; }

        /// <summary>Gets or sets the language.</summary>
        /// <value>The language.</value>
        string Language { get; set; }

        /*string Copyright { get; set; }
        string DiscNumber { get; set; }
        string ISRC { get; set; }
        string EANUPN { get; set; }
        string Label { get; set; }
        string LabelNumber { get; set; }
        string License { get; set; }
        string Opus { get; set; }
        string SourceMedia { get; set; }
        string TrackNumber { get; set; }
        string Version { get; set; }
        string EncodedBy { get; set; }
        string Encoding { get; set; }

        string Composer { get; set; }
        string Arranger { get; set; }
        string Lyricist { get; set; }
        string Author { get; set; }
        string Conductor { get; set; }
        string Performer { get; set; }
        string Ensemble { get; set; }
        string Part { get; set; }
        string PartNumber { get; set; }
        string Date { get; set; }
        string Location { get; set; }*/
    }
}

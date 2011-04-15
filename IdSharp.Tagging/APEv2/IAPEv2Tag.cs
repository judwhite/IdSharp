using System.ComponentModel;
using System.IO;

namespace IdSharp.Tagging.APEv2
{
    /// <summary>
    /// Provides methods for reading, writing, and updating APEv2 tags.
    /// </summary>
    public interface IAPEv2Tag : INotifyPropertyChanged
    {
        /// <summary>
        /// Reads the raw data from a specified file.
        /// </summary>
        /// <param name="path">The file to read from.</param>
        void Read(string path);

        /// <summary>
        /// Reads the raw data from a specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        void ReadStream(Stream stream);

        /// <summary>
        /// Gets the bytes of the current APEv2 tag.
        /// </summary>
        /// <returns>The bytes of the current APEv2 tag.</returns>
        byte[] GetBytes();

        /*String Album { get; set; }
        String Artist { get; set; }
        String Publisher { get; set; }
        String Copyright { get; set; }
        String DiscNumber { get; set; }
        String ISRC { get; set; }
        String EANUPN { get; set; }
        String Label { get; set; }
        String LabelNumber { get; set; }
        String License { get; set; }
        String Opus { get; set; }
        String SourceMedia { get; set; }
        String Title { get; set; }
        String TrackNumber { get; set; }
        String Version { get; set; }
        String EncodedBy { get; set; }
        String Encoding { get; set; }

        String Composer { get; set; }
        String Arranger { get; set; }
        String Lyricist { get; set; }
        String Author { get; set; }
        String Conductor { get; set; }
        String Performer { get; set; }
        String Ensemble { get; set; }
        String Part { get; set; }
        String PartNumber { get; set; }
        String Genre { get; set; }
        String Date { get; set; }
        String Location { get; set; }
        String Comment { get; set; }*/
    }
}

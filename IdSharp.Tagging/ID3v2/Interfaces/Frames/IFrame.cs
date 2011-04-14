using System.ComponentModel;
using System.IO;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// An arbitrary ID3v2 frame.  This interface is implemented by all frame types.
    /// </summary>
    public interface IFrame : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the frame header.
        /// </summary>
        /// <value>The frame header.</value>
        IFrameHeader FrameHeader { get; }

        /// <summary>
        /// Gets the FrameID of the frame for a specified tag version.
        /// </summary>
        /// <param name="tagVersion">The tag version.</param>
        /// <returns>The FrameID of the frame for a specified tag version</returns>
        string GetFrameID(ID3v2TagVersion tagVersion);

        /// <summary>
        /// Reads the raw data from a specified stream.  The Position property of the stream must be set to the position to begin reading.
        /// </summary>
        /// <param name="tagReadingInfo">The reading options.  Includes the version of the ID3v2 tag and how to perform reads.</param>
        /// <param name="stream">The stream to read from.  The Position property of the stream must be set to the position to begin reading.</param>
        void Read(TagReadingInfo tagReadingInfo, Stream stream);

        /// <summary>
        /// Gets the bytes of the raw data.
        /// </summary>
        /// <returns>The bytes of the raw data.</returns>
        byte[] GetBytes(ID3v2TagVersion tagVersion);
    }
}

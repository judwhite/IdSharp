using System.Collections.Generic;
using System.IO;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// Provides methods for reading, writing, and updating ID3v2 tags.
    /// </summary>
    public interface IID3v2Tag : IFrameContainer
    {
        /// <summary>
        /// Gets the ID3v2 header.
        /// </summary>
        /// <value>The ID3v2 header.</value>
        IID3v2Header Header { get; }

        /// <summary>
        /// Gets the ID3v2 extended header.
        /// </summary>
        /// <value>The ID3v2 extended header.</value>
        IID3v2ExtendedHeader ExtendedHeader { get; }

        /// <summary>
        /// Reads the raw data from a specified file.
        /// </summary>
        /// <param name="path">The file to read from.</param>
        void Read(string path);

        /// <summary>
        /// Reads the raw data from a specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        void Read(Stream stream);

        /// <summary>
        /// Saves the tag to the specified path.
        /// </summary>
        /// <param name="path">The path to save the tag.</param>
        void Save(string path);

        /// <summary>
        /// Gets all frames in the ID3v2 tag as a collection of IFrames.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag as a collection of IFrames.</returns>
        List<IFrame> GetFrames();

        /// <summary>
        /// Gets all frames in the ID3v2 tag with the specified <paramref name="frameID"/> as a collection of IFrames.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag with the specified <paramref name="frameID"/> as a collection of IFrames.</returns>
        List<IFrame> GetFrames(string frameID);

        /// <summary>
        /// Gets all frames in the ID3v2 tag with the specified <paramref name="frameIDs"/> as a collection of IFrames.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag with the specified <paramref name="frameIDs"/> as a collection of IFrames.</returns>
        List<IFrame> GetFrames(IEnumerable<string> frameIDs);

        /// <summary>
        /// Gets all frames in the ID3v2 tag which implement the specified interface <typeparamref name="T"/>.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag which implement the specified interface <typeparamref name="T"/>.</returns>
        List<T> GetFrames<T>();

        /// <summary>
        /// Gets the bytes of the current ID3v2 tag.
        /// </summary>
        /// <param name="minimumSize">The minimum size of the new tag, including the header and footer.</param>
        /// <returns>The bytes of the current ID3v2 tag.</returns>
        byte[] GetBytes(int minimumSize);
    }
}

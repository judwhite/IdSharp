using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.Lyrics3
{
    public partial class Lyrics3Tag
    {
        /// <summary>
        /// Gets the Lyrics3 tag size from a specified stream. Returns 0 if no tag exists.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static int GetTagSize(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return new Lyrics3Tag(stream).TotalTagSize;
        }

        /// <summary>
        /// Gets the Lyrics3 tag size from a specified path. Returns 0 if no tag exists.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public static int GetTagSize(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return GetTagSize(fileStream);
            }
        }

        /// <summary>
        /// Returns <c>true</c> if an Lyrics3 tag exists in the specified stream; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static bool DoesTagExist(Stream stream)
        {
            return (GetTagSize(stream) != 0);
        }

        /// <summary>
        /// Returns <c>true</c> if an Lyrics3 tag exists in the specified path; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public static bool DoesTagExist(string path)
        {
            return (GetTagSize(path) != 0);
        }

        /// <summary>
        /// Removes an Lyrics3 tag from a specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <returns><c>true</c> if an Lyrics3 tag was removed; otherwise, <c>false</c>.</returns>
        public static bool RemoveTag(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            Lyrics3Tag lyrics3 = new Lyrics3Tag(path);
            int tagSize = lyrics3.TotalTagSize;

            if (tagSize > 0)
            {
                long tagOffset = lyrics3.TagOffset.Value;
                ByteUtils.ReplaceBytes(path, tagSize, new byte[0], tagOffset);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

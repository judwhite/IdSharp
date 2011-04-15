using System;
using System.IO;

namespace IdSharp.Tagging.ID3v1
{
    public partial class ID3v1Tag
    {
        /// <summary>
        /// Gets the ID3v1 tag size from a specified stream.  Returns 128 if an ID3v1 tag exists; otherwise, 0.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static int GetTagSize(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            long currentPosition = stream.Position;
            try
            {
                if (stream.Length >= 128)
                {
                    stream.Seek(-128, SeekOrigin.End);

                    byte[] buf = new byte[3];
                    stream.Read(buf, 0, 3);

                    // Check for 'TAG'
                    if (buf[0] == 0x54 && buf[1] == 0x41 && buf[2] == 0x47)
                    {
                        return 128;
                    }
                }
                return 0;
            }
            finally
            {
                stream.Position = currentPosition;
            }
        }

        /// <summary>
        /// Gets the ID3v1 tag size from a specified path.  Returns 128 if an ID3v1 tag exists; otherwise, 0.
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
        /// Returns <c>true</c> if an ID3v1 tag exists in the specified stream; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static bool DoesTagExist(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return (GetTagSize(stream) != 0);
        }

        /// <summary>
        /// Returns <c>true</c> if an ID3v1 tag exists in the specified path; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public static bool DoesTagExist(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            return (GetTagSize(path) != 0);
        }

        /// <summary>
        /// Removes an ID3v1 tag from a specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <returns><c>true</c> if an ID3v1 tag was removed; otherwise, <c>false</c>.</returns>
        public static bool RemoveTag(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                if (!DoesTagExist(fileStream))
                    return false;

                fileStream.SetLength(fileStream.Length - 128);
            }

            return true;
        }
    }
}

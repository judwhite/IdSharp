using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2
{
    public partial class ID3v2Tag
    {
        /// <summary>
        /// Gets the ID3v2 tag size from a specified stream.  Returns 0 if no tag exists.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static int GetTagSize(Stream stream)
        {
            try
            {
                if (stream.Length >= 16)
                {
                    stream.Position = 0;

                    byte[] identifier = stream.Read(3);

                    // 'ID3' identifier
                    if (!(identifier[0] == 0x49 && identifier[1] == 0x44 && identifier[2] == 0x33))
                    {
                        return 0;
                    }

                    IID3v2Header header = new ID3v2Header(stream, false);
                    int tagSize = header.TagSize;
                    if (tagSize != 0)
                        return tagSize + 10 + (header.IsFooterPresent ? 10 : 0);
                    else
                        return 0;
                }
                return 0;
            }
            finally
            {
                stream.Position = 0;
            }
        }

        /// <summary>
        /// Gets the ID3v2 tag size from a specified path.  Returns 128 if an ID3v2 tag exists; otherwise, 0.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public static int GetTagSize(string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return GetTagSize(fileStream);
            }
        }

        /// <summary>
        /// Returns <c>true</c> if an ID3v2 tag exists in the specified stream; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static bool DoesTagExist(Stream stream)
        {
            return (GetTagSize(stream) != 0);
        }

        /// <summary>
        /// Returns <c>true</c> if an ID3v2 tag exists in the specified path; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public static bool DoesTagExist(string path)
        {
            return (GetTagSize(path) != 0);
        }

        /// <summary>
        /// Removes an ID3v2 tag from a specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <returns><c>true</c> if an ID3v2 tag was removed; otherwise, <c>false</c>.</returns>
        public static bool RemoveTag(string path)
        {
            int tagSize = GetTagSize(path);
            if (tagSize > 0)
            {
                ByteUtils.ReplaceBytes(path, tagSize, new byte[0]);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

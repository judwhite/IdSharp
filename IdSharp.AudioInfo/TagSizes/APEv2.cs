using System;
using System.IO;
using System.Text;
using IdSharp.Common.Utils;

namespace IdSharp.AudioInfo
{
    internal static class APEv2
    {
        private static readonly byte[] APETAGEX = Encoding.ASCII.GetBytes("APETAGEX");

        /// <summary>
        /// Gets the APEv2 tag size from a specified stream.  Returns 0 if no tag exists.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static int GetTagSize(Stream stream)
        {
            Int64 currentPosition = stream.Position;
            try
            {
                byte[] buf = new byte[32];
                stream.Seek(-32, SeekOrigin.End);
                stream.Read(buf, 0, 32);

                if (ByteUtils.Compare(buf, APETAGEX, 8) == false)
                {
                    // Skip past possible ID3v1 tag
                    stream.Seek(-128 - 32, SeekOrigin.End);
                    stream.Read(buf, 0, 32);
                    if (ByteUtils.Compare(buf, APETAGEX, 8) == false)
                    {
                        // TODO: skip past possible Lyrics3 tag
                        return 0;
                    }
                }

                // Check version
                int version = 0;
                for (int i = 8; i < 12; i++)
                {
                    version += (buf[i] << ((i - 8) * 8));
                }

                // Must be APEv2 or APEv1
                if (version != 2000 && version != 1000)
                {
                    return 0;
                }

                // Size
                int tagSize = 0;
                for (int i = 12; i < 16; i++)
                {
                    tagSize += (buf[i] << ((i - 12) * 8));
                }

                bool containsHeader = ((buf[23] >> 7) == 1);

                tagSize += (containsHeader ? 32 : 0);
                return tagSize;
            }
            finally
            {
                stream.Position = currentPosition;
            }
        }
    }
}

using System;
using System.IO;

namespace IdSharp.AudioInfo
{
    internal static class ID3v1
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
    }
}

using System.Diagnostics;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.AudioInfo
{
    internal static class ID3v2
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

                    // Identifier 'ID3'
                    if (!(identifier[0] == 0x49 && identifier[1] == 0x44 && identifier[2] == 0x33))
                    {
                        return 0;
                    }

                    ID3v2Header header = new ID3v2Header(stream);
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

        private sealed class ID3v2Header
        {
            private readonly ID3v2TagVersion _tagVersion;
            private readonly int _tagSize;
            private readonly bool _isFooterPresent;

            public ID3v2Header(Stream stream)
            {
                byte[] tmpHeader = stream.Read(7);

                // Version
                if (tmpHeader[0] < 2 || tmpHeader[0] > 4)
                {
                    string msg = string.Format("ID3 Version '{0}' not recognized (valid versions are 2, 3, and 4)", tmpHeader[0]);
                    Trace.WriteLine(msg);
                    throw new InvalidDataException(msg);
                }

                _tagVersion = (ID3v2TagVersion)tmpHeader[0];

                // Flags
                switch (_tagVersion)
                {
                    case ID3v2TagVersion.ID3v23:
                        _isFooterPresent = false;
                        break;
                    case ID3v2TagVersion.ID3v22:
                        _isFooterPresent = false;
                        break;
                    case ID3v2TagVersion.ID3v24:
                        _isFooterPresent = ((tmpHeader[2] & 0x10) == 0x10);
                        break;
                }

                // Size
                _tagSize = (tmpHeader[3] << 21);
                _tagSize += (tmpHeader[4] << 14);
                _tagSize += (tmpHeader[5] << 7);
                _tagSize += (tmpHeader[6]);
            }

            public int TagSize
            {
                get
                {
                    return _tagSize;
                }
            }

            public bool IsFooterPresent
            {
                get
                {
                    if (_tagVersion == ID3v2TagVersion.ID3v24)
                        return _isFooterPresent;
                    else
                        return false;
                }
            }

            /// <summary>
            /// ID3v2 tag version.
            /// </summary>
            private enum ID3v2TagVersion : byte
            {
                /// <summary>
                /// ID3v2.2.
                /// </summary>
                ID3v22 = 2,
                /// <summary>
                /// ID3v2.3.
                /// </summary>
                ID3v23 = 3,
                /// <summary>
                /// ID3v2.4.
                /// </summary>
                ID3v24 = 4
            }
        }
    }
}

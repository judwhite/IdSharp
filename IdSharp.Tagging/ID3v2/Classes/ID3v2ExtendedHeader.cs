using System;
using System.Diagnostics;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2
{
    internal sealed class ID3v2ExtendedHeader : IID3v2ExtendedHeader
    {
        private bool _isCRCDataPresent;
        private int _paddingSize;
        private uint _totalFrameCRC;

        public ID3v2ExtendedHeader(TagReadingInfo tagReadingInfo, Stream stream)
        {
            ReadFrom(tagReadingInfo, stream);
        }

        public ID3v2ExtendedHeader()
        {
            Clear();
        }

        private void Clear()
        {
            _isCRCDataPresent = false;
            _paddingSize = 0;
            _totalFrameCRC = 0;
        }

        #region IID3v2ExtendedHeader Members

        public int SizeExcludingSizeBytes
        {
            get
            {
                // TODO - not the same for all formats
                return (_isCRCDataPresent ? 10 : 6);
            }
        }

        public int SizeIncludingSizeBytes
        {
            get
            {
                // TODO - not the same for all formats
                return (_isCRCDataPresent ? 10 : 6) + 4;
            }
        }

        public bool IsCRCDataPresent
        {
            get
            {
                return _isCRCDataPresent;
            }
            set
            {
                if (_isCRCDataPresent != value)
                {
                    _isCRCDataPresent = value;
                    if (value == false)
                    {
                        CRC32 = 0;
                    }
                }
            }
        }

        public int PaddingSize
        {
            get
            {
                return _paddingSize;
            }
            set
            {
                if (_paddingSize != value)
                {
                    _paddingSize = value;
                }
            }
        }

        public uint CRC32
        {
            get
            {
                return _totalFrameCRC;
            }
            set
            {
                if (_totalFrameCRC != value)
                {
                    _totalFrameCRC = value;
                }
            }
        }

        #endregion

        #region IRawData Members

        public void ReadFrom(TagReadingInfo tagReadingInfo, Stream stream)
        {
            //Guard.ArgumentNotNull(stream, "stream");

            int size = stream.ReadInt32();

            // Test for a possible FrameID (0x41 = 'A')
            // Most likely the extended header bit is set but
            // there is no extended header.  Set the stream back
            // to its original position and return.
            if (size >= 0x41000000)
            {
                string msg = string.Format("FrameID found when expected extended header at position {0}", stream.Position - 4);
                Trace.WriteLine(msg);

                stream.Seek(-4, SeekOrigin.Current);
                _isCRCDataPresent = false;
                _paddingSize = 0;
                _totalFrameCRC = 0;
                return;
            }

            byte flags1 = stream.Read1();
            byte flags2 = stream.Read1();

            _isCRCDataPresent = ((flags1 & 0x80) == 0x80);

            _paddingSize = stream.ReadInt32();

            if (_isCRCDataPresent)
            {
                _totalFrameCRC = (uint)stream.ReadInt32();
            }
            else
            {
                _totalFrameCRC = 0;
            }
        }

        public byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            byte[] buf = new byte[_isCRCDataPresent ? 14 : 10];

            // Size (excluding these 4 bytes)
            buf[0] = 0;
            buf[1] = 0;
            buf[2] = 0;
            buf[3] = (byte)(_isCRCDataPresent ? 10 : 6);

            // Flags
            buf[4] = (Byte)(_isCRCDataPresent ? 0x80 : 0);
            buf[5] = 0;

            // Padding size
            buf[6] = (Byte)((_paddingSize >> 24) & 0xFF);
            buf[7] = (Byte)((_paddingSize >> 16) & 0xFF);
            buf[8] = (Byte)((_paddingSize >> 8) & 0xFF);
            buf[9] = (Byte)(_paddingSize & 0xFF);

            if (_isCRCDataPresent)
            {
                // Total Frame CRC
                buf[10] = (Byte)((_totalFrameCRC >> 24) & 0xFF);
                buf[11] = (Byte)((_totalFrameCRC >> 16) & 0xFF);
                buf[12] = (Byte)((_totalFrameCRC >> 8) & 0xFF);
                buf[13] = (Byte)(_totalFrameCRC & 0xFF);
            }

            return buf;
        }

        public bool IsTagAnUpdate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsTagRestricted
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ITagRestrictions TagRestrictions
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}

using System;
using System.Diagnostics;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2
{
    internal sealed class ID3v2Header : IID3v2Header
    {
        private ID3v2TagVersion _tagVersion;
        private byte _tagVersionRevision;
        private int _tagSize;
        private bool _usesUnsynchronization;
        private bool _hasExtendedHeader;
        private bool _isExperimental;
        private bool _isCompressed;
        private bool _isFooterPresent;

        public ID3v2Header(Stream stream, bool readIdentifier)
        {
            Read(stream, readIdentifier);
        }

        public ID3v2Header()
        {
            Clear();
        }

        private void Clear()
        {
            _tagVersion = ID3v2TagVersion.ID3v23;
            _tagVersionRevision = 0;
            _tagSize = 0;
            _usesUnsynchronization = false;
            _hasExtendedHeader = false;
            _isExperimental = false;
        }

        #region IID3v2Header Members

        public ID3v2TagVersion TagVersion
        {
            get
            {
                return _tagVersion;
            }
            set
            {
                //Guard.ArgumentEnumDefined(typeof(TagVersion), value, "value");
                _tagVersion = value;
            }
        }

        public byte TagVersionRevision
        {
            get
            {
                return _tagVersionRevision;
            }
            set
            {
                // TODO: Logic check here?
                _tagVersionRevision = value;
            }
        }

        public int TagSize
        {
            get 
            { 
                return _tagSize; 
            }
            set
            {
                if (value > 0xFFFFFFF)
                {
                    string msg = string.Format("Argument 'value' out of range.  Maximum tag size is {0}.", 0xFFFFFFF);
                    Trace.WriteLine(msg);
                    throw new ArgumentOutOfRangeException("value", value, msg);
                }
                else if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                _tagSize = value;
            }
        }

        public bool UsesUnsynchronization
        {
            get { return _usesUnsynchronization; }
            set 
            { 
                _usesUnsynchronization = value; 
            }
        }

        public bool HasExtendedHeader
        {
            get { return _hasExtendedHeader; }
            set { _hasExtendedHeader = value; }
        }

        public bool IsExperimental
        {
            get
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                    return _isExperimental;
                else
                    return false;
            }
            set
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                    _isExperimental = value;
                else
                    _isExperimental = false;
            }
        }

        public bool IsCompressed 
        {
            get 
            {
                if (_tagVersion == ID3v2TagVersion.ID3v22)
                    return _isCompressed;
                else
                    return false;
            }
            set 
            {
                if (_tagVersion == ID3v2TagVersion.ID3v22)
                    _isCompressed = value;
                else
                    _isCompressed = false;
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
            set 
            {
                if (_tagVersion == ID3v2TagVersion.ID3v24)
                    _isFooterPresent = value;
                else
                    _isFooterPresent = false;
            }
        }

        #endregion

        private void Read(Stream stream, bool readIdentifier)
        {
            if (readIdentifier)
            {
                Read(stream);
            }
            else
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

                // Version revision
                _tagVersionRevision = tmpHeader[1];

                // Flags
                switch (_tagVersion)
                {
                    case ID3v2TagVersion.ID3v23:
                        UsesUnsynchronization = ((tmpHeader[2] & 0x80) == 0x80);
                        /*if (this.UsesUnsynchronization == true)
                        {
                            Console.WriteLine(((FileStream)stream).Name);
                        }*/
                        _hasExtendedHeader = ((tmpHeader[2] & 0x40) == 0x40);
                        _isExperimental = ((tmpHeader[2] & 0x20) == 0x20);
                        _isFooterPresent = false;
                        _isCompressed = false;
                        break;
                    case ID3v2TagVersion.ID3v22:
                        _usesUnsynchronization = ((tmpHeader[2] & 0x80) == 0x80);
                        _isFooterPresent = false;
                        _isCompressed = ((tmpHeader[2] & 0x40) == 0x40);
                        break;
                    case ID3v2TagVersion.ID3v24:
                        _usesUnsynchronization = ((tmpHeader[2] & 0x80) == 0x80);
                        _hasExtendedHeader = ((tmpHeader[2] & 0x40) == 0x40);
                        _isExperimental = ((tmpHeader[2] & 0x20) == 0x20);
                        _isFooterPresent = ((tmpHeader[2] & 0x10) == 0x10);
                        _isCompressed = false;
                        break;
                }

                // Size
                _tagSize = (tmpHeader[3] << 21);
                _tagSize += (tmpHeader[4] << 14);
                _tagSize += (tmpHeader[5] << 7);
                _tagSize += (tmpHeader[6]);
            }
        }

        #region IRawData Members

        public void Read(Stream stream)
        {
            //Guard.ArgumentNotNull(stream, "stream");

            byte[] header = stream.Read(3);

            // Identifier
            if (!(header[0] == 0x49 && header[1] == 0x44 && header[2] == 0x33))
            {
                const string msg = "'ID3' marker not found";
                Trace.WriteLine(msg);
                throw new InvalidDataException(msg);
            }

            Read(stream, false);
        }

        public byte[] GetBytes()
        {
            byte[] header = new byte[10];

            // Identifier
            header[0] = 0x49; // 'I'
            header[1] = 0x44; // 'D'
            header[2] = 0x33; // '3'

            // Tag version/revision
            header[3] = (byte)_tagVersion;
            header[4] = _tagVersionRevision;

            // Flags
            header[5] = 0;

            if (_usesUnsynchronization) header[5] += 0x80;
            if (_hasExtendedHeader) header[5] += 0x40;
            if (_isExperimental) header[5] += 0x20;

            // Syncsafe size
            header[6] = (byte)((_tagSize >> 21) & 0x7F);
            header[7] = (byte)((_tagSize >> 14) & 0x7F);
            header[8] = (byte)((_tagSize >> 7) & 0x7F);
            header[9] = (byte)(_tagSize & 0x7F);

            return header;
        }

        #endregion
    }
}

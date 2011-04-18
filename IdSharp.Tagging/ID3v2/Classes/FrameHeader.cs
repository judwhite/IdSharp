using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2
{
    internal sealed class FrameHeader : IFrameHeader
    {
        private int _frameSize;
        private int _frameSizeExcludingAdditions;
        private bool _isTagAlterPreservation;
        private bool _isFileAlterPreservation;
        private bool _isReadOnly;
        private bool _isCompressed;
        private byte? _encryptionMethod;
        private byte? _groupingIdentity;
        private int _decompressedSize;
        private ID3v2TagVersion _tagVersion;

        public ID3v2TagVersion TagVersion
        {
            get { return _tagVersion; }
        }

        #region IFrameHeader Members

        public int FrameSize
        {
            get { return _frameSize; }
        }

        public int FrameSizeTotal
        {
            get { return _frameSize + (_tagVersion == ID3v2TagVersion.ID3v22 ? 6 : 10); }
        }

        public int FrameSizeExcludingAdditions
        {
            get { return _frameSizeExcludingAdditions; }
        }

        public bool IsTagAlterPreservation
        {
            get
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    return _isTagAlterPreservation;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    _isTagAlterPreservation = value;
                }
                else
                {
                    _isTagAlterPreservation = false;
                }
            }
        }

        public bool IsFileAlterPreservation
        {
            get
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    return _isFileAlterPreservation;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    _isFileAlterPreservation = value;
                }
                else
                {
                    _isFileAlterPreservation = false;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    return _isReadOnly;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    _isReadOnly = value;
                }
                else
                {
                    _isReadOnly = false;
                }
            }
        }

        public bool IsCompressed
        {
            get
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    return _isCompressed;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    _isCompressed = value;
                }
                else
                {
                    _isCompressed = false;
                }
            }
        }

        public byte? EncryptionMethod
        {
            get
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    return _encryptionMethod;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                // TODO
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    _encryptionMethod = value;
                }
                else
                {
                    _encryptionMethod = null;
                }
            }
        }

        public byte? GroupingIdentity
        {
            get
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    return _groupingIdentity;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    _groupingIdentity = value;
                }
                else
                {
                    _groupingIdentity = null;
                }
            }
        }

        public int DecompressedSize
        {
            get 
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    return _decompressedSize;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (_tagVersion != ID3v2TagVersion.ID3v22)
                {
                    _decompressedSize = value;
                }
                else
                {
                    _decompressedSize = 0;
                }
            }
        }

        public bool UsesUnsynchronization
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

        #endregion

        public void Read(TagReadingInfo tagReadingInfo, ref Stream stream)
        {
            // TODO: Some tags have the length INCLUDE the extra ten bytes of the tag header.  
            // Handle this (don't corrupt MP3 on rewrite)

            _tagVersion = tagReadingInfo.TagVersion;

            bool usesUnsynchronization = ((tagReadingInfo.TagVersionOptions & TagVersionOptions.Unsynchronized) == TagVersionOptions.Unsynchronized);

            if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v23)
            {
                if (!usesUnsynchronization)
                    _frameSize = stream.ReadInt32();
                else
                    _frameSize = ID3v2Utils.ReadInt32Unsynchronized(stream);

                _frameSizeExcludingAdditions = _frameSize;

                byte byte0 = stream.Read1();
                byte byte1 = stream.Read1();

                // First byte
                IsTagAlterPreservation = ((byte0 & 0x80) == 0x80);
                IsFileAlterPreservation = ((byte0 & 0x40) == 0x40);
                IsReadOnly = ((byte0 & 0x20) == 0x20);

                // Second byte
                IsCompressed = ((byte1 & 0x80) == 0x80);
                bool tmpIsEncrypted = ((byte1 & 0x40) == 0x40);
                bool tmpIsGroupingIdentity = ((byte1 & 0x20) == 0x20);

                // Additional bytes

                // Compression
                if (IsCompressed)
                {
                    DecompressedSize = stream.ReadInt32();
                    _frameSizeExcludingAdditions -= 4;
                }
                else
                {
                    DecompressedSize = 0;
                }

                // Encryption
                if (tmpIsEncrypted)
                {
                    EncryptionMethod = stream.Read1();
                    _frameSizeExcludingAdditions -= 1;
                }
                else
                {
                    EncryptionMethod = null;
                }

                // Grouping Identity
                if (tmpIsGroupingIdentity)
                {
                    GroupingIdentity = stream.Read1();
                    _frameSizeExcludingAdditions -= 1;
                }
                else
                {
                    GroupingIdentity = null;
                }

                if (usesUnsynchronization)
                {
                    stream = ID3v2Utils.ReadUnsynchronizedStream(stream, _frameSize);
                }
            }
            else if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v22)
            {
                if (!usesUnsynchronization)
                    _frameSize = stream.ReadInt24();
                else
                    _frameSize = ID3v2Utils.ReadInt24Unsynchronized(stream);

                if ((tagReadingInfo.TagVersionOptions & TagVersionOptions.AddOneByteToSize) == TagVersionOptions.AddOneByteToSize)
                {
                    _frameSize++;
                }
                _frameSizeExcludingAdditions = _frameSize;

                // These fields are not supported in ID3v2.2
                IsTagAlterPreservation = false;
                IsFileAlterPreservation = false;
                IsReadOnly = false;
                IsCompressed = false;
                DecompressedSize = 0;
                EncryptionMethod = null;
                GroupingIdentity = null;

                if (usesUnsynchronization)
                {
                    stream = ID3v2Utils.ReadUnsynchronizedStream(stream, _frameSize);
                }
            }
            else if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v24)
            {
                if ((tagReadingInfo.TagVersionOptions & TagVersionOptions.UseNonSyncSafeFrameSizeID3v24) == TagVersionOptions.UseNonSyncSafeFrameSizeID3v24)
                    _frameSize = stream.ReadInt32();
                else
                    _frameSize = ID3v2Utils.ReadInt32SyncSafe(stream);
                
                _frameSizeExcludingAdditions = _frameSize;

                byte byte0 = stream.Read1();
                byte byte1 = stream.Read1();

                bool hasDataLengthIndicator = ((byte1 & 0x01) == 0x01);
                usesUnsynchronization = ((byte1 & 0x03) == 0x03);
                if (hasDataLengthIndicator)
                {
                    _frameSizeExcludingAdditions -= 4;
                    stream.Seek(4, SeekOrigin.Current); // skip data length indicator
                }

                if (usesUnsynchronization)
                {
                    stream = ID3v2Utils.ReadUnsynchronizedStream(stream, _frameSize);
                }

                // TODO - finish parsing
            }

            if (IsCompressed)
            {
                stream = ID3v2Utils.DecompressFrame(stream, FrameSizeExcludingAdditions);
                IsCompressed = false;
                DecompressedSize = 0;
                _frameSizeExcludingAdditions = (int)stream.Length;
            }
        }

        public byte[] GetBytes(MemoryStream frameData, ID3v2TagVersion tagVersion, string frameID)
        {
            _frameSizeExcludingAdditions = (int)frameData.Length;

            if (frameID == null)
                return new byte[0];

            byte[] frameIDBytes = ByteUtils.ISO88591GetBytes(frameID);
            byte[] tmpRawData;

            if (tagVersion == ID3v2TagVersion.ID3v22)
            {
                if (frameIDBytes.Length != 3)
                    throw new ArgumentException(String.Format("FrameID must be 3 bytes from ID3v2.2 ({0} bytes passed)", frameIDBytes.Length));

                tmpRawData = new byte[6];
                tmpRawData[0] = frameIDBytes[0];
                tmpRawData[1] = frameIDBytes[1];
                tmpRawData[2] = frameIDBytes[2];
                tmpRawData[3] = (byte)((_frameSizeExcludingAdditions >> 16) & 0xFF);
                tmpRawData[4] = (byte)((_frameSizeExcludingAdditions >> 8) & 0xFF);
                tmpRawData[5] = (byte)(_frameSizeExcludingAdditions & 0xFF);
            }
            else if (tagVersion == ID3v2TagVersion.ID3v23)
            {
                int tmpRawDataSize = 10;

                byte tmpByte1 = (byte)((_isTagAlterPreservation ? 0x80 : 0) +
                                   (_isFileAlterPreservation ? 0x40 : 0) +
                                   (_isReadOnly ? 0x20 : 0));

                byte tmpByte2 = (byte)((_isCompressed ? 0x80 : 0) +
                                   (_encryptionMethod != null ? 0x40 : 0) +
                                   (_groupingIdentity != null ? 0x20 : 0));

                if (_isCompressed) tmpRawDataSize += 4;
                if (_encryptionMethod != null) tmpRawDataSize++;
                if (_groupingIdentity != null) tmpRawDataSize++;

                int tmpFrameSize = _frameSizeExcludingAdditions + (tmpRawDataSize - 10);

                tmpRawData = new byte[tmpRawDataSize];

                if (frameIDBytes.Length != 4)
                    throw new ArgumentException(string.Format("FrameID must be 4 bytes ({0} bytes passed)", frameIDBytes.Length));

                tmpRawData[0] = frameIDBytes[0];
                tmpRawData[1] = frameIDBytes[1];
                tmpRawData[2] = frameIDBytes[2];
                tmpRawData[3] = frameIDBytes[3];
                tmpRawData[4] = (byte)((tmpFrameSize >> 24) & 0xFF);
                tmpRawData[5] = (byte)((tmpFrameSize >> 16) & 0xFF);
                tmpRawData[6] = (byte)((tmpFrameSize >> 8) & 0xFF);
                tmpRawData[7] = (byte)(tmpFrameSize & 0xFF);
                tmpRawData[8] = tmpByte1;
                tmpRawData[9] = tmpByte2;

                int tmpCurrentPosition = 10;

                if (_isCompressed)
                {
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 24);
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 16);
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 8);
                    tmpRawData[tmpCurrentPosition++] = (byte)DecompressedSize;
                }
                if (_encryptionMethod != null) tmpRawData[tmpCurrentPosition++] = _encryptionMethod.Value;
                if (_groupingIdentity != null) tmpRawData[tmpCurrentPosition] = _groupingIdentity.Value;
            }
            else if (tagVersion == ID3v2TagVersion.ID3v24)
            {
                int tmpRawDataSize = 10;

                byte tmpByte1 = (byte)((_isTagAlterPreservation ? 0x40 : 0) +
                                  (_isFileAlterPreservation ? 0x20 : 0) +
                                  (_isReadOnly ? 0x10 : 0));

                byte tmpByte2 = (byte)((_groupingIdentity != null ? 0x40 : 0) +
                                  (_isCompressed ? 0x08 : 0) +
                                  (_encryptionMethod != null ? 0x04 : 0)/* +
                                  (false Unsynchronization ? 0x02 : 0) +
                                  (false Data length indicator ? 0x01 : 0)*/
                                                                            );

                if (_isCompressed) tmpRawDataSize += 4;
                if (_encryptionMethod != null) tmpRawDataSize++;
                if (_groupingIdentity != null) tmpRawDataSize++;
                /*TODO: unsync,DLI*/

                int tmpFrameSize = _frameSizeExcludingAdditions + (tmpRawDataSize - 10);

                tmpRawData = new byte[tmpRawDataSize];

                if (frameIDBytes.Length != 4)
                    throw new ArgumentException(string.Format("FrameID must be 4 bytes ({0} bytes passed)", frameIDBytes.Length));

                // Note: ID3v2.4 uses sync safe frame sizes

                tmpRawData[0] = frameIDBytes[0];
                tmpRawData[1] = frameIDBytes[1];
                tmpRawData[2] = frameIDBytes[2];
                tmpRawData[3] = frameIDBytes[3];
                tmpRawData[4] = (byte)((tmpFrameSize >> 21) & 0x7F);
                tmpRawData[5] = (byte)((tmpFrameSize >> 14) & 0x7F);
                tmpRawData[6] = (byte)((tmpFrameSize >> 7) & 0x7F);
                tmpRawData[7] = (byte)(tmpFrameSize & 0x7F);
                tmpRawData[8] = tmpByte1;
                tmpRawData[9] = tmpByte2;

                int tmpCurrentPosition = 10;

                if (_groupingIdentity != null) tmpRawData[tmpCurrentPosition++] = _groupingIdentity.Value;
                if (_isCompressed)
                {
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 24);
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 16);
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 8);
                    tmpRawData[tmpCurrentPosition++] = (byte)DecompressedSize;
                }
                if (_encryptionMethod != null) tmpRawData[tmpCurrentPosition++] = _encryptionMethod.Value;
                /*TODO: unsync,DLI*/
            }
            else
            {
                throw new ArgumentOutOfRangeException("tagVersion", tagVersion, "Unknown tag version");
            }

            using (MemoryStream totalFrame = new MemoryStream())
            {
                totalFrame.Write(tmpRawData);
                totalFrame.Write(frameData.ToArray());
                return totalFrame.ToArray();
            }
        }
    }
}

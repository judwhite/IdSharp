using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2
{
    internal sealed class FrameHeader : IFrameHeader
    {
        private int m_FrameSize;
        private int m_FrameSizeExcludingAdditions;
        private bool m_IsTagAlterPreservation;
        private bool m_IsFileAlterPreservation;
        private bool m_IsReadOnly;
        private bool m_IsCompressed;
        private byte? m_EncryptionMethod;
        private byte? m_GroupingIdentity;
        private int m_DecompressedSize;
        private ID3v2TagVersion m_TagVersion;

        public ID3v2TagVersion TagVersion
        {
            get { return m_TagVersion; }
        }

        #region IFrameHeader Members

        public int FrameSize
        {
            get { return m_FrameSize; }
        }

        public int FrameSizeTotal
        {
            get { return m_FrameSize + (m_TagVersion == ID3v2TagVersion.ID3v22 ? 6 : 10); }
        }

        public int FrameSizeExcludingAdditions
        {
            get { return m_FrameSizeExcludingAdditions; }
        }

        public bool IsTagAlterPreservation
        {
            get
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return m_IsTagAlterPreservation;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    m_IsTagAlterPreservation = value;
                }
                else
                {
                    m_IsTagAlterPreservation = false;
                }
            }
        }

        public bool IsFileAlterPreservation
        {
            get
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return m_IsFileAlterPreservation;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    m_IsFileAlterPreservation = value;
                }
                else
                {
                    m_IsFileAlterPreservation = false;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return m_IsReadOnly;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    m_IsReadOnly = value;
                }
                else
                {
                    m_IsReadOnly = false;
                }
            }
        }

        public bool IsCompressed
        {
            get
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return m_IsCompressed;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    m_IsCompressed = value;
                }
                else
                {
                    m_IsCompressed = false;
                }
            }
        }

        public Byte? EncryptionMethod
        {
            get
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return m_EncryptionMethod;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                // TODO
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    m_EncryptionMethod = value;
                }
                else
                {
                    m_EncryptionMethod = null;
                }
            }
        }

        public Byte? GroupingIdentity
        {
            get
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return m_GroupingIdentity;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    m_GroupingIdentity = value;
                }
                else
                {
                    m_GroupingIdentity = null;
                }
            }
        }

        public int DecompressedSize
        {
            get 
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return m_DecompressedSize;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    m_DecompressedSize = value;
                }
                else
                {
                    m_DecompressedSize = 0;
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

            m_TagVersion = tagReadingInfo.TagVersion;

            bool usesUnsynchronization = ((tagReadingInfo.TagVersionOptions & TagVersionOptions.Unsynchronized) == TagVersionOptions.Unsynchronized);

            if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v23)
            {
                if (!usesUnsynchronization)
                    m_FrameSize = stream.ReadInt32();
                else
                    m_FrameSize = ID3v2Utils.ReadInt32Unsynchronized(stream);

                m_FrameSizeExcludingAdditions = m_FrameSize;

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
                    m_FrameSizeExcludingAdditions -= 4;
                }
                else
                {
                    DecompressedSize = 0;
                }

                // Encryption
                if (tmpIsEncrypted)
                {
                    EncryptionMethod = stream.Read1();
                    m_FrameSizeExcludingAdditions -= 1;
                }
                else
                {
                    EncryptionMethod = null;
                }

                // Grouping Identity
                if (tmpIsGroupingIdentity)
                {
                    GroupingIdentity = stream.Read1();
                    m_FrameSizeExcludingAdditions -= 1;
                }
                else
                {
                    GroupingIdentity = null;
                }

                if (usesUnsynchronization)
                {
                    stream = ID3v2Utils.ReadUnsynchronizedStream(stream, m_FrameSize);
                }
            }
            else if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v22)
            {
                if (!usesUnsynchronization)
                    m_FrameSize = stream.ReadInt24();
                else
                    m_FrameSize = ID3v2Utils.ReadInt24Unsynchronized(stream);

                if ((tagReadingInfo.TagVersionOptions & TagVersionOptions.AddOneByteToSize) == TagVersionOptions.AddOneByteToSize)
                {
                    m_FrameSize++;
                }
                m_FrameSizeExcludingAdditions = m_FrameSize;

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
                    stream = ID3v2Utils.ReadUnsynchronizedStream(stream, m_FrameSize);
                }
            }
            else if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v24)
            {
                if ((tagReadingInfo.TagVersionOptions & TagVersionOptions.UseNonSyncSafeFrameSizeID3v24) == TagVersionOptions.UseNonSyncSafeFrameSizeID3v24)
                    m_FrameSize = stream.ReadInt32();
                else
                    m_FrameSize = ID3v2Utils.ReadInt32SyncSafe(stream);
                
                m_FrameSizeExcludingAdditions = m_FrameSize;

                byte byte0 = stream.Read1();
                byte byte1 = stream.Read1();

                bool hasDataLengthIndicator = ((byte1 & 0x01) == 0x01);
                usesUnsynchronization = ((byte1 & 0x03) == 0x03);
                if (hasDataLengthIndicator)
                {
                    m_FrameSizeExcludingAdditions -= 4;
                    stream.Seek(4, SeekOrigin.Current); // skip data length indicator
                }

                if (usesUnsynchronization)
                {
                    stream = ID3v2Utils.ReadUnsynchronizedStream(stream, m_FrameSize);
                }

                // TODO - finish parsing
            }

            if (IsCompressed)
            {
                stream = ID3v2Utils.DecompressFrame(stream, FrameSizeExcludingAdditions);
                IsCompressed = false;
                DecompressedSize = 0;
                m_FrameSizeExcludingAdditions = (int)stream.Length;
            }
        }

        public Byte[] GetBytes(MemoryStream frameData, ID3v2TagVersion tagVersion, String frameID)
        {
            m_FrameSizeExcludingAdditions = (int)frameData.Length;

            if (frameID == null)
                return new Byte[0];

            Byte[] frameIDBytes = ByteUtils.ISO88591GetBytes(frameID);
            Byte[] tmpRawData;

            if (tagVersion == ID3v2TagVersion.ID3v22)
            {
                if (frameIDBytes.Length != 3)
                    throw new ArgumentException(String.Format("FrameID must be 3 bytes from ID3v2.2 ({0} bytes passed)", frameIDBytes.Length));

                tmpRawData = new Byte[6];
                tmpRawData[0] = frameIDBytes[0];
                tmpRawData[1] = frameIDBytes[1];
                tmpRawData[2] = frameIDBytes[2];
                tmpRawData[3] = (Byte)((m_FrameSizeExcludingAdditions >> 16) & 0xFF);
                tmpRawData[4] = (Byte)((m_FrameSizeExcludingAdditions >> 8) & 0xFF);
                tmpRawData[5] = (Byte)(m_FrameSizeExcludingAdditions & 0xFF);
            }
            else if (tagVersion == ID3v2TagVersion.ID3v23)
            {
                int tmpRawDataSize = 10;

                byte tmpByte1 = (byte)((m_IsTagAlterPreservation ? 0x80 : 0) +
                                   (m_IsFileAlterPreservation ? 0x40 : 0) +
                                   (m_IsReadOnly ? 0x20 : 0));

                byte tmpByte2 = (byte)((m_IsCompressed ? 0x80 : 0) +
                                   (m_EncryptionMethod != null ? 0x40 : 0) +
                                   (m_GroupingIdentity != null ? 0x20 : 0));

                if (m_IsCompressed) tmpRawDataSize += 4;
                if (m_EncryptionMethod != null) tmpRawDataSize++;
                if (m_GroupingIdentity != null) tmpRawDataSize++;

                int tmpFrameSize = m_FrameSizeExcludingAdditions + (tmpRawDataSize - 10);

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

                if (m_IsCompressed)
                {
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 24);
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 16);
                    tmpRawData[tmpCurrentPosition++] = (byte)(DecompressedSize >> 8);
                    tmpRawData[tmpCurrentPosition++] = (byte)DecompressedSize;
                }
                if (m_EncryptionMethod != null) tmpRawData[tmpCurrentPosition++] = m_EncryptionMethod.Value;
                if (m_GroupingIdentity != null) tmpRawData[tmpCurrentPosition] = m_GroupingIdentity.Value;
            }
            else if (tagVersion == ID3v2TagVersion.ID3v24)
            {
                int tmpRawDataSize = 10;

                byte tmpByte1 = (byte)((m_IsTagAlterPreservation ? 0x40 : 0) +
                                  (m_IsFileAlterPreservation ? 0x20 : 0) +
                                  (m_IsReadOnly ? 0x10 : 0));

                byte tmpByte2 = (byte)((m_GroupingIdentity != null ? 0x40 : 0) +
                                  (m_IsCompressed ? 0x08 : 0) +
                                  (m_EncryptionMethod != null ? 0x04 : 0)/* +
                                  (false Unsynchronization ? 0x02 : 0) +
                                  (false Data length indicator ? 0x01 : 0)*/
                                                                            );

                if (m_IsCompressed) tmpRawDataSize += 4;
                if (m_EncryptionMethod != null) tmpRawDataSize++;
                if (m_GroupingIdentity != null) tmpRawDataSize++;
                /*TODO: unsync,DLI*/

                int tmpFrameSize = m_FrameSizeExcludingAdditions + (tmpRawDataSize - 10);

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

                if (m_GroupingIdentity != null) tmpRawData[tmpCurrentPosition++] = m_GroupingIdentity.Value;
                if (m_IsCompressed)
                {
                    tmpRawData[tmpCurrentPosition++] = (Byte)(DecompressedSize >> 24);
                    tmpRawData[tmpCurrentPosition++] = (Byte)(DecompressedSize >> 16);
                    tmpRawData[tmpCurrentPosition++] = (Byte)(DecompressedSize >> 8);
                    tmpRawData[tmpCurrentPosition++] = (Byte)DecompressedSize;
                }
                if (m_EncryptionMethod != null) tmpRawData[tmpCurrentPosition++] = m_EncryptionMethod.Value;
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

using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class AudioEncryption : Frame, IAudioEncryption
    {
        private string _ownerIdentifier;
        private short _previewStart;
        private short _previewLength;
        private byte[] _encryptionInfo;

        public string OwnerIdentifier
        {
            get { return _ownerIdentifier; }
            set
            {
                _ownerIdentifier = value;
                RaisePropertyChanged("OwnerIdentifier");
            }
        }

        public short PreviewStart
        {
            get { return _previewStart; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");

                _previewStart = value;
                RaisePropertyChanged("PreviewStart");
            }
        }

        public short PreviewLength
        {
            get { return _previewLength; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");

                _previewLength = value;
                RaisePropertyChanged("PreviewLength");
            }
        }

        public byte[] EncryptionInfo
        {
            get { return ByteUtils.Clone(_encryptionInfo); }
            set
            {
                _encryptionInfo = ByteUtils.Clone(value);
                RaisePropertyChanged("EncryptionInfo");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "AENC";
                case ID3v2TagVersion.ID3v22:
                    return "CRA";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;

            if (bytesLeft > 0)
            {
                OwnerIdentifier = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);
                if (bytesLeft >= 4)
                {
                    PreviewStart = stream.ReadInt16(ref bytesLeft);
                    PreviewLength = stream.ReadInt16(ref bytesLeft);
                    if (bytesLeft > 0)
                    {
                        EncryptionInfo = stream.Read(bytesLeft);
                        bytesLeft = 0;
                    }
                    else
                    {
                        // Incomplete frame
                        EncryptionInfo = null;
                    }
                }
                else
                {
                    // Incomplete frame
                    PreviewStart = 0;
                    PreviewLength = 0;
                    EncryptionInfo = null;
                }
            }
            else
            {
                // Incomplete frame
                OwnerIdentifier = null;
                PreviewStart = 0;
                PreviewLength = 0;
                EncryptionInfo = null;
            }

            // Seek to end of frame
            if (bytesLeft != 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(ID3v2Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, OwnerIdentifier, true));
                stream.Write(ByteUtils.Get2Bytes(PreviewStart));
                stream.Write(ByteUtils.Get2Bytes(PreviewLength));
                if (_encryptionInfo != null) 
                    stream.Write(_encryptionInfo);
                return _frameHeader.GetBytes(stream, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

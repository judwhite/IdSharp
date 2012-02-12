using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class EncryptionMethod : Frame, IEncryptionMethod
    {
        private string _ownerIdentifier;
        private byte _methodSymbol;
        private byte[] _encryptionData;

        public string OwnerIdentifier
        {
            get { return _ownerIdentifier; }
            set
            {
                _ownerIdentifier = value;
                RaisePropertyChanged("OwnerIdentifier");
            }
        }

        public byte MethodSymbol
        {
            get { return _methodSymbol; }
            set
            {
                _methodSymbol = value;
                RaisePropertyChanged("MethodSymbol");
            }
        }

        public byte[] EncryptionData
        {
            get { return ByteUtils.Clone(_encryptionData); }
            set
            {
                _encryptionData = ByteUtils.Clone(value);
                RaisePropertyChanged("EncryptionData");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "ENCR";
                case ID3v2TagVersion.ID3v22:
                    return null;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            Reset();

            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;

            if (bytesLeft > 0)
            {
                OwnerIdentifier = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);
                if (bytesLeft > 0)
                {
                    MethodSymbol = stream.Read1(ref bytesLeft);
                    if (bytesLeft > 0)
                    {
                        EncryptionData = stream.Read(bytesLeft);
                        bytesLeft = 0;
                    }
                }
            }

            if (bytesLeft != 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.Write(ByteUtils.ISO88591GetBytes(_ownerIdentifier));
                frameData.WriteByte(0); // terminate
                frameData.WriteByte(_methodSymbol);
                if (_encryptionData != null)
                {
                    frameData.Write(_encryptionData);
                }

                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }

        private void Reset()
        {
            OwnerIdentifier = null;
            MethodSymbol = 0;
            EncryptionData = null;
        }
    }
}

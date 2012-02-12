using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class PrivateFrame : Frame, IPrivateFrame
    {
        private string _ownerIdentifier;
        private byte[] _privateData;

        public string OwnerIdentifier
        {
            get { return _ownerIdentifier; }
            set
            {
                _ownerIdentifier = value;
                RaisePropertyChanged("OwnerIdentifier");
            }
        }

        public byte[] PrivateData
        {
            get { return ByteUtils.Clone(_privateData); }
            set
            {
                _privateData = ByteUtils.Clone(value);
                RaisePropertyChanged("PrivateData");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "PRIV";
                case ID3v2TagVersion.ID3v22:
                    return null;
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
                if (bytesLeft > 0)
                {
                    PrivateData = stream.Read(bytesLeft);
                }
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (_privateData == null || _privateData.Length == 0)
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.Write(ByteUtils.ISO88591GetBytes(OwnerIdentifier));
                frameData.WriteByte(0); // terminating byte
                frameData.Write(_privateData);
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

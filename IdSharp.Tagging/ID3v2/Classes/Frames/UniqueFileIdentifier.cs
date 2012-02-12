using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class UniqueFileIdentifier : Frame, IUniqueFileIdentifier
    {
        private string _ownerIdentifier;
        private byte[] _identifier;

        public string OwnerIdentifier
        {
            get { return _ownerIdentifier; }
            set
            {
                _ownerIdentifier = value;
                RaisePropertyChanged("OwnerIdentifier");
            }
        }

        public byte[] Identifier
        {
            get { return ByteUtils.Clone(_identifier); }
            set
            {
                _identifier = ByteUtils.Clone(value);
                RaisePropertyChanged("Identifier");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "UFID";
                case ID3v2TagVersion.ID3v22:
                    return "UFI";
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
                    Identifier = stream.Read(bytesLeft);
                }
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (_identifier == null || _identifier.Length == 0)
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.Write(ByteUtils.ISO88591GetBytes(_ownerIdentifier));
                frameData.WriteByte(0); // terminator
                frameData.Write(_identifier);

                return _frameHeader.GetBytes(frameData, tagVersion, this.GetFrameID(tagVersion));
            }
        }
    }
}

using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class Signature : Frame, ISignature
    {
        private byte _groupSymbol;
        private byte[] _signatureData;

        public byte GroupSymbol
        {
            get { return _groupSymbol; }
            set
            {
                _groupSymbol = value;
                RaisePropertyChanged("GroupSymbol");
            }
        }

        public byte[] SignatureData
        {
            get { return ByteUtils.Clone(_signatureData); }
            set
            {
                _signatureData = ByteUtils.Clone(value);
                RaisePropertyChanged("SignatureData");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "SIGN";
                case ID3v2TagVersion.ID3v22:
                    return null;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            throw new NotImplementedException();
        }
    }
}

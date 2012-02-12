using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class Podcast : Frame, IPodcast
    {
        private bool _value;

        public bool Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "PCST";
                case ID3v2TagVersion.ID3v22:
                    return "PCS";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;

            while (bytesLeft > 0)
            {
                stream.Read1(ref bytesLeft);
            }
            Value = true;
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (!Value)
            {
                return new byte[0];
            }

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.Write(ByteUtils.Get4Bytes(1));
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

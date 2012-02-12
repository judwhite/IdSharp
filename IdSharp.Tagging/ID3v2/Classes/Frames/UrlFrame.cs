using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class UrlFrame : Frame, IUrlFrame
    {
        private readonly string _id3v24FrameID;
        private readonly string _id3v23FrameID;
        private readonly string _id3v22FrameID;
        private string _value;

        public UrlFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID)
        {
            _id3v24FrameID = id3v24FrameID;
            _id3v23FrameID = id3v23FrameID;
            _id3v22FrameID = id3v22FrameID;
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                    return _id3v24FrameID;
                case ID3v2TagVersion.ID3v23:
                    return _id3v23FrameID;
                case ID3v2TagVersion.ID3v22:
                    return _id3v22FrameID;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);
            Value = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, _frameHeader.FrameSizeExcludingAdditions);
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (string.IsNullOrEmpty(_value))
                return new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(ByteUtils.ISO88591GetBytes(_value));
                return _frameHeader.GetBytes(stream, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

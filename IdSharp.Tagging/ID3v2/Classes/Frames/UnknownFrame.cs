using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class UnknownFrame : Frame
    {
        private byte[] _frameData;
        private readonly string _frameID;

        public UnknownFrame(string frameID, TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameID = frameID;
            Read(tagReadingInfo, stream);
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return (_frameID.Length != 4 ? null : _frameID);
                case ID3v2TagVersion.ID3v22:
                    return (_frameID.Length != 3 ? null : _frameID);
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);
            _frameData = stream.Read(_frameHeader.FrameSizeExcludingAdditions);
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (_frameData == null || _frameData.Length == 0)
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream(_frameData))
            {
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

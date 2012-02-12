using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class PlayCount : Frame, IPlayCount
    {
        private long? m_Value;

        public long? Value
        {
            get { return m_Value; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "'value' cannot be less than 0");
                }

                m_Value = value;
                RaisePropertyChanged("Value");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "PCNT";
                case ID3v2TagVersion.ID3v22:
                    return "CNT";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;

            long playCount = 0;
            while (bytesLeft > 0)
            {
                playCount <<= 8;
                playCount += stream.Read1(ref bytesLeft);
            }

            Value = playCount;
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (m_Value == null)
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream())
            {
                if (Value <= uint.MaxValue)
                    frameData.Write(ByteUtils.Get4Bytes((uint)Value.Value));
                else
                    frameData.Write(ByteUtils.GetBytesMinimal(Value.Value));

                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

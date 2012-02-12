using System;
using System.IO;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class RecommendedBufferSize : Frame, IRecommendedBufferSize
    {
        private int _bufferSize;
        private bool _embeddedInfo;
        private int? _offsetToNextTag;

        public int BufferSize
        {
            get { return _bufferSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                _bufferSize = value;
                RaisePropertyChanged("BufferSize");
            }
        }

        public bool EmbeddedInfo
        {
            get { return _embeddedInfo; }
            set
            {
                _embeddedInfo = value;
                RaisePropertyChanged("EmbeddedInfo");
            }
        }

        public int? OffsetToNextTag
        {
            get { return _offsetToNextTag; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                _offsetToNextTag = value;
                RaisePropertyChanged("OffsetToNextTag");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "RBUF";
                case ID3v2TagVersion.ID3v22:
                    return "BUF";
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
            if (BufferSize == 0)
                return new byte[0];

            throw new NotImplementedException();
        }
    }
}

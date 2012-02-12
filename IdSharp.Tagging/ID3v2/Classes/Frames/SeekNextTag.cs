using System;
using System.IO;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class SeekNextTag : Frame, ISeekNextTag
    {
        private int _minimumOffsetToNextTag;

        public int MinimumOffsetToNextTag
        {
            get { return _minimumOffsetToNextTag; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                _minimumOffsetToNextTag = value;
                RaisePropertyChanged("MinimumOffsetToNextTag");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "SEEK";
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
            if (_minimumOffsetToNextTag == 0)
                return new byte[0];

            throw new NotImplementedException();
        }
    }
}

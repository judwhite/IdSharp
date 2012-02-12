using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class MusicCDIdentifier : Frame, IMusicCDIdentifier
    {
        private byte[] _toc;

        public byte[] TOC
        {
            get { return ByteUtils.Clone(_toc); }
            set
            {
                _toc = ByteUtils.Clone(value);
                RaisePropertyChanged("TOC");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "MCDI";
                case ID3v2TagVersion.ID3v22:
                    return "MCI";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);
            if (_frameHeader.FrameSizeExcludingAdditions > 0)
                TOC = stream.Read(_frameHeader.FrameSizeExcludingAdditions);
            else
                TOC = null;
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (TOC == null || TOC.Length == 0)
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.Write(TOC, 0, TOC.Length);
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

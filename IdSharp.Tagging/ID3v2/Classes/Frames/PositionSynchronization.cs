using System;
using System.IO;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class PositionSynchronization : Frame, IPositionSynchronization
    {
        private TimestampFormat m_TimestampFormat;
        private int m_Position;

        public PositionSynchronization()
        {
            m_TimestampFormat = TimestampFormat.Milliseconds;
        }

        public TimestampFormat TimestampFormat
        {
            get { return m_TimestampFormat; }
            set
            {
                m_TimestampFormat = value;
                RaisePropertyChanged("TimestampFormat");
            }
        }

        public int Position
        {
            get { return m_Position; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                m_Position = value;
                RaisePropertyChanged("Position");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "POSS";
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
            if (Position == 0)
                return new byte[0];

            throw new NotImplementedException();
        }
    }
}

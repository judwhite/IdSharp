using System;
using System.IO;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class Reverb : Frame, IReverb
    {
        private short _reverbLeftMilliseconds;
        private short _reverbRightMilliseconds;
        private byte _reverbBouncesLeft;
        private byte _reverbBouncesRight;
        private byte _reverbFeedbackLeftToLeft;
        private byte _reverbFeedbackLeftToRight;
        private byte _reverbFeedbackRightToRight;
        private byte _reverbFeedbackRightToLeft;
        private byte _premixLeftToRight;
        private byte _premixRightToLeft;

        public short ReverbLeftMilliseconds
        {
            get { return _reverbLeftMilliseconds; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                _reverbLeftMilliseconds = value;
                RaisePropertyChanged("ReverbLeftMilliseconds");
            }
        }

        public short ReverbRightMilliseconds
        {
            get { return _reverbRightMilliseconds; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                _reverbRightMilliseconds = value;
                RaisePropertyChanged("ReverbRightMilliseconds");
            }
        }

        public byte ReverbBouncesLeft
        {
            get { return _reverbBouncesLeft; }
            set
            {
                _reverbBouncesLeft = value;
                RaisePropertyChanged("ReverbBouncesLeft");
            }
        }

        public byte ReverbBouncesRight
        {
            get { return _reverbBouncesRight; }
            set
            {
                _reverbBouncesRight = value;
                RaisePropertyChanged("ReverbBouncesRight");
            }
        }

        public byte ReverbFeedbackLeftToLeft
        {
            get { return _reverbFeedbackLeftToLeft; }
            set
            {
                _reverbFeedbackLeftToLeft = value;
                RaisePropertyChanged("ReverbFeedbackLeftToLeft");
            }
        }

        public byte ReverbFeedbackLeftToRight
        {
            get { return _reverbFeedbackLeftToRight; }
            set
            {
                _reverbFeedbackLeftToRight = value;
                RaisePropertyChanged("ReverbFeedbackLeftToRight");
            }
        }

        public byte ReverbFeedbackRightToRight
        {
            get { return _reverbFeedbackRightToRight; }
            set
            {
                _reverbFeedbackRightToRight = value;
                RaisePropertyChanged("ReverbFeedbackRightToRight");
            }
        }

        public byte ReverbFeedbackRightToLeft
        {
            get { return _reverbFeedbackRightToLeft; }
            set
            {
                _reverbFeedbackRightToLeft = value;
                RaisePropertyChanged("ReverbFeedbackRightToLeft");
            }
        }

        public byte PremixLeftToRight
        {
            get { return _premixLeftToRight; }
            set
            {
                _premixLeftToRight = value;
                RaisePropertyChanged("PremixLeftToRight");
            }
        }

        public byte PremixRightToLeft
        {
            get { return _premixRightToLeft; }
            set
            {
                _premixRightToLeft = value;
                RaisePropertyChanged("PremixRightToLeft");
            }
        }
        
        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "RVRB";
                case ID3v2TagVersion.ID3v22:
                    return "REV";
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
            if (ReverbLeftMilliseconds == 0 &&
                ReverbRightMilliseconds == 0)
            {
                return new byte[0];
            }

            throw new NotImplementedException();
        }
    }
}

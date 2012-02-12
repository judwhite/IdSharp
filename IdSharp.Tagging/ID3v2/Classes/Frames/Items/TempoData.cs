using System;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class TempoData : ITempoData
    {
        private short _tempoCode;
        private int _timestamp;

        public event PropertyChangedEventHandler PropertyChanged;

        public short TempoCode
        {
            get
            {
                return _tempoCode;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }

                _tempoCode = value;
                RaisePropertyChanged("TempoCode");
            }
        }

        public int Timestamp
        {
            get
            {
                return _timestamp;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");

                _timestamp = value;
                RaisePropertyChanged("Timestamp");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

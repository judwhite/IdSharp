using System;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class EventTimingItem : IEventTimingItem
    {
        private MusicEvent _eventType;
        private int _timestamp;

        public event PropertyChangedEventHandler PropertyChanged;

        public MusicEvent EventType
        {
            get
            {
                return _eventType;
            }
            set
            {
                _eventType = value;
                RaisePropertyChanged("EventType");
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
                    throw new ArgumentOutOfRangeException("value", "Value cannot be less than 0");

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

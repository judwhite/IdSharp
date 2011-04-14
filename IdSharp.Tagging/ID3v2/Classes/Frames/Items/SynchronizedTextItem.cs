using System;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class SynchronizedTextItem : ISynchronizedTextItem
    {
        private string _text;
        private int _timestamp;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                FirePropertyChanged("Text");
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
                FirePropertyChanged("Timestamp");
            }
        }

        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

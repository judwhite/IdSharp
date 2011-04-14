using System;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class MusicianCreditsItem : IMusicianCreditsItem
    {
        private string _instrument;
        private string _artists;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Instrument
        {
            get
            {
                return _instrument;
            }
            set
            {
                _instrument = value;
                SendPropertyChanged("Instrument");
            }
        }

        public string Artists
        {
            get
            {
                return _artists;
            }
            set
            {
                _artists = value;
                SendPropertyChanged("Artists");
            }
        }

        private void SendPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

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
                RaisePropertyChanged("Instrument");
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
                RaisePropertyChanged("Artists");
            }
        }

        private void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

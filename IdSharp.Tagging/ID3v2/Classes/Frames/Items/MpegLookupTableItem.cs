using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class MpegLookupTableItem : IMpegLookupTableItem
    {
        private long _deviationInBytes;
        private long _deviationInMilliseconds;

        public event PropertyChangedEventHandler PropertyChanged;

        public long DeviationInBytes
        {
            get
            {
                return _deviationInBytes;
            }
            set
            {
                _deviationInBytes = value;
                RaisePropertyChanged("DeviationInBytes");
            }
        }

        public long DeviationInMilliseconds
        {
            get
            {
                return _deviationInMilliseconds;
            }
            set
            {
                _deviationInMilliseconds = value;
                RaisePropertyChanged("DeviationInMilliseconds");
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

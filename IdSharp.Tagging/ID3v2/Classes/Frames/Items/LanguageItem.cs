using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class LanguageItem : ILanguageItem
    {
        private string _languageCode;
        private string _languageDisplay;

        public event PropertyChangedEventHandler PropertyChanged;

        public string LanguageCode
        {
            get
            {
                return _languageCode;
            }
            set
            {
                _languageCode = value;

                string languageDisplay;
                if (LanguageHelper.Languages.TryGetValue(_languageCode.ToLower(), out languageDisplay))
                {
                    LanguageDisplay = languageDisplay;
                }
                else
                {
                    LanguageDisplay = _languageCode;
                    // TODO: notify about bad data?
                }
                SendPropertyChanged("LanguageCode");
            }
        }

        public string LanguageDisplay
        {
            get 
            {
                return _languageDisplay;
            }
            private set             
            {
                _languageDisplay = value;
                SendPropertyChanged("LanguageDisplay");
            }
        }

        private void SendPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

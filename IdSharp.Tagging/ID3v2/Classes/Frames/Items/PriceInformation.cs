using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class PriceInformation : IPriceInformation
    {
        private string _currencyCode;
        private decimal _price;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrencyCode
        {
            get
            {
                return _currencyCode;
            }
            set
            {
                _currencyCode = value;
                SendPropertyChanged("CurrencyCode");
            }
        }

        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                SendPropertyChanged("Price");
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

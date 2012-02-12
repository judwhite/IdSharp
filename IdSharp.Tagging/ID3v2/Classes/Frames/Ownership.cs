using System;
using System.IO;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class Ownership : Frame, IOwnership
    {
        private EncodingType _textEncoding;
        private decimal _pricePaid;
        private string _currencyCode;
        private DateTime _dateOfPurchase;
        private string _seller;

        public EncodingType TextEncoding
        {
            get { return _textEncoding; }
            set
            {
                _textEncoding = value;
                RaisePropertyChanged("TextEncoding");
            }
        }

        public decimal PricePaid
        {
            get { return _pricePaid; }
            set
            {
                _pricePaid = value;
                RaisePropertyChanged("PricePaid");
            }
        }

        public string CurrencyCode
        {
            get { return _currencyCode; }
            set
            {
                _currencyCode = value; // todo: validation
                RaisePropertyChanged("CurrencyCode");
            }
        }

        public DateTime DateOfPurchase
        {
            get { return _dateOfPurchase.Date; }
            set
            {
                _dateOfPurchase = value.Date;
                RaisePropertyChanged("DateOfPurchase");
            }
        }

        public string Seller
        {
            get { return _seller; }
            set
            {
                _seller = value;
                RaisePropertyChanged("Seller");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "OWNE";
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
            if (PricePaid == 0 &&
                string.IsNullOrEmpty(CurrencyCode) &&
                DateOfPurchase == DateTime.MinValue &&
                string.IsNullOrEmpty(Seller))
            {
                return new byte[0];
            }

            throw new NotImplementedException();
        }
    }
}

using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Extensions;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class Commercial : Frame, ICommercial
    {
        private EncodingType _textEncoding;
        private readonly PriceInformationBindingList _priceList;
        private DateTime _validUntil;
        private string _contactUrl;
        private string _nameOfSeller;
        private ReceivedAs _receivedAs;
        private string _description;
        private string _sellerLogoMimeType;
        private byte[] _sellerLogo;

        public Commercial()
        {
            _priceList = new PriceInformationBindingList();
        }

        public EncodingType TextEncoding
        {
            get { return _textEncoding; }
            set
            {
                _textEncoding = value;
                RaisePropertyChanged("TextEncoding");
            }
        }

        public BindingList<IPriceInformation> PriceList
        {
            get { return _priceList; }
        }

        public DateTime ValidUntil
        {
            get { return _validUntil.Date; }
            set
            {
                _validUntil = value.Date;
                RaisePropertyChanged("ValidUntil");
            }
        }

        public string ContactUrl
        {
            get { return _contactUrl; }
            set
            {
                _contactUrl = value;
                RaisePropertyChanged("ContactUrl");
            }
        }

        public ReceivedAs ReceivedAs
        {
            get { return _receivedAs; }
            set
            {
                _receivedAs = value;
                RaisePropertyChanged("ReceivedAs");
            }
        }

        public string NameOfSeller
        {
            get { return _nameOfSeller; }
            set
            {
                _nameOfSeller = value;
                RaisePropertyChanged("NameOfSeller");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string SellerLogoMimeType
        {
            get { return _sellerLogoMimeType; }
            set
            {
                _sellerLogoMimeType = value;
                RaisePropertyChanged("SellerLogoMimeType");
            }
        }

        public byte[] SellerLogo
        {
            get { return ByteUtils.Clone(_sellerLogo); }
            set
            {
                _sellerLogo = ByteUtils.Clone(value);
                RaisePropertyChanged("SellerLogo");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "COMR";
                case ID3v2TagVersion.ID3v22:
                    return null;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            Reset();

            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
            if (bytesLeft > 1)
            {
                TextEncoding = (EncodingType)stream.Read1(ref bytesLeft);
                string priceString = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);

                if (!string.IsNullOrEmpty(priceString))
                {
                    foreach (string priceItem in priceString.Split('/'))
                    {
                        if (priceItem.Length > 3)
                        {
                            decimal price;
                            string pricePart = priceItem.Substring(3, priceItem.Length - 3);
                            if (decimal.TryParse(pricePart, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out price))
                            {
                                IPriceInformation priceInfo = new PriceInformation();
                                priceInfo.CurrencyCode = priceItem.Substring(0, 3);
                                priceInfo.Price = price;
                                _priceList.Add(priceInfo);
                            }
                        }
                    }
                }

                if (bytesLeft > 0)
                {
                    string validUntil = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, 8);
                    bytesLeft -= 8;

                    if (validUntil.Length == 8)
                    {
                        validUntil = string.Format("{0}-{1}-{2}", validUntil.Substring(0, 4),
                            validUntil.Substring(4, 2), validUntil.Substring(6, 2));
                        DateTime.TryParse(validUntil, out _validUntil);
                    }

                    if (bytesLeft > 0)
                    {
                        ContactUrl = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);

                        if (bytesLeft > 0)
                        {
                            ReceivedAs = (ReceivedAs)stream.Read1(ref bytesLeft);
                            NameOfSeller = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);
                            Description = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);
                            SellerLogoMimeType = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);
                            if (bytesLeft > 0)
                            {
                                SellerLogo = stream.Read(bytesLeft);
                                bytesLeft = 0;
                            }
                        }
                    }
                }
            }

            if (bytesLeft != 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (_priceList.Count == 0)
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream())
            {
                byte[] nameOfSellerData;
                byte[] descriptionData;
                do
                {
                    nameOfSellerData = ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, _nameOfSeller, true);
                    descriptionData = ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, _description, true);
                } while (
                    this.RequiresFix(tagVersion, _nameOfSeller, nameOfSellerData) ||
                    this.RequiresFix(tagVersion, _description, descriptionData)
                );

                frameData.WriteByte((byte)_textEncoding);

                string priceString = string.Empty;
                foreach (IPriceInformation priceInfo in _priceList)
                {
                    if (priceInfo.CurrencyCode != null && priceInfo.CurrencyCode.Length == 3)
                    {
                        if (priceString != string.Empty) 
                            priceString += "/";
                        priceString += string.Format("{0}{1:0.00}", priceInfo.CurrencyCode, priceInfo.Price);
                    }
                }

                // No valid price items
                if (priceString == string.Empty)
                    return new byte[0];

                frameData.Write(ByteUtils.ISO88591GetBytes(priceString));
                frameData.WriteByte(0); // terminator
                frameData.Write(ByteUtils.ISO88591GetBytes(_validUntil.ToString("yyyyMMdd")));
                frameData.Write(ByteUtils.ISO88591GetBytes(_contactUrl));
                frameData.WriteByte(0); // terminator
                frameData.WriteByte((byte)_receivedAs);
                frameData.Write(nameOfSellerData);
                frameData.Write(descriptionData);

                // This section is optional
                if (_sellerLogo != null && _sellerLogo.Length != 0)
                {
                    frameData.Write(ByteUtils.ISO88591GetBytes(_sellerLogoMimeType));
                    frameData.WriteByte(0); // terminator
                    frameData.Write(_sellerLogo);
                }
                    
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }

        private void Reset()
        {
            TextEncoding = EncodingType.ISO88591;
            PriceList.Clear();
            ValidUntil = DateTime.MinValue;
            ContactUrl = null;
            ReceivedAs = ReceivedAs.Other;
            NameOfSeller = null;
            Description = null;
            SellerLogoMimeType = null;
            SellerLogo = null;
        }
    }
}

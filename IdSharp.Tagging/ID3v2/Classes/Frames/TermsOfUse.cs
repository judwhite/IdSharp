using System;
using System.IO;
using IdSharp.Tagging.ID3v2.Extensions;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class TermsOfUse : Frame, ITermsOfUse
    {
        private EncodingType _textEncoding;
        private string _languageCode;
        private string _value;

        public EncodingType TextEncoding
        {
            get { return _textEncoding; }
            set
            {
                _textEncoding = value;
                RaisePropertyChanged("TextEncoding");
            }
        }

        public string LanguageCode
        {
            get { return _languageCode; }
            set
            {
                _languageCode = value;
                RaisePropertyChanged("LanguageCode");
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "USER";
                case ID3v2TagVersion.ID3v22:
                    return null;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
            if (bytesLeft >= 1)
            {
                TextEncoding = (EncodingType)stream.Read1(ref bytesLeft);
                if (bytesLeft >= 3)
                {
                    LanguageCode = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    bytesLeft -= 3;
                    if (bytesLeft > 0)
                    {
                        Value = ID3v2Utils.ReadString(TextEncoding, stream, bytesLeft);
                        bytesLeft = 0;
                    }
                }
                else
                {
                    LanguageCode = "eng";
                }
            }
            else
            {
                TextEncoding = EncodingType.ISO88591;
                LanguageCode = "eng";
            }

            // Seek past any unread bytes
            if (bytesLeft > 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (string.IsNullOrEmpty(_value))
                return new byte[0];

            byte[] valueData;
            do
            {
                valueData = ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, _value, false);
            } while (this.RequiresFix(tagVersion, _value, valueData));

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.WriteByte((byte)_textEncoding);
                frameData.Write(ByteUtils.ISO88591GetBytes(_languageCode));
                frameData.Write(valueData);
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class TXXXFrame : Frame, ITXXXFrame
    {
        private EncodingType _textEncoding;
        private string _description;
        private string _value;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                SendPropertyChanged("Description");
            }
        }

        public EncodingType TextEncoding
        {
            get { return _textEncoding; }
            set
            {
                _textEncoding = value;
                SendPropertyChanged("TextEncoding");
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                SendPropertyChanged("Value");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "TXXX";
                case ID3v2TagVersion.ID3v22:
                    return "TXX";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);
            if (_frameHeader.FrameSizeExcludingAdditions > 0)
            {
                TextEncoding = (EncodingType)stream.Read1();
                int bytesLeft = _frameHeader.FrameSizeExcludingAdditions - 1;
                Description = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);
                Value = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, bytesLeft);
            }
            else
            {
                /*String msg = String.Format("0 length frame '{0}' at position {1}", "TXXX", stream.Position);
                Trace.WriteLine(msg);*/

                Description = string.Empty;
                Value = string.Empty;
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (string.IsNullOrEmpty(_value))
                return new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                stream.WriteByte((byte)TextEncoding);
                stream.Write(ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, Description, true));
                stream.Write(ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, Value, false));
                return _frameHeader.GetBytes(stream, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

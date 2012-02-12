using System;
using System.IO;
using IdSharp.Tagging.ID3v2.Extensions;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class TextFrame : Frame, ITextFrame
    {
        private EncodingType _textEncoding;        
        private readonly string _id3v24FrameID;
        private readonly string _id3v23FrameID;
        private readonly string _id3v22FrameID;
        private string _value;

        public TextFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID)
        {
            _id3v24FrameID = id3v24FrameID;
            _id3v23FrameID = id3v23FrameID;
            _id3v22FrameID = id3v22FrameID;
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

        public string Value
        {
            get { return _value; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _value = value;
                else
                    _value = value.Trim();

                RaisePropertyChanged("Value");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                    return _id3v24FrameID;
                case ID3v2TagVersion.ID3v23:
                    return _id3v23FrameID;
                case ID3v2TagVersion.ID3v22:
                    return _id3v22FrameID;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);

            // Some taggers write 0 byte sized frames (which is explicitly forbidden by the spec)
            if (_frameHeader.FrameSizeExcludingAdditions >= 1)
            {
                TextEncoding = (EncodingType)stream.Read1();
                Value = ID3v2Utils.ReadString(_textEncoding, stream, _frameHeader.FrameSizeExcludingAdditions - 1);
            }
            else
            {
                //String msg = String.Format("Under-sized ({0} bytes) text frame at position {1}", m_FrameHeader.FrameSizeExcludingAdditions, stream.Position);
                //Trace.WriteLine(msg);

                TextEncoding = EncodingType.ISO88591;
                Value = "";
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (string.IsNullOrEmpty(Value))
                return new byte[0];

            byte[] valueData;
            do
            {
                valueData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, Value, false);
            } while (this.RequiresFix(tagVersion, Value, valueData));

            using (MemoryStream stream = new MemoryStream())
            {
                stream.WriteByte((byte)_textEncoding);
                stream.Write(valueData);
                return _frameHeader.GetBytes(stream, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

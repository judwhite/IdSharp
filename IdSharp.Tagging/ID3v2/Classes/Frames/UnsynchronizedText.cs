using System;
using System.Diagnostics;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class UnsynchronizedText : Frame, IUnsynchronizedText
    {
        private EncodingType m_TextEncoding;
        private string m_LanguageCode;
        private string m_ContentDescriptor;
        private string m_Text;

        public EncodingType TextEncoding
        {
            get { return m_TextEncoding; }
            set { m_TextEncoding = value; SendPropertyChanged("TextEncoding"); }
        }

        public string LanguageCode
        {
            get { return m_LanguageCode; }
            set { m_LanguageCode = value; SendPropertyChanged("LanguageCode"); }
        }

        public string ContentDescriptor
        {
            get { return m_ContentDescriptor; }
            set { m_ContentDescriptor = value; SendPropertyChanged("ContentDescriptor"); }
        }

        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; SendPropertyChanged("Text"); }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "USLT";
                case ID3v2TagVersion.ID3v22:
                    return "ULT";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);
            if (_frameHeader.FrameSizeExcludingAdditions >= 4)
            {
                TextEncoding = (EncodingType)stream.Read1();
                LanguageCode = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, 3);

                int tmpBytesLeft = _frameHeader.FrameSizeExcludingAdditions - 1 /*encoding*/- 3 /*language code*/;
                ContentDescriptor = ID3v2Utils.ReadString(TextEncoding, stream, ref tmpBytesLeft);

                Text = ID3v2Utils.ReadString(m_TextEncoding, stream, tmpBytesLeft);
            }
            else
            {
                string msg = string.Format("Under-sized ({0} bytes) unsynchronized text frame at position {1}", _frameHeader.FrameSizeExcludingAdditions, stream.Position);
                Trace.WriteLine(msg);

                LanguageCode = "eng";
                ContentDescriptor = "";
                Text = "";
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            // TODO: Test

            if (string.IsNullOrEmpty(Text))
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.WriteByte((byte)TextEncoding);
                frameData.Write(ByteUtils.ISO88591GetBytes(LanguageCode));
                frameData.Write(ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, ContentDescriptor, true));
                frameData.Write(ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, Text, false));
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

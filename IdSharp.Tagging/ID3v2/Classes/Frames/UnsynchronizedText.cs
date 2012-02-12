using System;
using System.Diagnostics;
using System.IO;
using IdSharp.Tagging.ID3v2.Extensions;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class UnsynchronizedText : Frame, IUnsynchronizedText
    {
        private EncodingType _textEncoding;
        private string _languageCode;
        private string _contentDescriptor;
        private string _text;

        public EncodingType TextEncoding
        {
            get { return _textEncoding; }
            set { _textEncoding = value; RaisePropertyChanged("TextEncoding"); }
        }

        public string LanguageCode
        {
            get { return _languageCode; }
            set { _languageCode = value; RaisePropertyChanged("LanguageCode"); }
        }

        public string ContentDescriptor
        {
            get { return _contentDescriptor; }
            set { _contentDescriptor = value; RaisePropertyChanged("ContentDescriptor"); }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; RaisePropertyChanged("Text"); }
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

                Text = ID3v2Utils.ReadString(_textEncoding, stream, tmpBytesLeft);
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

            byte[] contentDescriptorData;
            byte[] textData;
            do
            {
                contentDescriptorData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, ContentDescriptor, true);
                textData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, Text, false);
            } while (
                this.RequiresFix(tagVersion, ContentDescriptor, contentDescriptorData) ||
                this.RequiresFix(tagVersion, Text, textData)
            );

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.WriteByte((byte)TextEncoding);
                frameData.Write(ByteUtils.ISO88591GetBytes(LanguageCode));
                frameData.Write(contentDescriptorData);
                frameData.Write(textData);
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

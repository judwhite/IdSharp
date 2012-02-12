using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Extensions;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class SynchronizedText : Frame, ISynchronizedText
    {
        private EncodingType _textEncoding;
        private string _languageCode;
        private TimestampFormat _timestampFormat;
        private TextContentType _contentType;
        private string _contentDescriptor;
        private readonly SynchronizedTextItemBindingList _synchronizedTextItemBindingList;

        public SynchronizedText()
        {
            _synchronizedTextItemBindingList = new SynchronizedTextItemBindingList();
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

        public string LanguageCode
        {
            get { return _languageCode; }
            set
            {
                // todo: lang code validation
                _languageCode = value;
                RaisePropertyChanged("LanguageCode");
            }
        }

        public TimestampFormat TimestampFormat
        {
            get { return _timestampFormat; }
            set
            {
                _timestampFormat = value;
                RaisePropertyChanged("TimestampFormat");
            }
        }

        public TextContentType ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                RaisePropertyChanged("ContentType");
            }
        }

        public string ContentDescriptor
        {
            get { return _contentDescriptor; }
            set 
            {
                _contentDescriptor = value;
                RaisePropertyChanged("ContentDescriptor");
            }
        }

        public BindingList<ISynchronizedTextItem> Items
        {
            get { return _synchronizedTextItemBindingList; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "SYLT";
                case ID3v2TagVersion.ID3v22:
                    return "SLT";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            Items.Clear();

            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
            
            if (bytesLeft >= 1)
            {
                TextEncoding = (EncodingType)stream.Read1(ref bytesLeft);
                if (bytesLeft >= 3)
                {
                    LanguageCode = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    bytesLeft -= 3;
                    if (bytesLeft >= 2)
                    {
                        TimestampFormat = (TimestampFormat)stream.Read1(ref bytesLeft);
                        ContentType = (TextContentType)stream.Read1(ref bytesLeft);
                        if (bytesLeft > 0)
                        {
                            ContentDescriptor = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);

                            while (bytesLeft > 0)
                            {
                                string lyrics = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);
                                if (bytesLeft >= 4)
                                {
                                    SynchronizedTextItem textItem = new SynchronizedTextItem();
                                    textItem.Text = lyrics;
                                    textItem.Timestamp = stream.ReadInt32();
                                    bytesLeft -= 4;
                                    Items.Add(textItem);
                                }
                            }
                        }
                        else
                        {
                            // Incomplete frame
                            ContentDescriptor = "";
                        }
                    }
                    else
                    {
                        // Incomplete frame
                        TimestampFormat = TimestampFormat.Milliseconds;
                        ContentType = TextContentType.Other;
                        ContentDescriptor = "";
                    }
                }
                else
                {
                    // Incomplete frame
                    LanguageCode = "eng";
                    TimestampFormat = TimestampFormat.Milliseconds;
                    ContentType = TextContentType.Other;
                    ContentDescriptor = "";
                }
            }
            else
            {
                // Incomplete frame
                TextEncoding = EncodingType.ISO88591;
                LanguageCode = "eng";
                TimestampFormat = TimestampFormat.Milliseconds;
                ContentType = TextContentType.Other;
                ContentDescriptor = "";
            }

            if (bytesLeft > 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (Items.Count == 0)
                return new byte[0];

            if (TextEncoding == EncodingType.ISO88591)
            {
                foreach (ISynchronizedTextItem item in Items)
                {
                    byte[] textData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, item.Text, true);
                    if (this.RequiresFix(tagVersion, item.Text, textData))
                        break;
                }
            }

            byte[] contentDescriptorData;
            do
            {
                contentDescriptorData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, ContentDescriptor, true);
            } while (this.RequiresFix(tagVersion, ContentDescriptor, contentDescriptorData));

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.WriteByte((byte)TextEncoding);
                frameData.Write(ByteUtils.ISO88591GetBytes(LanguageCode));
                frameData.WriteByte((byte)TimestampFormat);
                frameData.WriteByte((byte)ContentType);
                frameData.Write(contentDescriptorData);
                foreach (ISynchronizedTextItem item in Items)
                {
                    frameData.Write(ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, item.Text, true));
                    frameData.Write(ByteUtils.Get4Bytes(item.Timestamp));
                }

                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

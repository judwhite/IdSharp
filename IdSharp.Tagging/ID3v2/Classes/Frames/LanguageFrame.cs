using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Extensions;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class LanguageFrame : Frame, ILanguageFrame
    {
        private EncodingType _textEncoding;
        private readonly LanguageItemBindingList _languageItems;

        public LanguageFrame()
        {
            _languageItems = new LanguageItemBindingList();
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

        public BindingList<ILanguageItem> Items
        {
            get { return _languageItems; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "TLAN";
                case ID3v2TagVersion.ID3v22:
                    return "TLA";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _languageItems.Clear();

            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
            if (bytesLeft >= 4)
            {
                TextEncoding = (EncodingType)stream.Read1(ref bytesLeft);
                // This could be implemented many ways
                // engfraspa etc
                // eng 0x00 fra 0x00 spa 0x00 etc
                // English
                // English 0x00 French 0x00 Spanish 0x00

                // TODO: Finish implementation
                string languageCode = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);
                if (languageCode.Length != 3)
                {
                    if (languageCode.ToLower() == "english" || languageCode.ToLower() == "en")
                    {
                        Items.AddNew().LanguageCode = "eng";
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> kvp in LanguageHelper.Languages)
                        {
                            if (kvp.Value.ToLower() == languageCode.ToLower())
                            {
                                Items.AddNew().LanguageCode = kvp.Key;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Items.AddNew().LanguageCode = languageCode;
                }
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

            // Set TextEncoding to Unicode/UTF8 if required
            if (TextEncoding == EncodingType.ISO88591)
            {
                foreach (ILanguageItem languageItem in Items)
                {
                    byte[] languageCodeData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, languageItem.LanguageCode, true);
                    this.RequiresFix(tagVersion, languageItem.LanguageCode, languageCodeData);
                }
            }

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.WriteByte((byte)TextEncoding);
                bool isTerminated = true; //(tagVersion == TagVersion.ID3v24);
                for (int i = 0; i < Items.Count; i++)
                {
                    ILanguageItem languageItem = Items[i];
                    if (i == Items.Count - 1) 
                        isTerminated = false;
                    frameData.Write(ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, languageItem.LanguageCode, isTerminated));
                }
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

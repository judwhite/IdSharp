using System;
using System.Diagnostics;
using System.IO;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Extensions;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class Comments : Frame, IComments
    {
        private EncodingType _textEncoding;
        private string _languageCode;
        private string _description;
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
                if (string.IsNullOrEmpty(value))
                {
                    _languageCode = "eng";
                }
                else
                {
                    _languageCode = value.ToLower().Trim();

                    // Language code must be 3 characters
                    if (_languageCode.Length != 3)
                    {
                        string msg = string.Format("Invalid language code '{0}' in COMM frame", value);
                        Trace.WriteLine(msg);

                        // TODO: Should this fire a warning?
                        if (_languageCode.Length > 3)
                            _languageCode = _languageCode.Substring(0, 3);
                        else
                            _languageCode = _languageCode.PadRight(3, ' ');
                    }
                }

                RaisePropertyChanged("LanguageCode");
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
                    return "COMM";
                case ID3v2TagVersion.ID3v22:
                    return "COM";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);

            // Sometimes a frame size of "0" comes through (which is explicitly forbidden in spec)
            if (_frameHeader.FrameSizeExcludingAdditions >= 1)
            {
                TextEncoding = (EncodingType)stream.Read1();

                // TODO: A common mis-implementation is to exclude the language code and description
                // Haven't decided how to handle this yet.  Maybe if a lookup to the language table fails,
                // the rest of the frame should be treated as suspicious.

                if (_frameHeader.FrameSizeExcludingAdditions >= 4)
                {
                    string languageCode = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    int bytesLeft = _frameHeader.FrameSizeExcludingAdditions - 1 - 3;
                    string description = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);

                    bool invalidFrame = false;
                    if (LanguageHelper.Languages.ContainsKey(languageCode.ToLower()) == false && languageCode.ToLower() != "xxx")
                    {
                        // most likely, it's en\0, or some other funk
                        if (languageCode.StartsWith("en"))
                        {
                            languageCode = "";
                        }

                        invalidFrame = true;

                        if (bytesLeft == 0)
                        {
                            Description = "";
                        }
                        else
                        {
                            Description = languageCode + description;
                        }
                        LanguageCode = "eng";
                    }
                    else
                    {
                        LanguageCode = languageCode;
                        Description = description;
                    }

                    if (bytesLeft > 0)
                    {
                        Value = ID3v2Utils.ReadString(TextEncoding, stream, bytesLeft);
                    }
                    else
                    {
                        if (invalidFrame)
                        {
                            if (languageCode.Contains("\0"))
                            {
                                // forget it, too messed up.
                                Value = "";
                            }
                            else
                            {
                                Value = languageCode + description;
                            }
                        }
                        else
                        {
                            Value = "";
                        }
                    }
                }
                else
                {
                    string msg = string.Format("Under-sized ({0} bytes) COMM frame at position {1}", _frameHeader.FrameSizeExcludingAdditions, stream.Position);
                    Trace.WriteLine(msg);

                    LanguageCode = "eng";
                    Value = "";
                }
            }
            else
            {
                string msg = string.Format("Under-sized ({0} bytes) COMM frame at position {1}", _frameHeader.FrameSizeExcludingAdditions, stream.Position);
                Trace.WriteLine(msg);

                LanguageCode = "eng";
                Value = "";
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (string.IsNullOrEmpty(Value))
                return new byte[0];

            if (LanguageCode == null || LanguageCode.Length != 3)
                LanguageCode = "eng";

            byte[] descriptionData;
            byte[] valueData;

            do
            {
                descriptionData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, Description, true);
                valueData = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, Value, false);
            } while (
                this.RequiresFix(tagVersion, Description, descriptionData) ||
                this.RequiresFix(tagVersion, Value, valueData)
            );

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.WriteByte((byte)TextEncoding);
                frameData.Write(ByteUtils.ISO88591GetBytes(LanguageCode));
                frameData.Write(descriptionData);
                frameData.Write(valueData);
                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

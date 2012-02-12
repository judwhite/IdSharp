using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Extensions;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class InvolvedPersonList : Frame, IInvolvedPersonList
    {
        private EncodingType _textEncoding;
        private readonly InvolvedPersonBindingList _involvedPersons;

        public InvolvedPersonList()
        {
            _involvedPersons = new InvolvedPersonBindingList();
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

        public BindingList<IInvolvedPerson> Items
        {
            get { return _involvedPersons; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                    return "TIPL";
                case ID3v2TagVersion.ID3v23:
                    return "IPLS";
                case ID3v2TagVersion.ID3v22:
                    return "IPL";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);
            _involvedPersons.Clear();

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
            if (bytesLeft > 0)
            {
                TextEncoding = (EncodingType)stream.Read1(ref bytesLeft);
                while (bytesLeft > 0)
                {
                    string involvement = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);
                    string name = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);

                    if (!string.IsNullOrEmpty(involvement) || !string.IsNullOrEmpty(name))
                    {
                        IInvolvedPerson involvedPerson = _involvedPersons.AddNew();
                        involvedPerson.Involvement = involvement;
                        involvedPerson.Name = name;
                    }
                }
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (Items.Count == 0)
                return new byte[0];

            // Sets appropriate TextEncoding if ISO-8859-1 is insufficient
            if (TextEncoding == EncodingType.ISO88591)
            {
                foreach (IInvolvedPerson involvedPerson in Items)
                {
                    if (string.IsNullOrEmpty(involvedPerson.Involvement) && string.IsNullOrEmpty(involvedPerson.Name))
                        continue;

                    byte[] involvementData = ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, involvedPerson.Involvement, true);
                    byte[] nameData = ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, involvedPerson.Name, true);

                    if (this.RequiresFix(tagVersion, involvedPerson.Involvement, involvementData))
                        break;
                    if (this.RequiresFix(tagVersion, involvedPerson.Name, nameData))
                        break;
                }
            }

            using (MemoryStream frameData = new MemoryStream())
            {
                frameData.WriteByte((byte)_textEncoding);

                bool foundItem = false;
                foreach (IInvolvedPerson involvedPerson in Items)
                {
                    if (string.IsNullOrEmpty(involvedPerson.Involvement) && string.IsNullOrEmpty(involvedPerson.Name))
                        continue;

                    frameData.Write(ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, involvedPerson.Involvement, true));
                    frameData.Write(ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, involvedPerson.Name, true));
                    foundItem = true;
                }

                if (!foundItem)
                    return new byte[0];

                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
        }
    }
}

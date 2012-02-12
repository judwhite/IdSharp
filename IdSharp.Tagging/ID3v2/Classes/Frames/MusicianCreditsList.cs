using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class MusicianCreditsList : Frame, IMusicianCreditsList
    {
        private EncodingType _textEncoding;
        private readonly MusicianCreditsItemBindingList _musicianCreditsItemBindingList;
        
        public MusicianCreditsList()
        {
            _musicianCreditsItemBindingList = new MusicianCreditsItemBindingList();
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

        public BindingList<IMusicianCreditsItem> Items
        {
            get { return _musicianCreditsItemBindingList; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23: // TODO: Should probably map to IPLS for 2.3
                    return "TMCL";
                case ID3v2TagVersion.ID3v22:
                    return null;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _musicianCreditsItemBindingList.Clear();
            throw new NotImplementedException();
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (Items.Count == 0)
                return new byte[0];

            throw new NotImplementedException();
        }
    }
}

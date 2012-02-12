using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class SynchronizedTempoCodes : Frame, ISynchronizedTempoCodes
    {
        private TimestampFormat _timestampFormat;
        private readonly TempoDataBindingList _tempoDataBindingList;

        public SynchronizedTempoCodes()
        {
            _tempoDataBindingList = new TempoDataBindingList();
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

        public BindingList<ITempoData> Items
        {
            get { return _tempoDataBindingList; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "SYTC";
                case ID3v2TagVersion.ID3v22:
                    return "STC";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _tempoDataBindingList.Clear();
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

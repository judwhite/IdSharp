using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class EventTiming : Frame, IEventTiming
    {
        private TimestampFormat _timestampFormat;
        private readonly EventTimingItemBindingList _eventTimingItemBindingList;

        public EventTiming()
        {
            _eventTimingItemBindingList = new EventTimingItemBindingList();
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

        public BindingList<IEventTimingItem> Items
        {
            get { return _eventTimingItemBindingList; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "ETCO";
                case ID3v2TagVersion.ID3v22:
                    return "ETC";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _eventTimingItemBindingList.Clear();
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

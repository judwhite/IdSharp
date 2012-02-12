using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class MpegLookupTable : Frame, IMpegLookupTable
    {
        private int _framesBetweenReference;
        private int _bytesBetweenReference;
        private int _millisecondsBetweenReference;
        private readonly MpegLookupTableItemBindingList _mpegLookupTableItemBindingList;

        public MpegLookupTable()
        {
            _mpegLookupTableItemBindingList = new MpegLookupTableItemBindingList();
        }

        public int FramesBetweenReference
        {
            get { return _framesBetweenReference; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                else if (value > 0xFFFF)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be greater than 0xFFFF");

                _framesBetweenReference = value;
                RaisePropertyChanged("FramesBetweenReference");
            }
        }

        public int BytesBetweenReference
        {
            get
            {
                return _bytesBetweenReference;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                else if (value > 0xFFFFFF)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be greater than 0xFFFFFF");

                _bytesBetweenReference = value;
                RaisePropertyChanged("BytesBetweenReference");
            }
        }

        public int MillisecondsBetweenReference
        {
            get
            {
                return _millisecondsBetweenReference;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                else if (value > 0xFFFFFF)
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be greater than 0xFFFFFF");

                _millisecondsBetweenReference = value;
                RaisePropertyChanged("MillisecondsBetweenReference");
            }
        }

        public BindingList<IMpegLookupTableItem> Items
        {
            get { return _mpegLookupTableItemBindingList; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "MLLT";
                case ID3v2TagVersion.ID3v22:
                    return "MLL";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _mpegLookupTableItemBindingList.Clear();
            throw new NotImplementedException();
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (FramesBetweenReference == 0 ||
                BytesBetweenReference == 0 ||
                MillisecondsBetweenReference == 0 ||
                Items.Count == 0)
            {
                return new byte[0];
            }

            throw new NotImplementedException();
        }
    }
}

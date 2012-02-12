using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Tagging.ID3v2.Frames.Items;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class EqualizationList : Frame, IEqualizationList
    {
        //private Int16 m_AdjustmentBits;
        private InterpolationMethod _interpolationMethod;
        private string _identification;
        private readonly BindingList<IEqualizationItem> _items;

        public EqualizationList()
        {
            _items = new EqualizationItemBindingList();
        }

        /*// TODO : This will probably come out and adjustment bits will be interpreted
        public Int16 AdjustmentBits
        {
            get
            {
                return m_AdjustmentBits;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }

                m_AdjustmentBits = value;
                FirePropertyChanged("AdjustmentBits");
            }
        }*/

        public InterpolationMethod InterpolationMethod
        {
            get { return _interpolationMethod; }
            set
            {
                _interpolationMethod = value;
                RaisePropertyChanged("InterpolationMethod");
            }
        }

        public string Identification
        {
            get { return _identification; }
            set
            {
                _identification = value;
                RaisePropertyChanged("Identification");
            }
        }

        public BindingList<IEqualizationItem> Items
        {
            get { return _items; }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                    return "EQU2";
                case ID3v2TagVersion.ID3v23:
                    return "EQUA";
                case ID3v2TagVersion.ID3v22:
                    return "EQU";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;

            if (bytesLeft != 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }

            throw new NotImplementedException();
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            throw new NotImplementedException();
        }
    }
}

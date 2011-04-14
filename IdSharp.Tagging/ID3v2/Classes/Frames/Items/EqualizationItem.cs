using System;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class EqualizationItem : IEqualizationItem
    {
        public VolumeAdjustmentDirection VolumeAdjustment
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public short Frequency
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Value cannot be less than 0");

                throw new NotImplementedException();
            }
        }

        public int Adjustment
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Value cannot be less than 0");

                throw new NotImplementedException();
            }
        }
    }
}

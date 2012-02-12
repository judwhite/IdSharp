using System;
using System.Diagnostics;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class RelativeVolumeAdjustment : Frame, IRelativeVolumeAdjustment
    {
        private const byte _id3v24BitsRepresentingPeak = 16;

        private string _identification;
        private decimal _frontRightAdjustment;
        private decimal _frontLeftAdjustment;
        private decimal _backRightAdjustment;
        private decimal _backLeftAdjustment;
        private decimal _frontCenterAdjustment;
        private decimal _subwooferAdjustment;
        private decimal _backCenterAdjustment;
        private decimal _otherAdjustment;
        private decimal _masterAdjustment;
        private decimal _frontRightPeak;
        private decimal _frontLeftPeak;
        private decimal _backRightPeak;
        private decimal _backLeftPeak;
        private decimal _frontCenterPeak;
        private decimal _subwooferPeak;
        private decimal _backCenterPeak;
        private decimal _otherPeak;
        private decimal _masterPeak;

        public string Identification
        {
            get { return _identification; }
            set
            {
                _identification = value;
                RaisePropertyChanged("Identification");
            }
        }

        public decimal FrontRightAdjustment
        {
            get { return _frontRightAdjustment; }
            set
            {
                _frontRightAdjustment = value;
                RaisePropertyChanged("FrontRightAdjustment");
            }
        }

        public decimal FrontLeftAdjustment
        {
            get { return _frontLeftAdjustment; }
            set
            {
                _frontLeftAdjustment = value;
                RaisePropertyChanged("FrontLeftAdjustment");
            }
        }

        public decimal BackRightAdjustment
        {
            get { return _backRightAdjustment; }
            set
            {
                _backRightAdjustment = value;
                RaisePropertyChanged("BackRightAdjustment");
            }
        }

        public decimal BackLeftAdjustment
        {
            get { return _backLeftAdjustment; }
            set
            {
                _backLeftAdjustment = value;
                RaisePropertyChanged("BackLeftAdjustment");
            }
        }

        public decimal FrontCenterAdjustment
        {
            get { return _frontCenterAdjustment; }
            set
            {
                _frontCenterAdjustment = value;
                RaisePropertyChanged("FrontCenterAdjustment");
            }
        }

        public decimal SubwooferAdjustment
        {
            get { return _subwooferAdjustment; }
            set
            {
                _subwooferAdjustment = value;
                RaisePropertyChanged("SubwooferAdjustment");
            }
        }

        public decimal BackCenterAdjustment
        {
            get { return _backCenterAdjustment; }
            set
            {
                _backCenterAdjustment = value;
                RaisePropertyChanged("BackCenterAdjustment");
            }
        }

        public decimal OtherAdjustment
        {
            get { return _otherAdjustment; }
            set
            {
                _otherAdjustment = value;
                RaisePropertyChanged("OtherAdjustment");
            }
        }

        public decimal MasterAdjustment
        {
            get { return _masterAdjustment; }
            set
            {
                _masterAdjustment = value;
                RaisePropertyChanged("MasterAdjustment");
            }
        }

        public decimal FrontRightPeak
        {
            get { return _frontRightPeak; }
            set
            {
                _frontRightPeak = value;
                RaisePropertyChanged("FrontRightPeak");
            }
        }

        public decimal FrontLeftPeak
        {
            get { return _frontLeftPeak; }
            set
            {
                _frontLeftPeak = value;
                RaisePropertyChanged("FrontLeftPeak");
            }
        }

        public decimal BackRightPeak
        {
            get { return _backRightPeak; }
            set
            {
                _backRightPeak = value;
                RaisePropertyChanged("BackRightPeak");
            }
        }

        public decimal BackLeftPeak
        {
            get { return _backLeftPeak; }
            set
            {
                _backLeftPeak = value;
                RaisePropertyChanged("BackLeftPeak");
            }
        }

        public decimal FrontCenterPeak
        {
            get { return _frontCenterPeak; }
            set
            {
                _frontCenterPeak = value;
                RaisePropertyChanged("FrontCenterPeak");
            }
        }

        public decimal SubwooferPeak
        {
            get { return _subwooferPeak; }
            set
            {
                _subwooferPeak = value;
                RaisePropertyChanged("SubwooferPeak");
            }
        }

        public decimal BackCenterPeak
        {
            get { return _backCenterPeak; }
            set
            {
                _backCenterPeak = value;
                RaisePropertyChanged("BackCenterPeak");
            }
        }

        public decimal OtherPeak
        {
            get { return _otherPeak; }
            set
            {
                _otherPeak = value;
                RaisePropertyChanged("OtherPeak");
            }
        }

        public decimal MasterPeak
        {
            get { return _masterPeak; }
            set
            {
                _masterPeak = value;
                RaisePropertyChanged("MasterPeak");
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                    return "RVA2";
                case ID3v2TagVersion.ID3v23:
                    return "RVAD";
                case ID3v2TagVersion.ID3v22:
                    return "RVA";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            // RVAD/RVA2
            /*Double original = -65534;
            Double newVal = Math.Log10(1 + original/65535.0)*20.0*512.0;
            Double original2 = Math.Pow(10, newVal/(20.0*512.0));
            original2 = original2 - 1;
            original2 *= 65535.0;*/

            /*Double original = 10000;
            Double newVal = Math.Log10(1 + original / 65535.0);
            Double original2 = Math.Pow(10, newVal);
            original2 = original2 - 1;
            original2 *= 65535.0;*/

            _frameHeader.Read(tagReadingInfo, ref stream);

            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
            if (bytesLeft > 0)
            {
                // todo: there needs to be some kind of a test to see if this is RVAD/RVA2 format, too
                // much varying implementation in 2.3 and 2.4

                bool isRVA2 = (_frameHeader.TagVersion == ID3v2TagVersion.ID3v24);

                if (isRVA2)
                {
                    // sometimes "identification" is completely ommitted... grr
                    Identification = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);
                    while (bytesLeft >= 3)
                    {
                        // TODO: Implementation not complete
                        byte channelType = stream.Read1(ref bytesLeft);
                        //if (channelType == 16) break; // Invalid, probably stored as an ID3v2.3 RVAD frame
                        // TODO: some kind of switch.. maybe a new internal enum
                        short volumeAdjustment = stream.ReadInt16(ref bytesLeft);
                        if (bytesLeft > 0)
                        {
                            // sometimes represented as BITS representing peak.. seriously.
                            byte bytesRepresentingPeak = stream.Read1(ref bytesLeft);
                            if (bytesRepresentingPeak == 0) break;
                            if (bytesLeft >= bytesRepresentingPeak)
                            {
                                // TODO: Finish implementation
                                byte[] peakVolume = stream.Read(bytesRepresentingPeak);
                                bytesLeft -= peakVolume.Length;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (bytesLeft > 0)
                    {
                        //Trace.WriteLine("Invalid RVA2 frame");
                        //stream.Seek(bytesLeft, SeekOrigin.Current);
                        //bytesLeft = 0;
                        // Try to read it like an ID3v2.3 RVAD frame
                        stream.Seek(bytesLeft - _frameHeader.FrameSizeExcludingAdditions, SeekOrigin.Current);
                        bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
                        isRVA2 = false;
                    }
                    else
                    {
                        // TODO
                        //MessageBox.Show("valid RVA2 frame, omg!");
                    }
                }

                // ID3v2.2, ID3v2.3, or mal-formed ID3v2.4
                if (isRVA2 == false)
                {
                    byte incrementDecrement = stream.Read1(ref bytesLeft);
                    if (bytesLeft > 0)
                    {
                        byte bitsUsedForVolumeDescription = stream.Read1(ref bytesLeft);
                        int bytesUsedForVolumeDescription = bitsUsedForVolumeDescription / 8;

                        // TODO: (may be useful for testing which implementation)
                        // if bits used for volume description is > 64, don't bother

                        // Relative volume change, right
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            FrontRightAdjustment = ByteUtils.ConvertToInt64(byteArray) * (ByteUtils.IsBitSet(incrementDecrement, 0) ? 1 : -1);
                        }
                        // Relative volume change, left
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            FrontLeftAdjustment = ByteUtils.ConvertToInt64(byteArray) * (ByteUtils.IsBitSet(incrementDecrement, 1) ? 1 : -1);
                        }
                        // Peak volume right
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            FrontRightPeak = ByteUtils.ConvertToInt64(byteArray);
                        }
                        // Peak volume left
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            FrontLeftPeak = ByteUtils.ConvertToInt64(byteArray);
                        }
                        // Relative volume change, right back
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            BackRightAdjustment = ByteUtils.ConvertToInt64(byteArray) * (ByteUtils.IsBitSet(incrementDecrement, 2) ? 1 : -1);
                        }
                        // Relative volume change, left back
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            BackLeftAdjustment = ByteUtils.ConvertToInt64(byteArray) * (ByteUtils.IsBitSet(incrementDecrement, 3) ? 1 : -1);
                        }
                        // Peak volume right back
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            BackRightPeak = ByteUtils.ConvertToInt64(byteArray);
                        }
                        // Peak volume left back
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            BackLeftPeak = ByteUtils.ConvertToInt64(byteArray);
                        }
                        // Relative volume change, center
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            FrontCenterAdjustment = ByteUtils.ConvertToInt64(byteArray) * (ByteUtils.IsBitSet(incrementDecrement, 4) ? 1 : -1);
                        }
                        // Peak volume center
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            FrontCenterPeak = ByteUtils.ConvertToInt64(byteArray);
                        }
                        // Relative volume change, bass
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            SubwooferAdjustment = ByteUtils.ConvertToInt64(byteArray) * (ByteUtils.IsBitSet(incrementDecrement, 5) ? 1 : -1);
                        }
                        // Peak volume bass
                        if (bytesLeft >= bytesUsedForVolumeDescription)
                        {
                            byte[] byteArray = stream.Read(bytesUsedForVolumeDescription, ref bytesLeft);
                            SubwooferPeak = ByteUtils.ConvertToInt64(byteArray);
                        }
                    }
                }

                // Skip past the rest of the frame
                if (bytesLeft > 0)
                {
                    Trace.WriteLine("Invalid RVA2/RVAD/RVA frame");
                    stream.Seek(bytesLeft, SeekOrigin.Current);
                    //bytesLeft = 0;
                }
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            // TODO
            return new byte[0];
            // throw new NotImplementedException();

            /*

            using (MemoryStream frameData = new MemoryStream())
            {
                if (tagVersion == TagVersion.ID3v22 || tagVersion == TagVersion.ID3v23)
                {
                    Byte adjustmentDirection = 0;

                    decimal maxValue = 0;
                    maxValue = Math.Max(maxValue, Math.Abs(_frontRightAdjustment));
                    maxValue = Math.Max(maxValue, Math.Abs(_frontLeftAdjustment));
                    maxValue = Math.Max(maxValue, Math.Abs(_frontRightPeak));
                    maxValue = Math.Max(maxValue, Math.Abs(_frontLeftPeak));

                    if (_frontRightAdjustment > 0) adjustmentDirection |= 0x01;
                    if (_frontLeftAdjustment > 0) adjustmentDirection |= 0x02;

                    if (tagVersion == TagVersion.ID3v23)
                    {
                        maxValue = Math.Max(maxValue, Math.Abs(_backRightAdjustment));
                        maxValue = Math.Max(maxValue, Math.Abs(_backLeftAdjustment));
                        maxValue = Math.Max(maxValue, Math.Abs(_backRightPeak));
                        maxValue = Math.Max(maxValue, Math.Abs(_backLeftPeak));
                        maxValue = Math.Max(maxValue, Math.Abs(_frontCenterAdjustment));
                        maxValue = Math.Max(maxValue, Math.Abs(_frontCenterPeak));
                        maxValue = Math.Max(maxValue, Math.Abs(_subwooferAdjustment));
                        maxValue = Math.Max(maxValue, Math.Abs(_subwooferPeak));

                        if (_backRightAdjustment > 0) adjustmentDirection |= 0x04;
                        if (_backLeftAdjustment > 0) adjustmentDirection |= 0x08;
                        if (_frontCenterAdjustment > 0) adjustmentDirection |= 0x10;
                        if (_subwooferAdjustment > 0) adjustmentDirection |= 0x20;
                    }

                    int bitsForVolume;
                    if (maxValue <= 0xFF) bitsForVolume = 8;
                    else if (maxValue <= 0xFFFF) bitsForVolume = 16;
                    else if (maxValue <= 0xFFFFFF) bitsForVolume = 24;
                    else if (maxValue <= 0xFFFFFFFF) bitsForVolume = 32;
                    else throw new InvalidOperationException("Maximum volume adjustment is 2^32");

                    int bytesForVolume = bitsForVolume / 8;

                    frameData.WriteByte(adjustmentDirection);
                    frameData.WriteByte((Byte)bitsForVolume);
                    frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_frontRightAdjustment), bytesForVolume));
                    frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_frontLeftAdjustment), bytesForVolume));
                    frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_frontRightPeak), bytesForVolume));
                    frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_frontLeftPeak), bytesForVolume));

                    if (tagVersion == TagVersion.ID3v23)
                    {
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_backRightAdjustment), bytesForVolume));
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_backLeftAdjustment), bytesForVolume));
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_backRightPeak), bytesForVolume));
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_backLeftPeak), bytesForVolume));
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_frontCenterAdjustment), bytesForVolume));
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_frontCenterPeak), bytesForVolume));
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_subwooferAdjustment), bytesForVolume));
                        frameData.Write(ByteUtils.ConvertToByteArray(Math.Abs(_subwooferPeak), bytesForVolume));
                    }
                }
                else if (tagVersion == TagVersion.ID3v24)
                {
                    // TODO: Test

                    frameData.Write(ID3v2Utils.GetStringBytes(tagVersion, EncodingType.ISO_8859_1, Identification, true));

                    WriteID3v24ChannelItem(frameData, ChannelType.Other, _otherAdjustment, _otherPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.MasterVolume, _masterAdjustment, _masterPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.FrontRight, _frontRightAdjustment, _frontRightPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.FrontLeft, _frontLeftAdjustment, _frontLeftPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.BackRight, _backRightAdjustment, _backRightPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.BackLeft, _backLeftAdjustment, _backLeftPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.FrontCenter, _frontCenterAdjustment, _frontCenterPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.BackCenter, _backCenterAdjustment, _backCenterPeak);
                    WriteID3v24ChannelItem(frameData, ChannelType.Subwoofer, _subwooferAdjustment, _subwooferPeak);
                }
                else
                {
                    throw new ArgumentException("Unknown tag version");
                }

                return _frameHeader.GetBytes(frameData, tagVersion, GetFrameID(tagVersion));
            }
             */
        }

        private void WriteID3v24ChannelItem(MemoryStream memoryStream, ChannelType channelType, decimal adjustment, decimal peak)
        {
            if (adjustment != 0 || peak != 0)
            {
                memoryStream.WriteByte((byte)channelType);

                // TODO                
                //Utils.Write(memoryStream, ByteUtils.ConvertDecimalToByteArray(adjustment));
                if (adjustment <= 64 && adjustment >= -64)
                {

                }
                else
                {

                }

                throw new NotImplementedException();
                /*
                memoryStream.WriteByte(_iD3v24BitsRepresentingPeak);
                ByteUtils.Write(memoryStream, ByteUtils.ConvertToByteArray(Math.Abs(peak), _iD3v24BitsRepresentingPeak/8));
                */
            }
        }
    }
}

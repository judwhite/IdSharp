using System;
using System.IO;
using System.Text;
using IdSharp.Common.Utils;

namespace IdSharp.AudioInfo
{
    /// <summary>
    /// MPEG
    /// </summary>
    public class Mpeg : IAudioFile
    {
        static Mpeg()
        {
            BitrateTable = new[]
            {
                new[]
                { 
                    // MPEG-2.5
                    new[] {  8, 16, 24, 32, 40, 48,  56,  64,  80,  96, 112, 128, 144, 160 }, // Layer-3
                    new[] {  8, 16, 24, 32, 40, 48,  56,  64,  80,  96, 112, 128, 144, 160 }, // Layer-2
                    new[] { 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256 }  // Layer-1
                },
                new int[3][],
                new[]
                {
                    // MPEG-2
                    new[] {  8, 16, 24, 32, 40, 48,  56,  64,  80,  96, 112, 128, 144, 160 }, // Layer-3
                    new[] {  8, 16, 24, 32, 40, 48,  56,  64,  80,  96, 112, 128, 144, 160 }, // Layer-2
                    new[] { 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256 }  // Layer-1
                },
                new[]
                {
                    // MPEG-1
                    new[] { 32, 40, 48, 56, 64, 80,  96, 112, 128, 160, 192, 224, 256, 320 },   // Layer-3
                    new[] { 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384 },   // Layer-2
                    new[] { 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448 } // Layer-1
                }
            };
        }

        // References:
        // http://www.mp3-tech.org/programmer/frame_header.html

        private static readonly int[][][] BitrateTable;
        private static readonly byte[] INFO_MARKER = Encoding.ASCII.GetBytes("Info");
        private static readonly byte[] XING_MARKER = Encoding.ASCII.GetBytes("Xing");

        private readonly int _frequency;
        private decimal _totalSeconds;
        private readonly int _channels;
        //private readonly long _samples = 0;
        private readonly int _samplesPerFrame;
        private readonly byte fh1;
        private readonly byte fh2;
        private readonly MpegVersion _mpegVersion;
        private readonly MpegLayer _mpegLayer;
        private readonly bool _isPrivate;
        private readonly bool _isCopyright;
        private readonly bool _isOriginal;
        private readonly double _frameSizeConst;
        private readonly int _paddingSizeConst;
        private readonly long _headerOffset;
        private readonly string _fileName;
        private decimal _bitrate;

        private bool? _isVBR;
        private int _frames;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mpeg"/> class.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <param name="calculateBitrate">if set to <c>true</c> the bitrate will be calculated before the constructor returns.</param>
        public Mpeg(string path, bool calculateBitrate)
        {
            _fileName = path;

            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int tmpID3v2TagSize = ID3v2.GetTagSize(stream);
                stream.Seek(tmpID3v2TagSize, SeekOrigin.Begin);

                byte[] tmpFrameHeader = new byte[4];

                bool acceptNullSamples = false;
                while (true)
                {
                    int tmpByte = stream.ReadByte(); // keep as ReadByte
                    while (tmpByte != 0xFF && tmpByte != -1)
                    {
                        tmpByte = stream.ReadByte(); // keep as ReadByte
                    }

                    if (tmpByte == -1)
                    {
                        if (acceptNullSamples)
                        {
							throw new InvalidDataException(string.Format("'{0}': Can't find frame sync", path));
                        }
                        stream.Seek(tmpID3v2TagSize, SeekOrigin.Begin);
                        acceptNullSamples = true;
                        continue;
                    }

                    tmpFrameHeader[0] = (byte)tmpByte;

                    // Get frame header
                    if (stream.Read(tmpFrameHeader, 1, 3) != 3)
                    {
                        throw new InvalidDataException(string.Format("'{0}': Invalid MPEG file; end of stream reached", path));
                    }

                    // No sync
                    if ((tmpFrameHeader[1] >> 5) != 0x07 ||
                        ((tmpFrameHeader[1] >> 1) & 0x03) == 0) // 2/18/05 - ignore reserved layer
                    {
                        stream.Seek(-3, SeekOrigin.Current);
                    }
                    else if (tmpFrameHeader[1] == 0xFF ||
                            ((tmpFrameHeader[1] >> 3) & 0x03) == 1) // 2/19/05 - more bad data
                    {
                        stream.Seek(-3, SeekOrigin.Current);
                    }
                    else
                    {
                        int tmpMpegID = (tmpFrameHeader[1] >> 3) & 0x03;
                        int tmpLayerNum = (tmpFrameHeader[1] >> 1) & 0x03;
                        int tmpFrequency = GetFrequency((MpegVersion)tmpMpegID, (tmpFrameHeader[2] >> 2) & 0x03);

                        // Check for invalid frequency
                        if (tmpFrequency == 0)
                        {
                            stream.Seek(-3, SeekOrigin.Current);
                            continue;
                        }

                        int tmpSamplesPerFrame = GetSamplesPerFrame((MpegVersion)tmpMpegID, (MpegLayer)tmpLayerNum);

                        int tmpUsesPadding = (tmpFrameHeader[2] >> 1) & 0x01;
                        double tmpFrameSizeConst = 125.0 * tmpSamplesPerFrame / tmpFrequency;
                        int tmpPaddingSize = (tmpLayerNum == 3 ? 4 : 1);
                        int tmpBitrateIndex = tmpFrameHeader[2] >> 4;

                        // Check for invalid values
                        if (tmpBitrateIndex < 1 || tmpBitrateIndex > 14 || tmpLayerNum == 0)
                        {
                            stream.Seek(-3, SeekOrigin.Current);
                            continue;
                        }

                        int tmpFrameBitrate = GetBitrate((MpegVersion)tmpMpegID, (MpegLayer)tmpLayerNum, tmpBitrateIndex);
                        int tmpFrameSize = (int)(tmpFrameBitrate * tmpFrameSizeConst) + (tmpUsesPadding * tmpPaddingSize);
                        _headerOffset = stream.Position - 4;

                        if (tmpFrameSize < 8)
                        {
                            stream.Seek(-3, SeekOrigin.Current);
                            continue;
                        }

                        // 7/21/05 - Check for 0x00 or 0xFF at end of last frame
                        // this sucks for tracks that start with silence
                        if (_headerOffset >= 1 && !acceptNullSamples) // if (ftell(fp) >= 5)
                        {
                            stream.Seek(-5, SeekOrigin.Current);
                            byte tmpLastByte = stream.Read1();
                            stream.Seek(4, SeekOrigin.Current);

                            if (tmpFrameBitrate != 320 && (tmpLastByte == 0x00 || tmpLastByte == 0xFF))
                            {
                                // 7/31/05
                                // may be a valid frame - skip its contents to prevent false sync
                                long tmpNewPosition = _headerOffset + tmpFrameSize;
                                if (tmpFrameSize == 0) 
                                    tmpNewPosition++;
                                stream.Seek(tmpNewPosition, SeekOrigin.Begin);
                                continue;
                            }
                        }

                        /*if (BR == 0 || FrameSizeConst == 0)
                        {
                            startpos = HeaderOffset+1;
                            fseek(fp, startpos, SEEK_SET);
                            continue;
                        }*/

                        stream.Seek(_headerOffset + tmpFrameSize, SeekOrigin.Begin);
                        if (stream.Read1() == 0xFF)
                        {
                            fh1 = stream.Read1();
                            fh2 = stream.Read1();

                            if (tmpFrameHeader[1] == fh1 &&
                                (tmpFrameHeader[2] & 0x0D) == (fh2 & 0x0D))
                            {
                                // header found
                                break;
                            }
                        }

                        stream.Seek(_headerOffset + 1, SeekOrigin.Begin);
                        continue;
                    }
                }

                _mpegVersion = (MpegVersion)((tmpFrameHeader[1] >> 3) & 0x03);
                _mpegLayer = (MpegLayer)((tmpFrameHeader[1] >> 1) & 0x03);
                _frequency = GetFrequency(_mpegVersion, (tmpFrameHeader[2] >> 2) & 0x03);
                if (_frequency == 0)
                {
                    throw new InvalidDataException(String.Format("'{0}'; cannot determine frequency", path));
                }

                _isPrivate = ((tmpFrameHeader[2] & 0x01) == 0x01);
                _samplesPerFrame = GetSamplesPerFrame(_mpegVersion, _mpegLayer);
                _frameSizeConst = 125.0 * _samplesPerFrame / _frequency;
                _paddingSizeConst = (_mpegLayer == MpegLayer.Layer1 ? 4 : 1);
                _isCopyright = (((tmpFrameHeader[3] >> 3) & 0x01) == 0x01);
                _isOriginal = (((tmpFrameHeader[3] >> 2) & 0x01) == 0x01);
                //tmpModeExtension = (FH[3] >> 4) & 0x03; // not interested, only used in joint-stereo
                //_mpegEmphasis = (MpegEmphasis)(tmpFrameHeader[3] & 0x03);

                if ((tmpFrameHeader[3] >> 6) == 3) _channels = 1; // Single Channel
                else _channels = 2;

                // Read LAME Info Tag
                bool tmpHasLameInfoTag = false;

                stream.Seek(tmpID3v2TagSize + 36, SeekOrigin.Begin);
                Byte[] buf = stream.Read(4);

                if (ByteUtils.Compare(buf, INFO_MARKER)) // CBR
                {
                    tmpHasLameInfoTag = true;
                    _isVBR = false;
                }
                else if (ByteUtils.Compare(buf, XING_MARKER)) // VBR
                {
                    tmpHasLameInfoTag = true;
                    _isVBR = true;
                }

                if (tmpHasLameInfoTag)
                {
                    stream.Seek(4, SeekOrigin.Current);
                    int tmpFrames = stream.ReadInt32();
                    uint tmpBytes = (uint)stream.ReadInt32();

                    if (tmpFrames > 256 && tmpBytes > 50000)
                    {
                        decimal tmpBitrate = tmpBytes / 125.0m / (tmpFrames * _samplesPerFrame / (decimal)_frequency);
                        if (tmpBitrate <= 320 && tmpBitrate >= 32)
                        {
                            _frames = tmpFrames;
                            _bitrate = tmpBitrate;
                            _totalSeconds = (tmpBytes / 125.0m) / _bitrate;
                        }
                    }
                }

                // TODO: Take these 2 lines out
                /*fs.Position = 0;
                CalculateBitrate(fs, null);*/

                if (calculateBitrate)
                {
                    if (_bitrate == 0 || _isVBR == null || _frames == 0 || _totalSeconds == 0)
                    {
                        stream.Position = 0;
                        CalculateBitrate(stream, null);
                    }
                }
            }
        }

        private static int GetBitrate(MpegVersion mpegVersion, MpegLayer mpegLayer, int bitrateIndex)
        {
            return BitrateTable[(int)mpegVersion][(int)mpegLayer - 1][bitrateIndex - 1];
        }

        private class CInterestedFrames
        {
            // TODO: stuff for split CUE
        }

        private void CalculateBitrate(Stream stream, CInterestedFrames InterestedFrames)
        {
            int totalTagSize = ID3v2.GetTagSize(stream);
            totalTagSize += ID3v1.GetTagSize(stream);
            totalTagSize += APEv2.GetTagSize(stream);

            Int64 audioLength = stream.Length - totalTagSize;

            _bitrate = 0;
            _isVBR = null;
            _frames = 0;
            _totalSeconds = 0;

            String step = "";

            try
            {
                int BR, Padding;
                int FrameSize;

                int TotalBR = 0;
                int FrameOffset = 0;
                //Int64 TagOffset = 0;
                bool bTrusting = true;
                bool ignoreall = false;
                Byte[] FH = new Byte[4];
                bool mPerfect = true;

                stream.Position = _headerOffset;

                //                if (_headerOffset > 0)
                //                {
                //                    TagOffset = _headerOffset;
                //                }

                int offset = 0;
                int frameCount = 0;
                int FirstBR = 0;

                int audioDataSize = (int)(stream.Length - _headerOffset);
                Byte[] audioData = new Byte[audioDataSize];
                //Int64 startoffset = stream.Position;
                int BufLen = stream.Read(audioData, 0, audioDataSize);
                while (offset < BufLen - 16 && !ignoreall)
                {
                    bool reservedlayer = false;

                    // Find FrameSync
                    if (FindFrameSync(FH, audioData, BufLen, ref FrameOffset, ref offset, ref mPerfect, ref ignoreall, ref bTrusting, ref reservedlayer))
                    {
                        FrameOffset = 0;

                        int bitrateIndex = FH[2] >> 4;
                        if (bitrateIndex <= 0 || bitrateIndex >= 15)
                        {
                            offset -= 3;
                            continue;
                        }

                        Padding = (FH[2] >> 1) & 0x01;
                        BR = GetBitrate(_mpegVersion, _mpegLayer, bitrateIndex);

                        if (BR == 0 || BR % 8 != 0)
                        {
                            offset -= 3;
                            continue;
                        }

                        //step = "last good frame @ " + String(startoffset + offset - 4) + ", frame " +
                        //       String(_frames + 1);

                        // todo: put back later
                        /*if (InterestedFrames != NULL)
                        {
                            if (((_frames + 1) * _samplesperframe / (float)_Frequency) * 75.0 >= InterestedFrames->CurrentFrame())
                            {
                                if (_frames == 0 || InterestedFrames->NoAccomodation)
                                {
                                    InterestedFrames->SetCurrentByteOffset(startoffset + offset - 4);
                                    InterestedFrames->SetCurrentFrameOffset(_frames);
                                }
                                InterestedFrames->SetCurrentByteEndOffset(startoffset + offset - 4);
                                InterestedFrames->Cur += 1;
                            }
                            else
                            {
                                InterestedFrames->SetCurrentByteOffset(startoffset + offset - 4);
                                InterestedFrames->SetCurrentFrameOffset(_frames);
                            }
                        }*/

                        if (_isVBR != true)
                        {
                            if (TotalBR == 0)
                            {
                                FirstBR = BR;
                            }
                            else if (BR != FirstBR)
                            {
                                _isVBR = true;
                            }
                        }

                        TotalBR += BR;

                        FrameSize = (int)(BR * _frameSizeConst + Padding * _paddingSizeConst);

                        offset += FrameSize - 4;
                        frameCount++;
                    }
                }// end while

                if (frameCount == 0)
                {
                    throw new InvalidDataException(String.Format("No frames found in {0}", _fileName));
                }

                _frames = frameCount;
                if (_isVBR == null)
                {
                    _bitrate = FirstBR;
                    _isVBR = false;
                }
                else
                {
                    _bitrate = TotalBR / _frames;
                }

                if (_bitrate == 0)
                {
                    throw new InvalidDataException(String.Format("Error determining bitrate: {0}", _fileName));
                }

                _totalSeconds = (audioLength / 125.0m) / _bitrate;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error calculating bitrate; {0}", step), ex);
            }
        }

        private bool FindFrameSync(Byte[] FH, Byte[] audioData, int BufLen, ref int FrameOffset, ref int offset, ref bool mPerfect, ref bool ignoreall, ref bool bTrusting, ref bool reservedlayer)
        {
            while (true)
            {
                if (FrameOffset == 0)
                {
                    if (offset >= BufLen - 8)
                    {
                        return false;
                    }
                    FH[0] = audioData[offset++];
                    while (FH[0] != 0xFF && offset < BufLen)
                    {
                        // Leading 0's
                        if (offset - 1 == 0 && FH[0] == 0)
                        {
                            mPerfect = false;
                            do
                            {
                                FH[0] = audioData[offset++];
                            } while (FH[0] == 0 && offset < BufLen);
                            continue;
                        }

                        if (FH[0] == 'L' && audioData[offset] == 'Y' && audioData[offset + 1] == 'R')
                        {
                            // todo: don't want to return, just stop reading data
                            ignoreall = true;
                            return false;
                        }

                        if (FH[0] == 'T' && audioData[offset] == 'A' && audioData[offset + 1] == 'G')
                        {
                            // todo: don't want to return, just stop reading data
                            ignoreall = true;
                            return false;
                        }

                        if (FH[0] == 'A' && audioData[offset] == 'P' && audioData[offset + 1] == 'E')
                        {
                            // todo: don't want to return, just stop reading data
                            ignoreall = true;
                            return false;
                        }

                        // ignore cut off frames
                        // todo: eh.. is this really important?
                        /*if (BufLen < cuiReadBufSize)
                        {
                            if (BufLen - offset < TagContainer->Lyrics3ExistingOffset + 128)
                            {
                                ignoreall = true;
                                mPerfect = false;
                                return;
                            }
                        }*/

                        FH[0] = audioData[offset++];

                        if (bTrusting == true)
                        {
                            mPerfect = false;
                            if (!reservedlayer)
                            {
                                bTrusting = false;
                            }
                        }
                    }

                    if (offset == BufLen)
                    {
                        return false;
                    }

                    FrameOffset = 1;
                } // end if (FrameOffset == 0)

                // Get frame header
                int i;
                for (i = FrameOffset; i < 4 && offset != BufLen; i++)
                {
                    FH[i] = audioData[offset++];
                }

                if (i != 4 && offset == BufLen)
                {
                    FrameOffset = i;
                    return false;
                }

                // No sync
                if ((FH[1] >> 5) != 0x07 || ((FH[1] >> 1) & 0x03) == 0) // 2/18/05 ignore reserved layer
                {
                    if (((FH[1] >> 1) & 0x03) == 0) reservedlayer = true;
                    FrameOffset = 0;
                    offset -= 3;
                    mPerfect = false;
                }
                else if (FH[1] != fh1 || (FH[2] & 0x0D) != (fh2 & 0x0D))
                {
                    // sync doesn't match expected type
                    //
                    FrameOffset = 0;
                    offset -= 3;
                    mPerfect = false;
                }
                else
                {
                    break;
                }
            } // end find frame sync

            return true;
        }

        private static int GetSamplesPerFrame(MpegVersion mpegVersion, MpegLayer mpegLayer)
        {
            int tmpSamplesPerFrame = 0;

            switch (mpegVersion)
            {
                // MPEG-1
                case MpegVersion.Mpeg1:
                    if (mpegLayer == MpegLayer.Layer1) tmpSamplesPerFrame = 384;
                    else if (mpegLayer == MpegLayer.Layer2 ||
                             mpegLayer == MpegLayer.Layer3) tmpSamplesPerFrame = 1152;
                    break;

                // MPEG-2/2.5
                case MpegVersion.Mpeg2:
                case MpegVersion.Mpeg25:
                    if (mpegLayer == MpegLayer.Layer1) tmpSamplesPerFrame = 384;
                    else if (mpegLayer == MpegLayer.Layer2) tmpSamplesPerFrame = 1152;
                    else if (mpegLayer == MpegLayer.Layer3) tmpSamplesPerFrame = 576;
                    break;
            } // end switch (ID)

            return tmpSamplesPerFrame;
        }

        private static int GetFrequency(MpegVersion mpegVersion, int frequencyID)
        {
            int tmpFrequency = 0;

            switch (mpegVersion)
            {
                // MPEG-1
                case MpegVersion.Mpeg1:
                    switch (frequencyID)
                    {
                        case 0:
                            tmpFrequency = 44100;
                            break;
                        case 1:
                            tmpFrequency = 48000;
                            break;
                        case 2:
                            tmpFrequency = 32000;
                            break;
                    } // end switch (Frequency)
                    break;

                // MPEG-2
                case MpegVersion.Mpeg2:
                    switch (frequencyID)
                    {
                        case 0:
                            tmpFrequency = 22050;
                            break;
                        case 1:
                            tmpFrequency = 24000;
                            break;
                        case 2:
                            tmpFrequency = 16000;
                            break;
                    } // end switch (Frequency)
                    break;

                // MPEG-2.5
                case MpegVersion.Mpeg25:
                    switch (frequencyID)
                    {
                        case 0:
                            tmpFrequency = 11025;
                            break;
                        case 1:
                            tmpFrequency = 12000;
                            break;
                        case 2:
                            tmpFrequency = 8000;
                            break;
                    } // end switch (Frequency)
                    break;
            } // end switch (ID)

            return tmpFrequency;
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get { return _frequency; }
        }

        private void CalculateBitrate()
        {
            using (FileStream fs = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                CalculateBitrate(fs, null);
            }
        }

        /// <summary>
        /// Gets the total seconds.
        /// </summary>
        /// <value>The total seconds.</value>
        public decimal TotalSeconds
        {
            get
            {
                if (_totalSeconds == 0) CalculateBitrate();
                return _totalSeconds;
            }
        }

        /// <summary>
        /// Gets the bitrate.
        /// </summary>
        /// <value>The bitrate.</value>
        public decimal Bitrate
        {
            get
            {
                if (_bitrate == 0) CalculateBitrate();
                return _bitrate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the bitrate has been determined.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is the bitrate has been determined; otherwise, <c>false</c>.
        /// </value>
        public bool IsBitrateDetermined
        {
            get { return _bitrate != 0; }
        }

        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        /// <value>The number of channels.</value>
        public int Channels
        {
            get { return _channels; }
        }

        /// <summary>
        /// Gets the type of the audio file.
        /// </summary>
        /// <value>The type of the audio file.</value>
        public AudioFileType FileType
        {
            get { return AudioFileType.Mpeg; }
        }

        /*/// <summary>
        /// Gets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        public long Samples
        {
            get 
            {
                if (_samples == 0) CalculateBitrate();
                return _samples; 
            }
        }*/

        /// <summary>
        /// Gets a value indicating whether the audio is encoded with a variable bitrate.
        /// </summary>
        /// <value><c>true</c> if the audio is encoded with a variable bitrate; otherwise, <c>false</c>.</value>
        public bool IsVBR
        {
            get
            {
                if (_isVBR == null) CalculateBitrate();
                return _isVBR.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the 'private' bit is set in the frame header.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the 'private' bit is set in the frame header; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrivate
        {
            get { return _isPrivate; }
        }

        /// <summary>
        /// Gets a value indicating whether the 'copyright' bit is set in the frame header.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the 'copyright' bit is set in the frame header; otherwise, <c>false</c>.
        /// </value>
        public bool IsCopyright
        {
            get { return _isCopyright; }
        }

        /// <summary>
        /// Gets a value indicating whether the 'original' bit is set in the frame header.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the 'original' bit is set in the frame header; otherwise, <c>false</c>.
        /// </value>
        public bool IsOriginal
        {
            get { return _isOriginal; }
        }
    }
}

using System;
using System.IO;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Extensions;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class AudioText : Frame, IAudioText
    {
        private static readonly byte[] _scrambleTable;

        private EncodingType _textEncoding;
        private string _mimeType;
        private string _equivalentText;
        private byte[] _audioData;
        private bool _isMpegOrAac;

        static AudioText()
        {
            _scrambleTable = new byte[127];
            _scrambleTable[0] = 0xFE;
            for (int i = 0; ; i++)
            {
                byte n = NextByte(_scrambleTable[i]);
                if (n == 0xFE)
                    break;
                _scrambleTable[i + 1] = n;
            }
        }

        private static byte[] Scramble(byte[] audioData)
        {
            byte[] newAudioData = new byte[audioData.Length];
            for (int i = 0, j = 0; i < audioData.Length; i++, j++)
            {
                newAudioData[i] = (byte)(audioData[i] ^ _scrambleTable[j]);
                if (j == 126) j = -1;
            }
            return newAudioData;
        }

        private static byte NextByte(byte n)
        {
            byte bit7 = (byte)((n >> 7) & 0x01);
            byte bit6 = (byte)((n >> 6) & 0x01);
            byte bit5 = (byte)((n >> 5) & 0x01);
            byte bit4 = (byte)((n >> 4) & 0x01);
            byte bit3 = (byte)((n >> 3) & 0x01);
            byte bit2 = (byte)((n >> 2) & 0x01);
            byte bit1 = (byte)((n >> 1) & 0x01);
            byte bit0 = (byte)(n & 0x01);

            byte newByte = (byte)(((bit6 ^ bit5) << 7) +
              ((bit5 ^ bit4) << 6) +
              ((bit4 ^ bit3) << 5) +
              ((bit3 ^ bit2) << 4) +
              ((bit2 ^ bit1) << 3) +
              ((bit1 ^ bit0) << 2) +
              ((bit7 ^ bit5) << 1) +
              (bit6 ^ bit4));

            return newByte;
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

        public string MimeType
        {
            get { return _mimeType; }
            set
            {
                _mimeType = value;
                RaisePropertyChanged("MimeType");
            }
        }

        public string EquivalentText
        {
            get { return _equivalentText; }
            set
            {
                _equivalentText = value;
                RaisePropertyChanged("EquivalentText");
            }
        }

        public void SetAudioData(string mimeType, byte[] audioData, bool isMpegOrAac)
        {
            MimeType = mimeType;
            _isMpegOrAac = isMpegOrAac;
            if (audioData == null)
            {
                _audioData = null;
            }
            else
            {
                if (_isMpegOrAac)
                    _audioData = ID3v2Utils.ConvertToUnsynchronized(_audioData);
                else
                    _audioData = Scramble(_audioData);
            }
            RaisePropertyChanged("AudioData");
        }

        public byte[] GetAudioData(AudioScramblingMode audioScramblingMode)
        {
            if (audioScramblingMode == AudioScramblingMode.Default)
                audioScramblingMode = (_isMpegOrAac ? AudioScramblingMode.Unsynchronization : AudioScramblingMode.Scrambling);

            switch (audioScramblingMode)
            {
                case AudioScramblingMode.Scrambling:
                    return Scramble(_audioData);

                case AudioScramblingMode.Unsynchronization:
                    return ID3v2Utils.ReadUnsynchronized(_audioData);

                default:
                    if (_audioData == null)
                        return null;
                    else
                        return (byte[])_audioData.Clone();
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "ATXT";
                case ID3v2TagVersion.ID3v22:
                    return null;
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            _frameHeader.Read(tagReadingInfo, ref stream);
            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;

            if (bytesLeft > 0)
            {
                TextEncoding = (EncodingType)stream.Read1(ref bytesLeft);
                if (bytesLeft > 0)
                {
                    MimeType = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);
                    if (bytesLeft > 1)
                    {
                        byte flags = stream.Read1(ref bytesLeft);
                        _isMpegOrAac = ((flags & 0x01) == 0x00);
                        EquivalentText = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);
                        if (bytesLeft > 0)
                        {
                            _audioData = stream.Read(bytesLeft);
                            bytesLeft = 0;
                        }
                    }
                    else
                    {
                        EquivalentText = null;
                        _audioData = null;
                    }
                }
                else
                {
                    MimeType = null;
                    EquivalentText = null;
                    _audioData = null;
                }
            }
            else
            {
                TextEncoding = EncodingType.ISO88591;
                MimeType = null;
                EquivalentText = null;
                _audioData = null;
            }

            if (bytesLeft > 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (_audioData == null || _audioData.Length == 0)
                return new byte[0];

            string frameID = GetFrameID(tagVersion);
            if (frameID == null) 
                return new byte[0];

            using (MemoryStream frameData = new MemoryStream())
            {
                byte[] mimeType = ID3v2Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, MimeType, true);
                
                byte[] equivText;
                do
                {
                    equivText = ID3v2Utils.GetStringBytes(tagVersion, TextEncoding, EquivalentText, true);
                } while (this.RequiresFix(tagVersion, EquivalentText, equivText));

                frameData.WriteByte((byte)TextEncoding);
                frameData.Write(mimeType, 0, mimeType.Length);
                frameData.WriteByte((byte)(_isMpegOrAac ? 0 : 1));
                frameData.Write(equivText, 0, equivText.Length);
                frameData.Write(_audioData, 0, _audioData.Length);

                return _frameHeader.GetBytes(frameData, tagVersion, frameID);
            }
        }
    }
}

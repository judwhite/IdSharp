using System;
using System.IO;
using System.Text;
using IdSharp.Common.Utils;

namespace IdSharp.AudioInfo
{
    /// <summary>
    /// Monkey's Audio
    /// </summary>
    public class MonkeysAudio : IAudioFile
    {
        private const int COMPRESSION_LEVEL_EXTRA_HIGH = 4000;
        private static readonly byte[] MAC_IDENTIFIER = Encoding.ASCII.GetBytes("MAC ");

        private readonly int _frequency;
        private readonly int _frames;
        private readonly decimal _totalSeconds;
        private readonly decimal _bitrate;
        private readonly int _channels;
        private readonly int _compressionLevel;
        private readonly int _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonkeysAudio"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public MonkeysAudio(string path)
        {
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // Skip ID3v2 tag
                int tmpID3v2TagSize = ID3v2.GetTagSize(stream);
                int tmpID3v1TagSize = ID3v1.GetTagSize(stream);
                int tmpAPEv2TagSize = APEv2.GetTagSize(stream);
                stream.Seek(tmpID3v2TagSize, SeekOrigin.Begin);

                byte[] identifier = stream.Read(4);
                if (ByteUtils.Compare(identifier, MAC_IDENTIFIER, 4) == false)
                {
                    throw new InvalidDataException("Invalid Monkey's Audio file");
                }

                byte[] buf = stream.Read(4);

                _version = buf[0] + (buf[1] << 8);
                int blocksPerFrame;
                int finalBlocks;

                if (_version >= 3980 && _version <= 3990)
                {
                    buf = stream.Read(4);
                    int descriptorLength = buf[0] + (buf[1] << 8) + (buf[2] << 16) + (buf[3] << 24);
                    stream.Seek(descriptorLength - 12, SeekOrigin.Current); // skip DESCRIPTOR

                    buf = stream.Read(4);
                    _compressionLevel = buf[0] + (buf[1] << 8);

                    blocksPerFrame = stream.ReadInt32LittleEndian();
                    finalBlocks = stream.ReadInt32LittleEndian();
                    _frames = stream.ReadInt32LittleEndian();

                    buf = stream.Read(4);
                    // skip bits per sample
                    _channels = buf[2] + (buf[3] << 8);

                    _frequency = stream.ReadInt32LittleEndian();
                }
                else if (_version <= 3970)
                {
                    // TODO: This section needs work

                    _compressionLevel = buf[2] + (buf[3] << 8);

                    buf = stream.Read(24);

                    // skip format flags
                    _channels = buf[2] + (buf[3] << 8);

                    _frequency = buf[4] + (buf[5] << 8) + (buf[6] << 16) + (buf[7] << 32);

                    if (_version >= 3950)
                        blocksPerFrame = 73728 * 4;
                    else if (_version >= 3900 || (_version >= 3800 && _compressionLevel == COMPRESSION_LEVEL_EXTRA_HIGH))
                        blocksPerFrame = 73728;
                    else
                        blocksPerFrame = 9216;

                    // TODO: This is definitely fucked up
                    finalBlocks = buf[0] + (buf[1] << 8) + (buf[2] << 16) + (buf[3] << 24);
                    _frames = buf[0] + (buf[1] << 8) + (buf[2] << 16) + (buf[3] << 24);
                }
                else
                {
                    throw new NotImplementedException(string.Format("MAC {0:0.00} not supported", _version / 1000.0));
                }

                long totalBlocks = ((_frames - 1) * blocksPerFrame) + finalBlocks;

                long totalSize = stream.Length - stream.Position - tmpAPEv2TagSize - tmpID3v1TagSize;

                _totalSeconds = totalBlocks / (decimal)_frequency;
                _bitrate = totalSize / (_totalSeconds * 125.0m);
            }
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get { return _frequency; }
        }

        /// <summary>
        /// Gets the frame count.
        /// </summary>
        /// <value>The frame count.</value>
        public int Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// Gets the total seconds.
        /// </summary>
        /// <value>The total seconds.</value>
        public decimal TotalSeconds
        {
            get { return _totalSeconds; }
        }

        /// <summary>
        /// Gets the bitrate.
        /// </summary>
        /// <value>The bitrate.</value>
        public decimal Bitrate
        {
            get { return _bitrate; }
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
            get { return AudioFileType.MonkeysAudio; }
        }

        /// <summary>
        /// Gets the compression level.
        /// </summary>
        /// <value>The compression level.</value>
        public int CompressionLevel
        {
            get { return _compressionLevel; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public int Version
        {
            get { return _version; }
        }
    }
}

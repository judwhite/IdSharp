using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.AudioInfo
{
    /// <summary>
    /// RIFF WAVE
    /// </summary>
    public class RiffWave : IAudioFile
    {
        private int _frequency;
        private int _channels;
        private decimal _totalSeconds;
        private decimal _bitrate;
        private long _audioDataOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="RiffWave"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public RiffWave(Stream stream)
        {
            ReadStream(stream);
        }

        private void ReadStream(Stream stream)
        {
            byte[] header = stream.Read(12);
			if (header[0] != 'R' || header[1] != 'I' || header[2] != 'F' || header[3] != 'F')
            {
                throw new InvalidDataException("'RIFF' identifier not found");
            }
            // Note: bytes 4 thru 7 contain the file size - 8 bytes
			if (header[8] != 'W' || header[9] != 'A' || header[10] != 'V' || header[11] != 'E')
            {
                throw new InvalidDataException("'WAVE' identifier not found");
            }

			bool fmtBlockFound = false;
			int dataSize;
			while (true)
			{
				byte[] identifierHeader = stream.Read(4);
				dataSize = stream.ReadInt32LittleEndian();

				// the data block starts WAV data
				if (identifierHeader[0] == 'd' && identifierHeader[1] == 'a' && identifierHeader[2] == 't' && identifierHeader[3] == 'a')
					break;

				byte[] data = stream.Read(dataSize);

				if (identifierHeader[0] == 'f' && identifierHeader[1] == 'm' && identifierHeader[2] == 't' && identifierHeader[3] == ' ')
				{
					fmtBlockFound = true;

					//int extraBytes = hdr[16] + (hdr[17] << 8) + (hdr[18] << 16) + (hdr[19] << 24) - 16;

					// start at 20
					int compression = data[0] + (data[1] << 8);
					// Type 1 is PCM/Uncompressed
					if (compression != 1)
						throw new NotSupportedException("Only PCM/Uncompressed is supported");

					_channels = data[2] + (data[3] << 8);
					// Only mono or stereo PCM is supported in this example
					if (_channels < 1 || _channels > 2)
						throw new NotSupportedException("Only mono or stereo PCM is supported");

					// Samples per second, independent of number of channels
					_frequency = data[4] + (data[5] << 8) + (data[6] << 16) + (data[7] << 24);
					// Bytes 8-11 contain the "average bytes per second", unneeded here
					// Bytes 12-13 contain the number of bytes per sample (includes channels)
					// Bytes 14-15 contain the number of bits per single sample
					int bits = data[14] + (data[15] << 8);
					// Supporting othe sample depths will require conversion
					if (bits != 16)
						throw new InvalidDataException(string.Format("Only 16-bit audio is supported (bits={0})", bits));

					// Skip past extra bytes, if any
					//if (extraBytes != 0)
					//	stream.Seek(extraBytes, SeekOrigin.Current);
				}
			}

			if (!fmtBlockFound)
				throw new InvalidDataException("'fmt ' identifier not found");

        	// Start reading the next frame.  Only supported frame is the data block
            //byte[] b = stream.Read(8);

            // Do we have a fact block?
            /*if (b[0] == 'f' && b[1] == 'a' && b[2] == 'c' && b[3] == 't')
            {
                // TODO: problem here - code just rewinds and rereads
                // Skip the fact block
                stream.Seek(36 + extraBytes, SeekOrigin.Begin);
                // Read the next frame
                b = stream.Read(8);
            }*/

			int bytes = dataSize;

            _audioDataOffset = stream.Position;
            _totalSeconds = bytes / (_channels * 2.0m * _frequency);
            _bitrate = bytes / _totalSeconds / 125.0m;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RiffWave"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public RiffWave(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ReadStream(fs);
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
            get { return AudioFileType.RiffWave; }
        }

        /// <summary>
        /// Gets the audio data offset.
        /// </summary>
        /// <value>The audio data offset.</value>
        public long AudioDataOffset
        {
            get { return _audioDataOffset; }
        }
    }
}

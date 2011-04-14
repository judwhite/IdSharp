using System;
using System.IO;

namespace IdSharp.AudioInfo
{
    /// <summary>
    /// OptimFROG
    /// </summary>
    public class OptimFrog : IAudioFile
    {
        private readonly int _channels;
        private readonly int _frequency;
        private readonly decimal _totalSeconds;
        private readonly decimal _bitrate;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimFrog"/> class.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public OptimFrog(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Position = 31;
                RiffWave wav = new RiffWave(fs);

                _channels = wav.Channels;
                _frequency = wav.Frequency;
                _totalSeconds = wav.TotalSeconds;
                _bitrate = (fs.Length / 125.0m) / _totalSeconds;
            }
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
        /// Gets the total seconds.
        /// </summary>
        /// <value>The total seconds.</value>
        public decimal TotalSeconds
        {
            get { return _totalSeconds; }
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
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get { return _frequency; }
        }

        /// <summary>
        /// Gets the type of the audio file.
        /// </summary>
        /// <value>The type of the audio file.</value>
        public AudioFileType FileType
        {
            get { return AudioFileType.OptimFrog; }
        }
    }
}

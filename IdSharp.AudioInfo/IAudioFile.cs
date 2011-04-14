namespace IdSharp.AudioInfo
{
    /// <summary>
    /// IAudioFile interface
    /// </summary>
    public interface IAudioFile
    {
        /// <summary>
        /// Gets the bitrate.
        /// </summary>
        /// <value>The bitrate.</value>
        decimal Bitrate { get; }
        
        /// <summary>
        /// Gets the total seconds.
        /// </summary>
        /// <value>The total seconds.</value>
        decimal TotalSeconds { get; }
        
        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        /// <value>The number of channels.</value>
        int Channels { get; }
        
        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        int Frequency { get; }

        /// <summary>
        /// Gets the type of the audio file.
        /// </summary>
        /// <value>The type of the audio file.</value>
        AudioFileType FileType { get; }
    }
}

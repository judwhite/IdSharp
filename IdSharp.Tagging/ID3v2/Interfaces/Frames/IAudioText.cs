namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Audio text frame.  This frame carries a short audio clip which represents the information carried by another ID3v2 frame that is present in the same tag.
    /// </summary>
    public interface IAudioText : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the MIME type of the audio data.
        /// </summary>
        /// <value>The MIME type of the audio data.</value>
        string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the text the audio data represents.  This text should be present in another frame in the ID3v2 tag.
        /// </summary>
        /// <value>The text the audio data represents.</value>
        string EquivalentText { get; set; }

        /// <summary>
        /// Sets the audio data.
        /// </summary>
        /// <param name="mimeType">The MIME type of the audio data.</param>
        /// <param name="audioData">The audio data.  Do not alter the audio data with the scrambling or unsynchronization technique, these will be applied internally.</param>
        /// <param name="isMpegOrAac">if set to <c>true</c> then the audio is MPEG or AAC.</param>
        void SetAudioData(string mimeType, byte[] audioData, bool isMpegOrAac);

        /// <summary>
        /// Gets the audio data.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <param name="audioScramblingMode">The audio scrambling mode.</param>
        /// <returns>A copy of the audio data.</returns>
        byte[] GetAudioData(AudioScramblingMode audioScramblingMode);
    }
}

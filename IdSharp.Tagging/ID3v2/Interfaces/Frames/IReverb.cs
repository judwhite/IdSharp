namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Reverb</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>Yet another subjective one. You may here adjust echoes of different
    ///    kinds. Reverb left/right is the delay between every bounce in ms.
    ///    Reverb bounces left/right is the number of bounces that should be
    ///    made. $FF equals an infinite number of bounces. Feedback is the
    ///    amount of volume that should be returned to the next echo bounce. $00
    ///    is 0%, $FF is 100%. If this value were $7F, there would be 50% volume
    ///    reduction on the first bounce, 50% of that on the second and so on.
    ///    Left to left means the sound from the left bounce to be played in the
    ///    left speaker, while left to right means sound from the left bounce to
    ///    be played in the right speaker.</para>
    /// 
    ///    <para>'Premix left to right' is the amount of left sound to be mixed in the
    ///    right before any reverb is applied, where $00 is 0% and $FF is 100%.
    ///    'Premix right to left' does the same thing, but right to left.
    ///    Setting both premix to $FF would result in a mono output (if the
    ///    reverb is applied symmetric).</para>
    /// 
    ///    <para>There may only be one "RVRB" frame in each tag.</para>
    /// 
    ///      <para>[Header for 'Reverb', ID: "RVRB"]<br />
    ///      Reverb left (ms)                 $xx xx<br />
    ///      Reverb right (ms)                $xx xx<br />
    ///      Reverb bounces, left             $xx<br />
    ///      Reverb bounces, right            $xx<br />
    ///      Reverb feedback, left to left    $xx<br />
    ///      Reverb feedback, left to right   $xx<br />
    ///      Reverb feedback, right to right  $xx<br />
    ///      Reverb feedback, right to left   $xx<br />
    ///      Premix left to right             $xx<br />
    ///      Premix right to left             $xx</para>
    /// </summary>
    public interface IReverb : IFrame
    {
        /// <summary>
        /// Gets or sets the delay between each bounce in milliseconds on the left channel.
        /// </summary>
        /// <value>The delay between each bounce in milliseconds on the left channel.</value>
        short ReverbLeftMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the delay between each bounce in milliseconds on the right channel.
        /// </summary>
        /// <value>The delay between each bounce in milliseconds on the right channel.</value>
        short ReverbRightMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the number of bounces on the left channel.  0xFF represents an infinite number of bounces.
        /// </summary>
        /// <value>The number of bounces on the left channel.</value>
        byte ReverbBouncesLeft { get; set; }

        /// <summary>
        /// Gets or sets the number of bounces on the right channel.  0xFF represents an infinite number of bounces.
        /// </summary>
        /// <value>The number of bounces on the right channel.</value>
        byte ReverbBouncesRight { get; set; }

        /// <summary>
        /// Gets or sets the reverb feedback left to left.  Represents the amount of volume that should be returned on 
        /// each echo bounce.  For example, 0xFF represents 100%, 0x7F represents 50%, 0x00 represents 0%.
        /// </summary>
        /// <value>The reverb feedback left to left.</value>
        byte ReverbFeedbackLeftToLeft { get; set; }

        /// <summary>
        /// Gets or sets the reverb feedback left to right.  Represents the amount of volume that should be returned on 
        /// each echo bounce.  For example, 0xFF represents 100%, 0x7F represents 50%, 0x00 represents 0%.
        /// </summary>
        /// <value>The reverb feedback left to right.</value>
        byte ReverbFeedbackLeftToRight { get; set; }

        /// <summary>
        /// Gets or sets the reverb feedback right to right.  Represents the amount of volume that should be returned on 
        /// each echo bounce.  For example, 0xFF represents 100%, 0x7F represents 50%, 0x00 represents 0%.
        /// </summary>
        /// <value>The reverb feedback right to right.</value>
        byte ReverbFeedbackRightToRight { get; set; }

        /// <summary>
        /// Gets or sets the reverb feedback right to left.  Represents the amount of volume that should be returned on 
        /// each echo bounce.  For example, 0xFF represents 100%, 0x7F represents 50%, 0x00 represents 0%.
        /// </summary>
        /// <value>The reverb feedback right to left.</value>
        byte ReverbFeedbackRightToLeft { get; set; }

        /// <summary>
        /// Gets or sets the amount of left sound to be mixed in the right before any reverb is applied, where 
        /// 0x00 is 0% and 0xFF is 100%.
        /// </summary>
        /// <value>The premix left to right.</value>
        byte PremixLeftToRight { get; set; }

        /// <summary>
        /// Gets or sets the amount of right sound to be mixed in the left before any reverb is applied, where 
        /// 0x00 is 0% and 0xFF is 100%.
        /// </summary>
        /// <value>The premix left to right.</value>
        byte PremixRightToLeft { get; set; }
    }
}

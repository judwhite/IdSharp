namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Popularimeter</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>The purpose of this frame is to specify how good an audio file is.
    ///    Many interesting applications could be found to this frame such as a
    ///    playlist that features better audiofiles more often than others or it
    ///    could be used to profile a person's taste and find other 'good' files
    ///    by comparing people's profiles. The frame is very simple. It contains
    ///    the email address to the user, one rating byte and a four byte play
    ///    counter, intended to be increased with one for every time the file is
    ///    played. The email is a terminated string. The rating is 1-255 where
    ///    1 is worst and 255 is best. 0 is unknown. If no personal counter is
    ///    wanted it may be omitted.  When the counter reaches all one's, one
    ///    byte is inserted in front of the counter thus making the counter
    ///    eight bits bigger in the same away as the play counter ("PCNT").
    ///    There may be more than one "POPM" frame in each tag, but only one
    ///    with the same email address.</para>
    /// 
    ///      <para>[Header for 'Popularimeter', ID: "POPM"]<br />
    ///      Email to user   [text string] $00<br />
    ///      Rating          $xx<br />
    ///      Counter         $xx xx xx xx (xx ...)</para>
    /// </summary>
    public interface IPopularimeter : IFrame
    {
        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        /// <value>The user's email.</value>
        string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the rating.  The rating is 1-255 where 1 is worst and 255 is best.  0 is unknown.
        /// </summary>
        /// <value>The rating.</value>
        byte Rating { get; set; }

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        /// <value>The play count.</value>
        long PlayCount { get; set; }
    }
}

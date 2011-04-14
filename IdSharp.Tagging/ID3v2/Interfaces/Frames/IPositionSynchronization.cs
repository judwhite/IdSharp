namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Position synchronization frame</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>
    ///    This frame delivers information to the listener of how far into the
    ///    audio stream he picked up; in effect, it states the time offset of
    ///    the first frame in the stream. The frame layout is:</para>
    /// 
    ///      <para>[Head for 'Position synchronization', ID: "POSS"]<br />
    ///      Time stamp format         $xx<br />
    ///      Position                  $xx (xx ...)</para>
    /// 
    ///    <para>Where time stamp format is:</para>
    /// 
    ///      <para>$01  Absolute time, 32 bit sized, using MPEG frames as unit
    ///      $02  Absolute time, 32 bit sized, using milliseconds as unit</para>
    /// 
    ///    <para>and position is where in the audio the listener starts to receive,
    ///    i.e. the beginning of the next frame. If this frame is used in the
    ///    beginning of a file the value is always 0. There may only be one
    ///    "POSS" frame in each tag.</para>
    /// </summary>
    public interface IPositionSynchronization : IFrame
    {
        /// <summary>
        /// Gets or sets the time stamp format.
        /// </summary>
        /// <value>The time stamp format.</value>
        TimestampFormat TimestampFormat { get; set; }

        /// <summary>
        /// Gets or sets the position in milliseconds or frames.  See <see cref="TimestampFormat "/>.
        /// </summary>
        /// <value>The position in milliseconds or frames.  See <see cref="TimestampFormat"/>.</value>
        int Position { get; set; }
    }
}

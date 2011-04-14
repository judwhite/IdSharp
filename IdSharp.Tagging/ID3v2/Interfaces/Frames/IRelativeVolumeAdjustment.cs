namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Relative volume adjustment</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>This is a more subjective function than the previous ones. It allows
    ///    the user to say how much he wants to increase/decrease the volume on
    ///    each channel while the file is played. The purpose is to be able to
    ///    align all files to a reference volume, so that you don't have to
    ///    change the volume constantly. This frame may also be used to balance
    ///    adjust the audio. If the volume peak levels are known then this could
    ///    be described with the 'Peak volume right' and 'Peak volume left'
    ///    field. If peak volume is not known these fields could be left zeroed
    ///    or, if no other data follows, be completely omitted. There may only
    ///    be one "RVAD" frame in each tag.</para>
    /// 
    ///      <para>[Header for 'Relative volume adjustment', ID: "RVAD"]<br />
    ///      Increment/decrement           %00xxxxxx<br />
    ///      Bits used for volume descr.   $xx<br />
    ///      Relative volume change, right $xx xx (xx ...)<br />
    ///      Relative volume change, left  $xx xx (xx ...)<br />
    ///      Peak volume right             $xx xx (xx ...)<br />
    ///      Peak volume left              $xx xx (xx ...)</para>
    /// 
    ///    <para>In the increment/decrement field bit 0 is used to indicate the right
    ///    channel and bit 1 is used to indicate the left channel. 1 is
    ///    increment and 0 is decrement.</para>
    /// 
    ///    <para>The 'bits used for volume description' field is normally $10 (16
    ///    bits) for MPEG 2 layer I, II and III [MPEG] and MPEG 2.5. This value
    ///    may not be $00. The volume is always represented with whole bytes,
    ///    padded in the beginning (highest bits) when 'bits used for volume
    ///    description' is not a multiple of eight.</para>
    /// 
    ///    <para>This datablock is then optionally followed by a volume definition for
    ///    the left and right back channels. If this information is appended to
    ///    the frame the first two channels will be treated as front channels.
    ///    In the increment/decrement field bit 2 is used to indicate the right
    ///    back channel and bit 3 for the left back channel.</para>
    /// 
    ///      <para>Relative volume change, right back $xx xx (xx ...)<br />
    ///      Relative volume change, left back  $xx xx (xx ...)<br />
    ///      Peak volume right back             $xx xx (xx ...)<br />
    ///      Peak volume left back              $xx xx (xx ...)</para>
    /// 
    ///    <para>If the center channel adjustment is present the following is appended
    ///    to the existing frame, after the left and right back channels. The
    ///    center channel is represented by bit 4 in the increase/decrease
    ///    field.</para>
    /// 
    ///      <para>Relative volume change, center  $xx xx (xx ...)<br />
    ///      Peak volume center              $xx xx (xx ...)</para>
    /// 
    ///    <para>If the bass channel adjustment is present the following is appended
    ///    to the existing frame, after the center channel. The bass channel is
    ///    represented by bit 5 in the increase/decrease field.</para>
    /// 
    ///      <para>Relative volume change, bass  $xx xx (xx ...)<br />
    ///      Peak volume bass              $xx xx (xx ...)</para>
    /// </summary>
    public interface IRelativeVolumeAdjustment : IFrame
    {
        /// <summary>
        /// Gets or sets the identification string.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The identification string.  Only supported in ID3v2.4.</value>
        string Identification { get; set; }

        #region <<< Not used >>>
        // <summary>
        // Gets or sets the bits used for volume description.  Typically 16 bits.
        // </summary>
        // <value>The bits used for volume description.</value>
        //UInt16 BitsUsedForVolumeDescription { get; set; }
        #endregion <<< Not used >>>

        #region <<< Volume adjustment direction >>>
        /*
        /// <summary>
        /// Gets or sets the front right channel volume adjustment direction.
        /// </summary>
        /// <value>The front right channel volume adjustment direction.</value>
        VolumeAdjustmentDirection FrontRightDirection { get; set; }

        /// <summary>
        /// Gets or sets the front left channel volume adjustment direction.
        /// </summary>
        /// <value>The front left channel volume adjustment direction.</value>
        VolumeAdjustmentDirection FrontLeftDirection { get; set; }

        /// <summary>
        /// Gets or sets the right back channel volume adjustment direction.
        /// </summary>
        /// <value>The right back channel volume adjustment direction.</value>
        VolumeAdjustmentDirection BackRightDirection { get; set; }

        /// <summary>
        /// Gets or sets the back left channel volume adjustment direction.
        /// </summary>
        /// <value>The back left channel volume adjustment direction.</value>
        VolumeAdjustmentDirection BackLeftDirection { get; set; }

        /// <summary>
        /// Gets or sets the front center channel volume adjustment direction.
        /// </summary>
        /// <value>The front center channel volume adjustment direction.</value>
        VolumeAdjustmentDirection FrontCenterDirection { get; set; }

        /// <summary>
        /// Gets or sets the subwoofer/bass volume adjustment direction.
        /// </summary>
        /// <value>The subwoofer/bass volume adjustment direction.</value>
        VolumeAdjustmentDirection SubwooferDirection { get; set; }

        /// <summary>
        /// Gets or sets the back center channel volume adjustment direction.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The back center channel volume adjustment direction.  Only supported in ID3v2.4.</value>
        VolumeAdjustmentDirection BackCenterDirection { get; set; }

        /// <summary>
        /// Gets or sets the 'other' volume adjustment direction.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The 'other' volume adjustment direction.  Only supported in ID3v2.4.</value>
        VolumeAdjustmentDirection OtherDirection { get; set; }

        /// <summary>
        /// Gets or sets the master volume adjustment direction.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The master volume adjustment direction.  Only supported in ID3v2.4.</value>
        VolumeAdjustmentDirection MasterDirection { get; set; }*/

        #endregion <<< Volume adjustment direction >>>

        #region <<< Volume adjustments >>>

        /// <summary>
        /// Gets or sets the relative volume change in the front right channel.
        /// </summary>
        /// <value>The relative volume change in the front right channel.</value>
        decimal FrontRightAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the relative volume change in the front left channel.
        /// </summary>
        /// <value>The relative volume change in the front left channel.</value>
        decimal FrontLeftAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the relative volume change in the back right channel.
        /// </summary>
        /// <value>The relative volume change in the back right channel.</value>
        decimal BackRightAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the relative volume change in the back left channel.
        /// </summary>
        /// <value>The relative volume change in the back left channel.</value>
        decimal BackLeftAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the relative volume change in the front center channel.
        /// </summary>
        /// <value>The relative volume change in the front center channel.</value>
        decimal FrontCenterAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the relative volume change in the subwoofer/bass.
        /// </summary>
        /// <value>The relative volume change in the subwoofer/bass.</value>
        decimal SubwooferAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the relative volume change in the back center channel.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The relative volume change in the back center channel.  Only supported in ID3v2.4.</value>
        decimal BackCenterAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the 'other' relative volume change.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The 'other' relative volume change.  Only supported in ID3v2.4.</value>
        decimal OtherAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the master relative volume change.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The master relative volume change.  Only supported in ID3v2.4.</value>
        decimal MasterAdjustment { get; set; }

        #endregion <<< Volume adjustments >>>

        #region <<< Peak volume >>>

        /// <summary>
        /// Gets or sets the peak volume in the front right channel.
        /// </summary>
        /// <value>The peak volume in the front right channel.</value>
        decimal FrontRightPeak { get; set; }

        /// <summary>
        /// Gets or sets the peak volume in the front left channel.
        /// </summary>
        /// <value>The peak volume in the front left channel.</value>
        decimal FrontLeftPeak { get; set; }

        /// <summary>
        /// Gets or sets the peak volume in the back right channel.
        /// </summary>
        /// <value>The peak volume in the back right channel.</value>
        decimal BackRightPeak { get; set; }

        /// <summary>
        /// Gets or sets the peak volume in the back left channel.
        /// </summary>
        /// <value>The peak volume in the back left channel.</value>
        decimal BackLeftPeak { get; set; }

        /// <summary>
        /// Gets or sets the peak volume in the front center channel.
        /// </summary>
        /// <value>The peak volume in the front center channel.</value>
        decimal FrontCenterPeak { get; set; }

        /// <summary>
        /// Gets or sets the peak volume in the subwoofer/bass.
        /// </summary>
        /// <value>The peak volume in the subwoofer/bass.</value>
        decimal SubwooferPeak { get; set; }

        /// <summary>
        /// Gets or sets the peak volume in the back center channel.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The peak volume in the back center channel.  Only supported in ID3v2.4.</value>
        decimal BackCenterPeak { get; set; }

        /// <summary>
        /// Gets or sets the 'other' peak volume.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The 'other' peak volume.  Only supported in ID3v2.4.</value>
        decimal OtherPeak { get; set; }

        /// <summary>
        /// Gets or sets the master peak volume.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The master peak volume.  Only supported in ID3v2.4.</value>
        decimal MasterPeak { get; set; }

        #endregion <<< Peak volume >>>
    }
}

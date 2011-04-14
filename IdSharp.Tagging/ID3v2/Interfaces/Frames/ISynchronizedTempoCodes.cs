using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Synchronized tempo codes</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>For a more accurate description of the tempo of a musical piece this
    ///    frame might be used. After the header follows one byte describing
    ///    which time stamp format should be used. Then follows one or more
    ///    tempo codes. Each tempo code consists of one tempo part and one time
    ///    part. The tempo is in BPM described with one or two bytes. If the
    ///    first byte has the value $FF, one more byte follows, which is added
    ///    to the first giving a range from 2 - 510 BPM, since $00 and $01 is
    ///    reserved. $00 is used to describe a beat-free time period, which is
    ///    not the same as a music-free time period. $01 is used to indicate one
    ///    single beat-stroke followed by a beat-free period.</para>
    /// 
    ///    <para>The tempo descriptor is followed by a time stamp. Every time the
    ///    tempo in the music changes, a tempo descriptor may indicate this for
    ///    the player. All tempo descriptors should be sorted in chronological
    ///    order. The first beat-stroke in a time-period is at the same time as
    ///    the beat description occurs. There may only be one "SYTC" frame in
    ///    each tag.</para>
    /// 
    ///      <para>[Header for 'synchronized tempo codes', ID: "SYTC"]<br />
    ///      Time stamp format   $xx<br />
    ///      Tempo data          [binary data]</para>
    /// 
    ///    <para>Where time stamp format is:</para>
    /// 
    ///      <para>$01  Absolute time, 32 bit sized, using MPEG frames as unit<br />
    ///      $02  Absolute time, 32 bit sized, using milliseconds as unit</para>
    /// 
    ///    <para>Abolute time means that every stamp contains the time from the
    ///    beginning of the file.</para>
    /// </summary>
    public interface ISynchronizedTempoCodes : IFrame
    {
        /// <summary>
        /// Gets or sets the time stamp format.
        /// </summary>
        /// <value>The time stamp format.</value>
        TimestampFormat TimestampFormat { get; set; }

        /// <summary>
        /// Gets the BindingList of tempo data items.
        /// </summary>
        /// <value>The BindingList of tempo data items.</value>
        BindingList<ITempoData> Items { get; }
    }
}

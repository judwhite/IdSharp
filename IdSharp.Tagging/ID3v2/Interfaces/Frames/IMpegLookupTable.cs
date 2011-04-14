using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>MPEG location lookup table</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>
    ///    To increase performance and accuracy of jumps within a MPEG [MPEG]
    ///    audio file, frames with time codes in different locations in the file
    ///    might be useful. This ID3v2 frame includes references that the
    ///    software can use to calculate positions in the file. After the frame
    ///    header follows a descriptor of how much the 'frame counter' should be
    ///    increased for every reference. If this value is two then the first
    ///    reference points out the second frame, the 2nd reference the 4th
    ///    frame, the 3rd reference the 6th frame etc. In a similar way the
    ///    'bytes between reference' and 'milliseconds between reference' points
    ///    out bytes and milliseconds respectively.
    /// </para><para>
    ///    Each reference consists of two parts; a certain number of bits, as
    ///    defined in 'bits for bytes deviation', that describes the difference
    ///    between what is said in 'bytes between reference' and the reality and
    ///    a certain number of bits, as defined in 'bits for milliseconds
    ///    deviation', that describes the difference between what is said in
    ///    'milliseconds between reference' and the reality. The number of bits
    ///    in every reference, i.e. 'bits for bytes deviation'+'bits for
    ///    milliseconds deviation', must be a multiple of four. There may only
    ///    be one "MLLT" frame in each tag.
    /// </para>
    ///      <para>[Header for 'Location lookup table', ID: "MLLT"]<br />
    ///      MPEG frames between reference  $xx xx<br />
    ///      Bytes between reference        $xx xx xx<br />
    ///      Milliseconds between reference $xx xx xx<br />
    ///      Bits for bytes deviation       $xx<br />
    ///      Bits for milliseconds dev.     $xx</para>
    /// <para>
    ///    Then for every reference the following data is included;
    /// </para>
    ///      <para>Deviation in bytes         %xxx....<br />
    ///      Deviation in milliseconds  %xxx....</para>
    /// </summary>
    public interface IMpegLookupTable : IFrame
    {
        /// <summary>
        /// Gets or sets the frames between references.  The maximum value is 65535.
        /// </summary>
        /// <value>The frames between references.</value>
        int FramesBetweenReference { get; set; }

        /// <summary>
        /// Gets or sets the bytes between references.  See <see cref="Items"/> for more information.  The maximum value is 0xFFFFFF.
        /// </summary>
        /// <value>The bytes between references.</value>
        int BytesBetweenReference { get; set; }

        /// <summary>
        /// Gets or sets the milliseconds between references.  See <see cref="Items"/> for more information.  The maximum value is 0xFFFFFF.
        /// </summary>
        /// <value>The milliseconds between references.</value>
        int MillisecondsBetweenReference { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        BindingList<IMpegLookupTableItem> Items { get; }
    }
}

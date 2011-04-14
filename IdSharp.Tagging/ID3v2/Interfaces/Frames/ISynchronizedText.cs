using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Synchronized lyrics/text</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>This is another way of incorporating the words, said or sung lyrics,
    ///    in the audio file as text, this time, however, in sync with the
    ///    audio. It might also be used to describing events e.g. occurring on a
    ///    stage or on the screen in sync with the audio. The header includes a
    ///    content descriptor, represented with as terminated textstring. If no
    ///    descriptor is entered, 'Content descriptor' is $00 (00) only.</para>
    /// 
    ///      <para>[Header for 'synchronized lyrics/text', ID: "SYLT"]<br />
    ///      Text encoding        $xx<br />
    ///      Language             $xx xx xx<br />
    ///      Time stamp format    $xx<br />
    ///      Content type         $xx<br />
    ///      Content descriptor   [text string according to encoding] $00 (00)</para>
    /// 
    ///    <para>Time stamp format is:</para>
    /// 
    ///      <para>$01  Absolute time, 32 bit sized, using MPEG [MPEG] frames as unit<br />
    ///      $02  Absolute time, 32 bit sized, using milliseconds as unit</para>
    /// 
    ///    <para>Absolute time means that every stamp contains the time from the
    ///    beginning of the file.</para>
    /// 
    ///    <para>The text that follows the frame header differs from that of the
    ///    unsynchronized lyrics/text transcription in one major way. Each
    ///    syllable (or whatever size of text is considered to be convenient by
    ///    the encoder) is a null terminated string followed by a time stamp
    ///    denoting where in the sound file it belongs. Each sync thus has the
    ///    following structure:</para>
    /// 
    ///      <para>Terminated text to be synced (typically a syllable)<br />
    ///      Sync identifier (terminator to above string)   $00 (00)<br />
    ///      Time stamp                                     $xx (xx ...)</para>
    /// 
    ///    <para>The 'time stamp' is set to zero or the whole sync is omitted if
    ///    located directly at the beginning of the sound. All time stamps
    ///    should be sorted in chronological order. The sync can be considered
    ///    as a validator of the subsequent string.</para>
    /// 
    ///    <para>Newline ($0A) characters are allowed in all "SYLT" frames and should
    ///    be used after every entry (name, event etc.) in a frame with the
    ///    content type $03 - $04.</para>
    /// 
    ///    <para>A few considerations regarding whitespace characters: Whitespace
    ///    separating words should mark the beginning of a new word, thus
    ///    occurring in front of the first syllable of a new word. This is also
    ///    valid for new line characters. A syllable followed by a comma should
    ///    not be broken apart with a sync (both the syllable and the comma
    ///    should be before the sync).</para>
    /// 
    ///    <para>An example: The "USLT" passage</para>
    /// 
    ///      <para>"Strangers in the night" $0A "Exchanging glances"</para>
    /// 
    ///    <para>would be "SYLT" encoded as:</para>
    /// 
    ///      <para>"Strang" $00 xx xx "ers" $00 xx xx " in" $00 xx xx " the" $00 xx xx
    ///      " night" $00 xx xx 0A "Ex" $00 xx xx "chang" $00 xx xx "ing" $00 xx
    ///      xx "glan" $00 xx xx "ces" $00 xx xx</para>
    /// 
    ///    <para>There may be more than one "SYLT" frame in each tag, but only one
    ///    with the same language and content descriptor.</para>
    /// </summary>
    public interface ISynchronizedText : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the ISO-639-2 language code.
        /// </summary>
        /// <value>The ISO-639-2 language code.</value>
        string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the time stamp format.
        /// </summary>
        /// <value>The time stamp format.</value>
        TimestampFormat TimestampFormat { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        TextContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content descriptor.
        /// </summary>
        /// <value>The content descriptor.</value>
        string ContentDescriptor { get; set; }

        /// <summary>
        /// Gets the BindingList of synchronized text items.
        /// </summary>
        /// <value>The BindingList of synchronized text items.</value>
        BindingList<ISynchronizedTextItem> Items { get; }
    }
}

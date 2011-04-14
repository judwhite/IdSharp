namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Recommended buffer size</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>Sometimes the server from which a audio file is streamed is aware of
    ///    transmission or coding problems resulting in interruptions in the
    ///    audio stream. In these cases, the size of the buffer can be
    ///    recommended by the server using this frame. If the 'embedded info
    ///    flag' is true (1) then this indicates that an ID3 tag with the
    ///    maximum size described in 'Buffer size' may occur in the audiostream.
    ///    In such case the tag should reside between two MPEG frames, if
    ///    the audio is MPEG encoded. If the position of the next tag is known,
    ///    'offset to next tag' may be used. The offset is calculated from the
    ///    end of tag in which this frame resides to the first byte of the
    ///    header in the next. This field may be omitted. Embedded tags are
    ///    generally not recommended since this could render unpredictable
    ///    behaviour from present software/hardware.</para>
    /// 
    ///    <para>For applications like streaming audio it might be an idea to embed
    ///    tags into the audio stream though. If the clients connects to
    ///    individual connections like HTTP and there is a possibility to begin
    ///    every transmission with a tag, then this tag should include a
    ///    'recommended buffer size' frame. If the client is connected to a
    ///    arbitrary point in the stream, such as radio or multicast, then the
    ///    'recommended buffer size' frame should be included in every tag.
    ///    Every tag that is picked up after the initial/first tag is to be
    ///    considered as an update of the previous one. E.g. if there is a
    ///    "TIT2" frame in the first received tag and one in the second tag,
    ///    then the first should be 'replaced' with the second.</para>
    /// 
    ///    <para>The 'Buffer size' should be kept to a minimum. There may only be one
    ///    "RBUF" frame in each tag.</para>
    /// 
    ///      <para>[Header for 'Recommended buffer size', ID: "RBUF"]<br />
    ///      Buffer size               $xx xx xx<br />
    ///      Embedded info flag        %0000000x<br />
    ///      Offset to next tag        $xx xx xx xx</para>
    /// </summary>
    public interface IRecommendedBufferSize : IFrame
    {
        /// <summary>
        /// Gets or sets the recommended size of the buffer.
        /// </summary>
        /// <value>The recommended size of the buffer.</value>
        int BufferSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an ID3 tag with the maximum size described in 'Buffer Size' may occur in the audio stream.
        /// </summary>
        /// <value><c>true</c> if an ID3 tag may occur in the audio stream; otherwise, <c>false</c>.</value>
        bool EmbeddedInfo { get; set; }

        /// <summary>
        /// Gets or sets the offset to the next tag.  The offset is calculated from the
        ///    end of the tag in which this frame resides to the first byte of the
        ///    header in the next.  This field is optional.
        /// </summary>
        /// <value>The offset to the next tag.</value>
        int? OffsetToNextTag { get; set; }
    }
}

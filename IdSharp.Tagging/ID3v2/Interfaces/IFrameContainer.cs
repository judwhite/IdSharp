using System.ComponentModel;
using IdSharp.Common.Events;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// Frame container.
    /// </summary>
    public interface IFrameContainer : INotifyPropertyChanged, INotifyInvalidData
    {
        /// <summary>
        /// Gets the list of unique file identifiers.  See <see cref="IUniqueFileIdentifier"/>.  UFID/UFI.
        /// </summary>
        /// <value>The list of unique file identifiers.  See <see cref="IUniqueFileIdentifier"/>.  UFID/UFI.</value>
        BindingList<IUniqueFileIdentifier> UniqueFileIdentifierList { get; }

        /// <summary>
        /// Gets or sets the album.  The 'Album/Movie/Show title' frame is intended for the title of the
        /// recording/source of sound which the audio in the file is taken from.  TALB/TAL.
        /// </summary>
        /// <value>The album name.  The 'Album/Movie/Show title' frame is intended for the title of the
        /// recording/source of sound which the audio in the file is taken from.  TALB/TAL.</value>
        string Album { get; set; }

        /// <summary>
        /// Gets or sets the number of beats per minute in the mainpart of the audio.  TBPM/TBP.
        /// </summary>
        /// <value>The the number of beats per minute in the mainpart of the audio.  TBPM/TBP.</value>
        string BPM { get; set; }

        /// <summary>
        /// Gets or sets the composer.  TCOM/TCM.
        /// </summary>
        /// <value>The composer.  TCOM/TCM.</value>
        string Composer { get; set; }

        /// <summary>
        /// Gets or sets the genre.  TCON/TCO.
        /// </summary>
        /// <value>The genre.  TCON/TCO.</value>
        string Genre { get; set; }

        /// <summary>
        /// Gets or sets the copyright message, which is a string which must begin with a
        /// year and a space character.  Intended for
        /// the copyright holder of the original sound, not the audio file
        /// itself.  TCOP/TCR.
        /// </summary>
        /// <value>The copyright message.  TCOP/TCR.</value>
        string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the playlist delay in milliseconds.  TDLY/TDY.
        /// </summary>
        /// <value>The playlist delay in milliseconds.  TDLY/TDY.</value>
        int? PlaylistDelayMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the name of the person or organisation that encoded the audio file.  TENC/TEN.
        /// </summary>
        /// <value>The name of the person or organisation that encoded the audio file.  TENC/TEN.</value>
        string EncodedByWho { get; set; }

        /// <summary>
        /// Gets or sets the lyricist.  TEXT/TXT.  TODO: Add list for predefined types.
        /// </summary>
        /// <value>The lyricist.  TEXT/TXT.</value>
        string Lyricist { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.  TFLT/TFT.
        /// </summary>
        /// <value>The type of the file.  TFLT/TFT.</value>
        string FileType { get; set; }

        /// <summary>
        /// Gets or sets the content group.  The 'Content group description' frame 
        /// is used if the sound belongs to
        /// a larger category of sounds/music. For example, classical music is
        /// often sorted in different musical sections (e.g. "Piano Concerto",
        /// "Weather - Hurricane"). TIT1/TT1.
        /// </summary>
        /// <value>The content group.  The 'Content group description' frame 
        /// is used if the sound belongs to
        /// a larger category of sounds/music. For example, classical music is
        /// often sorted in different musical sections (e.g. "Piano Concerto",
        /// "Weather - Hurricane").  TIT1/TT1.</value>
        string ContentGroup { get; set; }

        /// <summary>
        /// Gets or sets the title/song name/content description.  TIT2/TT2.
        /// </summary>
        /// <value>The title/song name/content description.  TIT2/TT2.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the subtitle/description refinement.  TIT3/TT3.
        /// </summary>
        /// <value>The subtitle/description refinement.  TIT3/TT3.</value>
        string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the initial key.  Represented as a string with a maximum length of three
        /// characters.  The ground keys are represented with "A","B","C","D","E",
        /// "F" and "G" and halfkeys represented with "b" and "#". Minor is
        /// represented as "m". Example "Cbm". Off key is represented with an "o"
        /// only.  TKEY/TKE.
        /// </summary>
        /// <value>The initial key.</value>
        string InitialKey { get; set; }

        /// <summary>
        /// Gets the languages of the text or lyrics in the audio file.  TLAN/TLA.
        /// </summary>
        /// <value>The languages of the text or lyrics in the audio file.  TLAN/TLA.</value>
        ILanguageFrame Languages { get; }

        /// <summary>
        /// Gets or sets the length of the audio in milliseconds, as reported by the ID3 tag.  TLEN/TLE.
        /// </summary>
        /// <value>The length of the audio in milliseconds, as reported by the ID3 tag.  TLEN/TLE.</value>
        int? LengthMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the type of the media.  TMED/TMT.  TODO: Dictionary/list of predefined types.
        /// </summary>
        /// <value>The type of the media.  TMED/TMT.</value>
        string MediaType { get; set; }

        /// <summary>
        /// Gets or sets the original album/movie/show/source of sound title.  TOAL/TOT.
        /// </summary>
        /// <value>The original album/movie/show/source of sound title.  TOAL/TOT.</value>
        string OriginalSourceTitle { get; set; }

        /// <summary>
        /// Gets or sets the original filename.  Contains the preferred filename for the
        /// file, since some media doesn't allow the desired length of the
        /// filename. The filename is case sensitive and includes its suffix.  TOFN/TOF.
        /// </summary>
        /// <value>
        /// The original filename.  Contains the preferred filename for the
        /// file, since some media doesn't allow the desired length of the
        /// filename. The filename is case sensitive and includes its suffix.  TOFN/TOF.
        /// </value>
        string OriginalFileName { get; set; }

        /// <summary>
        /// Gets or sets the original lyricist.  TOLY/TOL.
        /// </summary>
        /// <value>The original lyricist.  TOLY/TOL.</value>
        string OriginalLyricist { get; set; }

        /// <summary>
        /// Gets or sets the original artist.  TOPE/TOA.
        /// </summary>
        /// <value>The original artist.  TOPE/TOA.</value>
        string OriginalArtist { get; set; }

        /// <summary>
        /// Gets or sets the original release year.  TDOR/TORY/TOR.  TODO: New implementation in ID3v2.4.
        /// </summary>
        /// <value>The original release year.</value>
        string OriginalReleaseYear { get; set; }

        /// <summary>
        /// Gets or sets the name of the file owner.  TOWN.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>The name of the file owner.</value>
        string FileOwnerName { get; set; }

        /// <summary>
        /// Gets or sets the artist.  TPE1/TP1.
        /// </summary>
        /// <value>The artist.  TPE1/TP1.</value>
        string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album artist/accompaniment.  TPE2/TP2.
        /// </summary>
        /// <value>The album artist/accompaniment.  TPE2/TP2.</value>
        string AlbumArtist { get; set; }

        /// <summary>
        /// Gets or sets the conductor.  TPE3/TP3.
        /// </summary>
        /// <value>The conductor.  TPE3/TP3.</value>
        string Conductor { get; set; }

        /// <summary>
        /// Gets or sets who remixed the audio.  TPE4/TP4.
        /// </summary>
        /// <value>Who remixed the audio.  TPE4/TP4.</value>
        string RemixedBy { get; set; }

        /// <summary>
        /// Gets or sets the disc number.  TPOS/TPA.
        /// </summary>
        /// <value>The disc number.  TPOS/TPA.</value>
        string DiscNumber { get; set; }

        /// <summary>
        /// Gets or sets the publisher/record label.  TPUB/TPB.
        /// </summary>
        /// <value>The publisher/record label.  TPUB/TPB.</value>
        string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the track number.  TRCK/TRK.
        /// </summary>
        /// <value>The track number.  TRCK/TRK.</value>
        string TrackNumber { get; set; }

        /// <summary>
        /// Gets or sets the recording dates.  TRDA/TRD.  TODO: Replaced by TDRC in ID3v2.4.
        /// </summary>
        /// <value>The recording dates.  TRDA/TRD.</value>
        string RecordingDates { get; set; }

        /// <summary>
        /// Gets or sets the name of the internet radio station.  TRSN.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>The name of the internet radio station.  TRSN.  Only valid in ID3v2.3 and higher.</value>
        string InternetRadioStationName { get; set; }

        /// <summary>
        /// Gets or sets the internet radio station owner.  TRSO.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>The internet radio station owner.  TRSO.  Only valid in ID3v2.3 and higher.</value>
        string InternetRadioStationOwner { get; set; }

        /// <summary>
        /// Gets or sets the file size excluding the ID3v2 tag, as reported by the tag.  TSIZ/TSI.  Only valid in ID3v2.3 and ID3v2.2.
        /// </summary>
        /// <value>The file size excluding the ID3v2 tag, as reported by the tag.  TSIZ/TSI.  Only valid in ID3v2.3 and ID3v2.2.</value>
        long? FileSizeExcludingTag { get; set; }

        /// <summary>
        /// Gets or sets the ISRC code.  Value should be 12 characters in length.  TSRC/TRC.
        /// </summary>
        /// <value>The ISRC code.  Value should be 12 characters in length.  TSRC/TRC.</value>
        string ISRC { get; set; }

        /// <summary>
        /// Gets or sets the software/hardware and settings used for encoding.  TSSE/TSS.
        /// </summary>
        /// <value>The encoder settings.  TSSE/TSS.</value>
        string EncoderSettings { get; set; }

        /// <summary>
        /// Gets or sets the year.  TYER in ID3v2.3, TYR in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="ReleaseTimestamp"/> (TDRL).
        /// </summary>
        /// <value>The year.  TYER in ID3v2.3, TYR in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="ReleaseTimestamp"/> (TDRL).</value>
        string Year { get; set; }

        /// <summary>
        /// Gets or sets the date recorded in MMDD format.  TDAT in ID3v2.3, TDA in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).
        /// </summary>
        /// <value>The date recorded in MMDD format.  TDAT in ID3v2.3, TRD in ID3v2.2  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).</value>
        string DateRecorded { get; set; }

        /// <summary>
        /// Gets or sets the time recorded in HHMM format.  TIME in ID3v2.3, TIM in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).
        /// </summary>
        /// <value>The time recorded in HHMM format.  TIME in ID3v2.3, TIM in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).</value>
        string TimeRecorded { get; set; }

        /// <summary>
        /// Gets or sets the release timestamp.  TDRL in ID3v2.4.  
        /// Setting this field affects <see cref="Year"/> (TYER/TYE).
        /// <para>
        /// The timestamp fields are based on a subset of ISO-8601. When being as
        /// precise as possible the format of a time string is
        /// yyyy-MM-ddTHH:mm:ss (year, "-", month, "-", day, "T", hour (out of
        /// 24), ":", minutes, ":", seconds), but the precision may be reduced by
        /// removing as many time indicators as wanted. Hence valid timestamps
        /// are yyyy, yyyy-MM, yyyy-MM-dd, yyyy-MM-ddTHH, yyyy-MM-ddTHH:mm and
        /// yyyy-MM-ddTHH:mm:ss.
        /// </para>
        /// </summary>
        /// <value>The release timestamp.  TDRL in ID3v2.4.</value>
        string ReleaseTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the original release timestamp.  TDOR in ID3v2.4.  
        /// Setting this field affects ... TODO
        /// <para>
        /// The timestamp fields are based on a subset of ISO-8601. When being as
        /// precise as possible the format of a time string is
        /// yyyy-MM-ddTHH:mm:ss (year, "-", month, "-", day, "T", hour (out of
        /// 24), ":", minutes, ":", seconds), but the precision may be reduced by
        /// removing as many time indicators as wanted. Hence valid timestamps
        /// are yyyy, yyyy-MM, yyyy-MM-dd, yyyy-MM-ddTHH, yyyy-MM-ddTHH:mm and
        /// yyyy-MM-ddTHH:mm:ss.
        /// </para>
        /// </summary>
        /// <value>The original release timestamp.  TDOR in ID3v2.4.</value>
        string OriginalReleaseTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the recording timestamp.  TDRC in ID3v2.4.
        /// Setting this field affects <see cref="DateRecorded"/> (TDAT/TDA) and <see cref="TimeRecorded"/> (TIME/TIM). If
        /// the necessary precision is left off, the forementioned fields are set to null.  The "Year" portion of the RecordingTimestamp
        /// does not affect any other frames in the tag.
        /// <para>
        /// The timestamp fields are based on a subset of ISO-8601. When being as
        /// precise as possible the format of a time string is
        /// yyyy-MM-ddTHH:mm:ss (year, "-", month, "-", day, "T", hour (out of
        /// 24), ":", minutes, ":", seconds), but the precision may be reduced by
        /// removing as many time indicators as wanted. Hence valid timestamps
        /// are yyyy, yyyy-MM, yyyy-MM-dd, yyyy-MM-ddTHH, yyyy-MM-ddTHH:mm and
        /// yyyy-MM-ddTHH:mm:ss.
        /// </para>
        /// </summary>
        /// <value>The recording timestamp.  TDRC in ID3v2.4.</value>
        string RecordingTimestamp { get; set; }

        /// <summary>
        /// Gets or sets whether the track is part of a compilation.  TCMP/TCP.
        /// <para>Note: This is a non-standard ID3v2 frame that is used by some tag editors, including iTunes.</para>
        /// </summary>
        /// <value>
        /// <c>true</c> if track is part of a compilation set; otherwise, <c>false</c>.
        /// </value>
        bool IsPartOfCompilation { get; set; }

        /// <summary>
        /// Gets the list of user defined text frames.  TXXX/TXX.
        /// </summary>
        /// <value>The list of user defined text frames.  TXXX/TXX.</value>
        BindingList<ITXXXFrame> UserDefinedText { get; }

        /// <summary>
        /// Gets the list of commercial info URLs.  WCOM/WCM.
        /// </summary>
        /// <value>The list of commercial info URLs.  WCOM/WCM.</value>
        BindingList<IUrlFrame> CommercialInfoUrlList { get; }

        /// <summary>
        /// Gets or sets the copyright URL.  WCOP/WCP.
        /// </summary>
        /// <value>The copyright URL.  WCOP/WCP.</value>
        string CopyrightUrl { get; set; }

        /// <summary>
        /// Gets or sets the audio file URL.  WOAF/WAF.
        /// </summary>
        /// <value>The audio file URL.</value>
        string AudioFileUrl { get; set; }

        /// <summary>
        /// Gets the list of artist URLs.  WOAR/WAR.
        /// </summary>
        /// <value>The list of artist URLs.  WOAR/WAR.</value>
        BindingList<IUrlFrame> ArtistUrlList { get; }

        /// <summary>
        /// Gets or sets the audio source URL, e.g. a movie page.  WOAS/WAS.
        /// </summary>
        /// <value>The audio source URL, e.g. a movie page.  WOAS/WAS.</value>
        string AudioSourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the internet radio station URL.  WORS.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>The internet radio station URL.  WORS.  Only valid in ID3v2.3 and higher.</value>
        string InternetRadioStationUrl { get; set; }

        /// <summary>
        /// Gets or sets the payment URL.  WPAY.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>The payment URL.  WPAY.  Only valid in ID3v2.3 and higher.</value>
        string PaymentUrl { get; set; }

        /// <summary>
        /// Gets or sets the publisher/record label URL.  WPUB/WPB.
        /// </summary>
        /// <value>The publisher/record label URL.  WPUB/WPB.</value>
        string PublisherUrl { get; set; }

        /// <summary>
        /// Gets the BindingList of user defined URL frames.
        /// </summary>
        /// <value>The BindingList of user defined URL frames.</value>
        BindingList<IWXXXFrame> UserDefinedUrlList { get; }

        /// <summary>
        /// Gets the involved persons frame.
        /// </summary>
        /// <value>The involved persons frame.</value>
        IInvolvedPersonList InvolvedPersonList { get; }

        /// <summary>
        /// Gets the Music CD Identifier (Table of Contents) frame.
        /// </summary>
        /// <value>The Music CD Identifier (Table of Contents) frame.</value>
        IMusicCDIdentifier MusicCDIdentifier { get; }

        /// <summary>
        /// Gets the event timing frame.
        /// </summary>
        /// <value>The event timing frame.</value>
        IEventTiming EventTiming { get; }

        /// <summary>
        /// Gets the MPEG lookup table frame.
        /// </summary>
        /// <value>The MPEG lookup table frame.</value>
        IMpegLookupTable MpegLookupTable { get; }

        /// <summary>
        /// Gets the synchronized tempo codes frame.
        /// </summary>
        /// <value>The synchronized tempo codes frame.</value>
        ISynchronizedTempoCodes SynchronizedTempoCodeList { get; }

        /// <summary>
        /// Gets the BindingList of unsynchronized lyrics frames.
        /// </summary>
        /// <value>The BindingList of unsynchronized lyrics frames.</value>
        BindingList<IUnsynchronizedText> UnsynchronizedLyricsList { get; }

        /// <summary>
        /// Gets the BindingList of synchronized lyrics frames.
        /// </summary>
        /// <value>The BindingList of synchronized lyrics frames.</value>
        BindingList<ISynchronizedText> SynchronizedLyrics { get; }

        /// <summary>
        /// Gets the BindingList of comment frames. Excludes iTunes comment frames, see <see cref="iTunesCommentsList" /> instead.
        /// </summary>
        /// <value>The BindingList of comment frames.  Excludes iTunes comment frames, see <see cref="iTunesCommentsList" /> instead.</value>
        BindingList<IComments> CommentsList { get; }

        /// <summary>
        /// Gets the BindingList of iTunes comment frames. For example: iTunNORM, iTunSMPB, iTunPGAP, iTunes_CDDB_IDs, iTunes_CDDB_1, iTunes_CDDB_TrackNumber.
        /// </summary>
        /// <value>The BindingList of iTunes comment frames. For example: iTunNORM, iTunSMPB, iTunPGAP, iTunes_CDDB_IDs, iTunes_CDDB_1, iTunes_CDDB_TrackNumber.</value>
        BindingList<IComments> iTunesCommentsList { get; }

        /// <summary>
        /// Gets the BindingList of relative volume adjustment frames.
        /// </summary>
        /// <value>The BindingList of relative volume adjustment frames.</value>
        BindingList<IRelativeVolumeAdjustment> RelativeVolumeAdjustmentList { get; }

        /// <summary>
        /// Gets the BindingList of equalization frames.
        /// </summary>
        /// <value>The BindingList of equalization frames.</value>
        BindingList<IEqualizationList> EqualizationList { get; }

        /// <summary>
        /// Gets the reverb frame.
        /// </summary>
        /// <value>The reverb frame.</value>
        IReverb Reverb { get; }

        /// <summary>
        /// Gets the BindingList of attached picture frames.
        /// </summary>
        /// <value>The BindingList of attached picture frames.</value>
        BindingList<IAttachedPicture> PictureList { get; }

        /// <summary>
        /// Gets the BindingList of general encapsulated object frames.
        /// </summary>
        /// <value>The BindingList of general encapsulated object frames.</value>
        BindingList<IGeneralEncapsulatedObject> GeneralEncapsulatedObjectList { get; }

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        /// <value>The play count.</value>
        IPlayCount PlayCount { get; }

        /// <summary>
        /// Gets or sets if podcast.
        /// </summary>
        /// <value>True if podcast.</value>
        bool IsPodcast { get; set;  }
        
        /// <summary>
        /// Gets the BindingList of popularimeter frames.
        /// </summary>
        /// <value>The BindingList of popularimeter frames.</value>
        BindingList<IPopularimeter> PopularimeterList { get; }

        /// <summary>
        /// Gets the recommended buffer size frame.
        /// </summary>
        /// <value>The recommended buffer size frame.</value>
        IRecommendedBufferSize RecommendedBufferSize { get; }

        /// <summary>
        /// Gets the BindingList of audio encryption frames.
        /// </summary>
        /// <value>The BindingList of audio encryption frames.</value>
        BindingList<IAudioEncryption> AudioEncryptionList { get; }

        /// <summary>
        /// Gets the BindingList of linked information frames.
        /// </summary>
        /// <value>The BindingList of linked information frames.</value>
        BindingList<ILinkedInformation> LinkedInformationList { get; }

        /// <summary>
        /// Gets the position synchronization frame.
        /// </summary>
        /// <value>The position synchronization frame.</value>
        IPositionSynchronization PositionSynchronization { get; }

        /// <summary>
        /// Gets the audio seek point index frame.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The index of the audio seek point.  Only supported in ID3v2.4.</value>
        IAudioSeekPointIndex AudioSeekPointIndex { get; }

        /// <summary>
        /// Gets the BindingList of terms of use frames.  USER.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>The BindingList of terms of use frames.  USER.  Only supported in ID3v2.3 and ID3v2.4.</value>
        BindingList<ITermsOfUse> TermsOfUseList { get; }

        /// <summary>
        /// Gets the BindingList of commercial info frames.  COMR.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>The BindingList of commercial info frames.  COMR.  Only supported in ID3v2.3 and ID3v2.4.</value>
        BindingList<ICommercial> CommercialInfoList { get; }

        /// <summary>
        /// Gets the BindingList of encryption method frames.
        /// </summary>
        /// <value>The BindingList of encryption method frames.</value>
        BindingList<IEncryptionMethod> EncryptionMethodList { get; }

        /// <summary>
        /// Gets the BindingList of group identification frames.
        /// </summary>
        /// <value>The BindingList of group identification frames.</value>
        BindingList<IGroupIdentification> GroupIdentificationList { get; }

        /// <summary>
        /// Gets the BindingList of private frames.  Only valid in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>The BindingList of private frames.  Only valid in ID3v2.3 and ID3v2.4.</value>
        BindingList<IPrivateFrame> PrivateFrameList { get; }

        /// <summary>
        /// Gets or sets the date and time the audio was encoded.  Only valid in ID3v2.4.  TDEN in ID3v2.4.
        /// </summary>
        /// <value>The date and time the audio was encoded.  Only valid in ID3v2.4.  TDEN in ID3v2.4.</value>
        string EncodingTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the date and time the audio was tagged.  Only valid in ID3v2.4.  TDTG in ID3v2.4.
        /// </summary>
        /// <value>The date and time the audio was tagged.  Only valid in ID3v2.4.  TDTG in ID3v2.4.</value>
        string TaggingTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the mood.  Only valid in ID3v2.4.  TMOO in ID3v2.4.
        /// </summary>
        /// <value>The mood.  Only valid in ID3v2.4.  TMOO in ID3v2.4.</value>
        string Mood { get; set; }

        /// <summary>
        /// Gets or sets the album sort order, which defines a string which should be used
        /// instead of the album name for sorting purposes.  Only supported in ID3v2.4.  TSOA in ID3v2.4.
        /// </summary>
        /// <value>The album sort order, which defines a string which should be used
        /// instead of the album name for sorting purposes.  Only supported in ID3v2.4.  TSOA in ID3v2.4.</value>
        string AlbumSortOrder { get; set; }

        /// <summary>
        /// Gets or sets the artist sort order, which defines a string which should be used
        /// instead of the artist name for sorting purposes.  Also known as 'Performer Sort Order'.
        /// Only supported in ID3v2.4.  TSOP in ID3v2.4.
        /// </summary>
        /// <value>The artist sort order, which defines a string which should be used
        /// instead of the artist name for sorting purposes.  Also known as 'Performer Sort Order'.
        /// Only supported in ID3v2.4.  TSOP in ID3v2.4.</value>
        string ArtistSortOrder { get; set; }

        /// <summary>
        /// Gets or sets the title sort order, which defines a string which should be used
        /// instead of the title name for sorting purposes.  Only supported in ID3v2.4.  TSOT in ID3v2.4.
        /// </summary>
        /// <value>The title sort order.</value>
        string TitleSortOrder { get; set; }

        /// <summary>
        /// Gets or sets the produced notice, which is a string which must begin with a
        /// year and a space character.  Intended for
        /// the production copyright holder of the original sound, not the audio
        /// file itself.  Only supported in ID3v2.4.  TPRO in ID3v2.4.
        /// </summary>
        /// <value>The produced notice.</value>
        string ProducedNotice { get; set; }

        /// <summary>
        /// Gets or sets the set subtitle.  Intended for the subtitle of the 'part of
        /// a set' (for example, a specific disc in a multi disc album) this track belongs to.
        /// Only supported in ID3v2.4.  TSST in ID3v2.4.
        /// </summary>
        /// <value>The set subtitle.  Intended for the subtitle of the 'part of
        /// a set' (for example, a specific disc in a multi disc album) this track belongs to.
        /// Only supported in ID3v2.4.  TSST in ID3v2.4.</value>
        string SetSubtitle { get; set; }

        /// <summary>
        /// Gets or sets the series category. Used for iTunes Podcasts.
        /// Only supported in ID3v2.3+.  TCAT in ID3v2.3.
        /// </summary>
        /// <value>
        /// The series category. TCAT
        /// </value>
        string PodcastSeriesCategory { get; set; }

        /// <summary>
        /// Gets or sets the episode description. Used for iTunes Podcasts.
        /// Only supported in ID3v2.3+.  TCAT in ID3v2.3.
        /// </summary>
        /// <value>
        /// The episode description. TDES
        /// </value>
        string PodcastEpisodeDescription { get; set; }

        /// <summary>
        /// Gets or sets the episode URL. Used for iTunes Podcasts.
        /// Only supported in ID3v2.3+.  TGID in ID3v2.3.
        /// </summary>
        /// <value>
        /// The episode description. TGID
        /// </value>
        string PodcastEpisodeUrl { get; set; }

        /// <summary>
        /// Gets or sets the podcast feed URL.  Used in iTunes Podcasts. WFED.
        /// </summary>
        /// <value>The podcast feed URL.  WFED.</value>
        string PodcastFeedUrl { get; set; }

        /// <summary>
        /// Gets the ownership frame.  OWNE.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>The ownership frame.  OWNE.  Only supported in ID3v2.3 and ID3v2.4.</value>
        IOwnership Ownership { get; }

        /// <summary>
        /// Gets the seek next tag frame.  SEEK.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The seek next tag frame.  SEEK.  Only supported in ID3v2.4.</value>
        ISeekNextTag SeekNextTag { get; }

        /// <summary>
        /// Gets the BindingList of signatures.  SIGN.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The BindingList of signatures.  SIGN.  Only supported in ID3v2.4.</value>
        BindingList<ISignature> SignatureList { get; }

        /// <summary>
        /// Gets the musician credits list.  TMCL.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The musician credits list.  TMCL.  Only supported in ID3v2.4.</value>
        IMusicianCreditsList MusicianCreditsList { get; }

        /// <summary>
        /// Gets the BindingList of audio text frames.  ATXT.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>The BindingList of audio text frames.  ATXT.  Only supported in ID3v2.3 and ID3v2.4.</value>
        BindingList<IAudioText> AudioTextList { get; }

        /// <summary>
        /// Forces the <see cref="INotifyPropertyChanged.PropertyChanged"/> event to fire.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        void RaisePropertyChanged(string propertyName);
    }
}

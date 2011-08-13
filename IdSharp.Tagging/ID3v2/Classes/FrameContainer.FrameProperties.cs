using System;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.ID3v2
{
    public abstract partial class FrameContainer : IFrameContainer
    {
        /// <summary>
        /// Gets the list of unique file identifiers.  See <see cref="IUniqueFileIdentifier"/>.  UFID/UFI.
        /// </summary>
        /// <value>
        /// The list of unique file identifiers.  See <see cref="IUniqueFileIdentifier"/>.  UFID/UFI.
        /// </value>
        public BindingList<IUniqueFileIdentifier> UniqueFileIdentifierList
        {
            get { return m_UniqueFileIdentifierList; }
        }

        /// <summary>
        /// Gets or sets the album.  The 'Album/Movie/Show title' frame is intended for the title of the
        /// recording/source of sound which the audio in the file is taken from.  TALB/TAL.
        /// </summary>
        /// <value>
        /// The album name.  The 'Album/Movie/Show title' frame is intended for the title of the
        /// recording/source of sound which the audio in the file is taken from.  TALB/TAL.
        /// </value>
        public string Album
        {
            get { return m_Album.Value; }
            set { m_Album.Value = value; }
        }

        /// <summary>
        /// Gets or sets the number of beats per minute in the mainpart of the audio.  TBPM/TBP.
        /// </summary>
        /// <value>
        /// The the number of beats per minute in the mainpart of the audio.  TBPM/TBP.
        /// </value>
        public string BPM
        {
            get { return m_BPM.Value; }
            set { m_BPM.Value = value; }
        }

        /// <summary>
        /// Gets or sets the composer.  TCOM/TCM.
        /// </summary>
        /// <value>The composer.  TCOM/TCM.</value>
        public string Composer
        {
            get { return m_Composer.Value; }
            set { m_Composer.Value = value; }
        }

        /// <summary>
        /// Gets or sets the genre.  TCON/TCO.
        /// </summary>
        /// <value>The genre.  TCON/TCO.</value>
        public string Genre
        {
            get { return m_Genre.Value; }
            set { m_Genre.Value = value; }
        }

        /// <summary>
        /// Gets or sets the copyright message, which is a string which must begin with a
        /// year and a space character.  Intended for
        /// the copyright holder of the original sound, not the audio file
        /// itself.  TCOP/TCR.
        /// </summary>
        /// <value>The copyright message.  TCOP/TCR.</value>
        public string Copyright
        {
            get { return m_Copyright.Value; }
            set { m_Copyright.Value = value; }
        }

        /// <summary>
        /// Gets or sets the date recorded in MMDD format.  TDAT in ID3v2.3, TDA in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).
        /// </summary>
        /// <value>
        /// The date recorded in MMDD format.  TDAT in ID3v2.3, TRD in ID3v2.2  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).
        /// </value>
        public string DateRecorded
        {
            get { return m_DateRecorded.Value; }
            set { m_DateRecorded.Value = value; }
        }

        /// <summary>
        /// Gets or sets the playlist delay in milliseconds.  TDLY/TDY.
        /// </summary>
        /// <value>The playlist delay in milliseconds.  TDLY/TDY.</value>
        public int? PlaylistDelayMilliseconds
        {
            get
            {
                int result;
                if (int.TryParse(m_PlaylistDelayMilliseconds.Value, out result))
                    return result;
                else
                    return null;
            }
            set
            {
                if (value == null)
                {
                    m_PlaylistDelayMilliseconds.Value = null;
                }
                else
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                    }

                    m_PlaylistDelayMilliseconds.Value = value.Value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the person or organisation that encoded the audio file.  TENC/TEN.
        /// </summary>
        /// <value>
        /// The name of the person or organisation that encoded the audio file.  TENC/TEN.
        /// </value>
        public string EncodedByWho
        {
            get { return m_EncodedByWho.Value; }
            set { m_EncodedByWho.Value = value; }
        }

        /// <summary>
        /// Gets or sets the lyricist.  TEXT/TXT.  TODO: Add list for predefined types.
        /// </summary>
        /// <value>The lyricist.  TEXT/TXT.</value>
        public string Lyricist
        {
            get { return m_Lyricist.Value; }
            set { m_Lyricist.Value = value; }
        }

        /// <summary>
        /// Gets or sets the type of the file.  TFLT/TFT.
        /// </summary>
        /// <value>The type of the file.  TFLT/TFT.</value>
        public string FileType
        {
            get { return m_FileType.Value; }
            set { m_FileType.Value = value; }
        }

        /// <summary>
        /// Gets or sets the time recorded in HHMM format.  TIME in ID3v2.3, TIM in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).
        /// </summary>
        /// <value>
        /// The time recorded in HHMM format.  TIME in ID3v2.3, TIM in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="RecordingTimestamp"/> (TDRC).
        /// </value>
        public string TimeRecorded
        {
            get { return m_TimeRecorded.Value; }
            set { m_TimeRecorded.Value = value; }
        }

        /// <summary>
        /// Gets or sets the content group.  The 'Content group description' frame
        /// is used if the sound belongs to
        /// a larger category of sounds/music. For example, classical music is
        /// often sorted in different musical sections (e.g. "Piano Concerto",
        /// "Weather - Hurricane"). TIT1/TT1.
        /// </summary>
        /// <value>
        /// The content group.  The 'Content group description' frame
        /// is used if the sound belongs to
        /// a larger category of sounds/music. For example, classical music is
        /// often sorted in different musical sections (e.g. "Piano Concerto",
        /// "Weather - Hurricane").  TIT1/TT1.
        /// </value>
        public string ContentGroup
        {
            get { return m_ContentGroup.Value; }
            set { m_ContentGroup.Value = value; }
        }

        /// <summary>
        /// Gets or sets the title/song name/content description.  TIT2/TT2.
        /// </summary>
        /// <value>The title/song name/content description.  TIT2/TT2.</value>
        public string Title
        {
            get { return m_Title.Value; }
            set { m_Title.Value = value; }
        }

        /// <summary>
        /// Gets or sets the artist. TPE1/TP1.
        /// </summary>
        /// <value>The artist. TPE1/TP1.</value>
        public string Artist
        {
            get { return m_Artist.Value; }
            set { m_Artist.Value = value; }
        }

        /// <summary>
        /// Gets or sets the album artist. TPE2/TP2.
        /// </summary>
        /// <value>The album artist. TPE2/TP2.</value>
        public string AlbumArtist
        {
            get { return m_AlbumArtist.Value; }
            set { m_AlbumArtist.Value = value; }
        }

        /// <summary>
        /// Gets or sets the subtitle/description refinement.  TIT3/TT3.
        /// </summary>
        /// <value>The subtitle/description refinement.  TIT3/TT3.</value>
        public string Subtitle
        {
            get { return m_Subtitle.Value; }
            set { m_Subtitle.Value = value; }
        }

        /// <summary>
        /// Gets or sets the initial key.  Represented as a string with a maximum length of three
        /// characters.  The ground keys are represented with "A","B","C","D","E",
        /// "F" and "G" and halfkeys represented with "b" and "#". Minor is
        /// represented as "m". Example "Cbm". Off key is represented with an "o"
        /// only.  TKEY/TKE.
        /// </summary>
        /// <value>The initial key.</value>
        public string InitialKey
        {
            get { return m_InitialKey.Value; }
            set { m_InitialKey.Value = value; }
        }

        /// <summary>
        /// Gets the languages of the text or lyrics in the audio file.  TLAN/TLA.
        /// </summary>
        /// <value>
        /// The languages of the text or lyrics in the audio file.  TLAN/TLA.
        /// </value>
        public ILanguageFrame Languages
        {
            get { return m_Languages; }
        }

        /// <summary>
        /// Gets or sets the length of the audio in milliseconds, as reported by the ID3 tag.  TLEN/TLE.
        /// </summary>
        /// <value>
        /// The length of the audio in milliseconds, as reported by the ID3 tag.  TLEN/TLE.
        /// </value>
        public int? LengthMilliseconds
        {
            get
            {
                int result;
                if (int.TryParse(m_LengthMilliseconds.Value, out result))
                    return result;
                else
                    return null;
            }
            set
            {
                if (value == null)
                    m_LengthMilliseconds.Value = null;
                else
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                    }

                    m_LengthMilliseconds.Value = value.Value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the media.  TMED/TMT.  TODO: Dictionary/list of predefined types.
        /// </summary>
        /// <value>The type of the media.  TMED/TMT.</value>
        public string MediaType
        {
            get { return m_MediaType.Value; }
            set { m_MediaType.Value = value; }
        }

        /// <summary>
        /// Gets or sets the original album/movie/show/source of sound title.  TOAL/TOT.
        /// </summary>
        /// <value>The original album/movie/show/source of sound title.  TOAL/TOT.</value>
        public string OriginalSourceTitle
        {
            get { return m_OriginalSourceTitle.Value; }
            set { m_OriginalSourceTitle.Value = value; }
        }

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
        public string OriginalFileName
        {
            get { return m_OriginalFileName.Value; }
            set { m_OriginalFileName.Value = value; }
        }

        /// <summary>
        /// Gets or sets the original lyricist.  TOLY/TOL.
        /// </summary>
        /// <value>The original lyricist.  TOLY/TOL.</value>
        public string OriginalLyricist
        {
            get { return m_OriginalLyricist.Value; }
            set { m_OriginalLyricist.Value = value; }
        }

        /// <summary>
        /// Gets or sets the original artist.  TOPE/TOA.
        /// </summary>
        /// <value>The original artist.  TOPE/TOA.</value>
        public string OriginalArtist
        {
            get { return m_OriginalArtist.Value; }
            set { m_OriginalArtist.Value = value; }
        }

        /// <summary>
        /// Gets or sets the original release year.  TDOR/TORY/TOR.  TODO: New implementation in ID3v2.4.
        /// </summary>
        /// <value>The original release year.</value>
        public string OriginalReleaseYear
        {
            get { return m_OriginalReleaseYear.Value; }
            set { m_OriginalReleaseYear.Value = value; }
        }

        /// <summary>
        /// Gets or sets the name of the file owner.  TOWN.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>The name of the file owner.</value>
        public string FileOwnerName
        {
            get { return m_FileOwnerName.Value; }
            set { m_FileOwnerName.Value = value; }
        }

        /*/// <summary>
        /// Gets or sets the accompaniment.  TPE2/TP2.
        /// </summary>
        /// <value>The accompaniment.  TPE2/TP2.</value>
        public string Accompaniment
        {
            get { return m_Accompaniment.Value; }
            set { m_Accompaniment.Value = value; }
        }*/

        /// <summary>
        /// Gets or sets the conductor.  TPE3/TP3.
        /// </summary>
        /// <value>The conductor.  TPE3/TP3.</value>
        public string Conductor
        {
            get { return m_Conductor.Value; }
            set { m_Conductor.Value = value; }
        }

        /// <summary>
        /// Gets or sets who remixed the audio.  TPE4/TP4.
        /// </summary>
        /// <value>Who remixed the audio.  TPE4/TP4.</value>
        public string RemixedBy
        {
            get { return m_RemixedBy.Value; }
            set { m_RemixedBy.Value = value; }
        }

        /// <summary>
        /// Gets or sets the disc number.  TPOS/TPA.
        /// </summary>
        /// <value>The disc number.  TPOS/TPA.</value>
        public string DiscNumber
        {
            get { return m_DiscNumber.Value; }
            set { m_DiscNumber.Value = value; }
        }

        /// <summary>
        /// Gets or sets the publisher/record label.  TPUB/TPB.
        /// </summary>
        /// <value>The publisher/record label.  TPUB/TPB.</value>
        public string Publisher
        {
            get { return m_Publisher.Value; }
            set { m_Publisher.Value = value; }
        }

        /// <summary>
        /// Gets or sets the track number.  TRCK/TRK.
        /// </summary>
        /// <value>The track number.  TRCK/TRK.</value>
        public string TrackNumber
        {
            get { return m_TrackNumber.Value; }
            set { m_TrackNumber.Value = value; }
        }

        /// <summary>
        /// Gets or sets the recording dates.  TRDA/TRD.  TODO: Replaced by TDRC in ID3v2.4.
        /// </summary>
        /// <value>The recording dates.  TRDA/TRD.</value>
        public string RecordingDates
        {
            get { return m_RecordingDates.Value; }
            set { m_RecordingDates.Value = value; }
        }

        /// <summary>
        /// Gets or sets the name of the internet radio station.  TRSN.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>
        /// The name of the internet radio station.  TRSN.  Only valid in ID3v2.3 and higher.
        /// </value>
        public string InternetRadioStationName
        {
            get { return m_InternetRadioStationName.Value; }
            set { m_InternetRadioStationName.Value = value; }
        }

        /// <summary>
        /// Gets or sets the internet radio station owner.  TRSO.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>
        /// The internet radio station owner.  TRSO.  Only valid in ID3v2.3 and higher.
        /// </value>
        public string InternetRadioStationOwner
        {
            get { return m_InternetRadioStationOwner.Value; }
            set { m_InternetRadioStationOwner.Value = value; }
        }

        /// <summary>
        /// Gets or sets the file size excluding the ID3v2 tag, as reported by the tag.  TSIZ/TSI.  Only valid in ID3v2.3 and ID3v2.2.
        /// </summary>
        /// <value>
        /// The file size excluding the ID3v2 tag, as reported by the tag.  TSIZ/TSI.  Only valid in ID3v2.3 and ID3v2.2.
        /// </value>
        public long? FileSizeExcludingTag
        {
            get
            {
                long result;
                if (long.TryParse(m_FileSizeExcludingTag.Value, out result))
                    return result;
                else
                    return null;
            }
            set
            {
                if (value == null)
                    m_FileSizeExcludingTag.Value = null;
                else
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                    }

                    m_FileSizeExcludingTag.Value = value.Value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the ISRC code.  Value should be 12 characters in length.  TSRC/TRC.
        /// </summary>
        /// <value>
        /// The ISRC code.  Value should be 12 characters in length.  TSRC/TRC.
        /// </value>
        public string ISRC
        {
            get { return m_ISRC.Value; }
            set { m_ISRC.Value = value; }
        }

        /// <summary>
        /// Gets or sets the software/hardware and settings used for encoding.  TSSE/TSS.
        /// </summary>
        /// <value>The encoder settings.  TSSE/TSS.</value>
        public string EncoderSettings
        {
            get { return m_EncoderSettings.Value; }
            set { m_EncoderSettings.Value = value; }
        }

        /// <summary>
        /// Gets or sets the year.  TYER in ID3v2.3, TYR in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="ReleaseTimestamp"/> (TDRL).
        /// </summary>
        /// <value>
        /// The year.  TYER in ID3v2.3, TYR in ID3v2.2.  Replaced by TDRL and TDRC in ID3v2.4.  Setting this field affects <see cref="ReleaseTimestamp"/> (TDRL).
        /// </value>
        public string Year
        {
            get { return m_Year.Value; }
            set { m_Year.Value = value; }
        }

        /// <summary>
        /// Gets the list of user defined text frames.  TXXX/TXX.
        /// </summary>
        /// <value>The list of user defined text frames.  TXXX/TXX.</value>
        public BindingList<ITXXXFrame> UserDefinedText
        {
            get { return m_UserDefinedTextList; }
        }

        /// <summary>
        /// Gets the list of commercial info URLs.  WCOM/WCM.
        /// </summary>
        /// <value>The list of commercial info URLs.  WCOM/WCM.</value>
        public BindingList<IUrlFrame> CommercialInfoUrlList
        {
            get { return m_CommercialInfoUrlList; }
        }

        /// <summary>
        /// Gets or sets the copyright URL.  WCOP/WCP.
        /// </summary>
        /// <value>The copyright URL.  WCOP/WCP.</value>
        public string CopyrightUrl
        {
            get { return m_CopyrightUrl.Value; }
            set { m_CopyrightUrl.Value = value; }
        }

        /// <summary>
        /// Gets or sets the audio file URL.  WOAF/WAF.
        /// </summary>
        /// <value>The audio file URL.</value>
        public string AudioFileUrl
        {
            get { return m_AudioFileUrl.Value; }
            set { m_AudioFileUrl.Value = value; }
        }

        /// <summary>
        /// Gets the list of artist URLs.  WOAR/WAR.
        /// </summary>
        /// <value>The list of artist URLs.  WOAR/WAR.</value>
        public BindingList<IUrlFrame> ArtistUrlList
        {
            get { return m_ArtistUrlList; }
        }

        /// <summary>
        /// Gets or sets the audio source URL, e.g. a movie page.  WOAS/WAS.
        /// </summary>
        /// <value>The audio source URL, e.g. a movie page.  WOAS/WAS.</value>
        public string AudioSourceUrl
        {
            get { return m_AudioSourceUrl.Value; }
            set { m_AudioSourceUrl.Value = value; }
        }

        /// <summary>
        /// Gets or sets the internet radio station URL.  WORS.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>
        /// The internet radio station URL.  WORS.  Only valid in ID3v2.3 and higher.
        /// </value>
        public string InternetRadioStationUrl
        {
            get { return m_InternetRadioStationUrl.Value; }
            set { m_InternetRadioStationUrl.Value = value; }
        }

        /// <summary>
        /// Gets or sets the payment URL.  WPAY.  Only valid in ID3v2.3 and higher.
        /// </summary>
        /// <value>The payment URL.  WPAY.  Only valid in ID3v2.3 and higher.</value>
        public string PaymentUrl
        {
            get { return m_PaymentUrl.Value; }
            set { m_PaymentUrl.Value = value; }
        }

        /// <summary>
        /// Gets or sets the publisher/record label URL.  WPUB/WPB.
        /// </summary>
        /// <value>The publisher/record label URL.  WPUB/WPB.</value>
        public string PublisherUrl
        {
            get { return m_PublisherUrl.Value; }
            set { m_PublisherUrl.Value = value; }
        }

        /// <summary>
        /// Gets the BindingList of user defined URL frames.
        /// </summary>
        /// <value>The BindingList of user defined URL frames.</value>
        public BindingList<IWXXXFrame> UserDefinedUrlList
        {
            get { return m_UserDefinedUrlList; }
        }

        /// <summary>
        /// Gets the involved persons frame.
        /// </summary>
        /// <value>The involved persons frame.</value>
        public IInvolvedPersonList InvolvedPersonList
        {
            get { return m_InvolvedPersonList; }
        }

        /// <summary>
        /// Gets the Music CD Identifier (Table of Contents) frame.
        /// </summary>
        /// <value>The Music CD Identifier (Table of Contents) frame.</value>
        public IMusicCDIdentifier MusicCDIdentifier
        {
            get { return m_MusicCDIdentifier; }
        }

        /// <summary>
        /// Gets the event timing frame.
        /// </summary>
        /// <value>The event timing frame.</value>
        public IEventTiming EventTiming
        {
            get { return m_EventTiming; }
        }

        /// <summary>
        /// Gets the MPEG lookup table frame.
        /// </summary>
        /// <value>The MPEG lookup table frame.</value>
        public IMpegLookupTable MpegLookupTable
        {
            get { return m_MpegLookupTable; }
        }

        /// <summary>
        /// Gets the synchronized tempo codes frame.
        /// </summary>
        /// <value>The synchronized tempo codes frame.</value>
        public ISynchronizedTempoCodes SynchronizedTempoCodeList
        {
            get { return m_SynchronizedTempoCodes; }
        }

        /// <summary>
        /// Gets the BindingList of unsynchronized lyrics frames.
        /// </summary>
        /// <value>The BindingList of unsynchronized lyrics frames.</value>
        public BindingList<IUnsynchronizedText> UnsynchronizedLyricsList
        {
            get { return m_UnsynchronizedLyricsList; }
        }

        /// <summary>
        /// Gets the BindingList of synchronized lyrics frames.
        /// </summary>
        /// <value>The BindingList of synchronized lyrics frames.</value>
        public BindingList<ISynchronizedText> SynchronizedLyrics
        {
            get { return m_SynchronizedLyricsList; }
        }

        /// <summary>
        /// Gets the BindingList of comment frames. Excludes iTunes comment frames, see <see cref="iTunesCommentsList" /> instead.
        /// </summary>
        /// <value>The BindingList of comment frames.  Excludes iTunes comment frames, see <see cref="iTunesCommentsList" /> instead.</value>
        public BindingList<IComments> CommentsList
        {
            get { return m_CommentsList; }
        }

        /// <summary>
        /// Gets the BindingList of iTunes comment frames. For example: iTunNORM, iTunSMPB, iTunPGAP, iTunes_CDDB_IDs, iTunes_CDDB_1, iTunes_CDDB_TrackNumber.
        /// </summary>
        /// <value>The BindingList of iTunes comment frames. For example: iTunNORM, iTunSMPB, iTunPGAP, iTunes_CDDB_IDs, iTunes_CDDB_1, iTunes_CDDB_TrackNumber.</value>
        public BindingList<IComments> iTunesCommentsList
        {
            get { return m_iTunesCommentsList; }
        }

        /// <summary>
        /// Gets the BindingList of relative volume adjustment frames.
        /// </summary>
        /// <value>The BindingList of relative volume adjustment frames.</value>
        public BindingList<IRelativeVolumeAdjustment> RelativeVolumeAdjustmentList
        {
            get { return m_RelativeVolumeAdjustmentList; }
        }

        /// <summary>
        /// Gets the BindingList of equalization frames.
        /// </summary>
        /// <value>The BindingList of equalization frames.</value>
        public BindingList<IEqualizationList> EqualizationList
        {
            get { return m_EqualizationList; }
        }

        /// <summary>
        /// Gets the reverb frame.
        /// </summary>
        /// <value>The reverb frame.</value>
        public IReverb Reverb
        {
            get { return m_Reverb; }
        }

        /// <summary>
        /// Gets the BindingList of attached picture frames.
        /// </summary>
        /// <value>The BindingList of attached picture frames.</value>
        public BindingList<IAttachedPicture> PictureList
        {
            get { return m_AttachedPictureList; }
        }

        /// <summary>
        /// Gets the BindingList of general encapsulated object frames.
        /// </summary>
        /// <value>The BindingList of general encapsulated object frames.</value>
        public BindingList<IGeneralEncapsulatedObject> GeneralEncapsulatedObjectList
        {
            get { return m_GeneralEncapsulatedObjectList; }
        }

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        /// <value>The play count.</value>
        public IPlayCount PlayCount
        {
            get { return m_PlayCount; }
        }

        /// <summary>
        /// Gets or sets if the file is podcast.
        /// </summary>
        /// <value><c>true</c> if the file is a podcast; otherwise, <c>false</c>.</value>
        public bool IsPodcast
        {
            get { return m_IsPodcast.Value; }
            set { m_IsPodcast.Value = value; }
        }

        /// <summary>
        /// Gets the BindingList of popularimeter frames.
        /// </summary>
        /// <value>The BindingList of popularimeter frames.</value>
        public BindingList<IPopularimeter> PopularimeterList
        {
            get { return m_PopularimeterList; }
        }

        /// <summary>
        /// Gets the recommended buffer size frame.
        /// </summary>
        /// <value>The recommended buffer size frame.</value>
        public IRecommendedBufferSize RecommendedBufferSize
        {
            get { return m_RecommendedBufferSize; }
        }

        /// <summary>
        /// Gets the BindingList of audio encryption frames.
        /// </summary>
        /// <value>The BindingList of audio encryption frames.</value>
        public BindingList<IAudioEncryption> AudioEncryptionList
        {
            get { return m_AudioEncryptionList; }
        }

        /// <summary>
        /// Gets the BindingList of linked information frames.
        /// </summary>
        /// <value>The BindingList of linked information frames.</value>
        public BindingList<ILinkedInformation> LinkedInformationList
        {
            get { return m_LinkedInformationList; }
        }

        /// <summary>
        /// Gets the position synchronization frame.
        /// </summary>
        /// <value>The position synchronization frame.</value>
        public IPositionSynchronization PositionSynchronization
        {
            get { return m_PositionSynchronization; }
        }

        /// <summary>
        /// Gets the audio seek point index frame.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The index of the audio seek point.  Only supported in ID3v2.4.</value>
        public IAudioSeekPointIndex AudioSeekPointIndex
        {
            get { return m_AudioSeekPointIndex; }
        }

        /// <summary>
        /// Gets the BindingList of terms of use frames.  USER.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>
        /// The BindingList of terms of use frames.  USER.  Only supported in ID3v2.3 and ID3v2.4.
        /// </value>
        public BindingList<ITermsOfUse> TermsOfUseList
        {
            get { return m_TermsOfUseList; }
        }

        /// <summary>
        /// Gets the BindingList of commercial info frames.  COMR.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>
        /// The BindingList of commercial info frames.  COMR.  Only supported in ID3v2.3 and ID3v2.4.
        /// </value>
        public BindingList<ICommercial> CommercialInfoList
        {
            get { return m_CommercialInfoList; }
        }

        /// <summary>
        /// Gets the BindingList of encryption method frames.
        /// </summary>
        /// <value>The BindingList of encryption method frames.</value>
        public BindingList<IEncryptionMethod> EncryptionMethodList
        {
            get { return m_EncryptionMethodList; }
        }

        /// <summary>
        /// Gets the BindingList of group identification frames.
        /// </summary>
        /// <value>The BindingList of group identification frames.</value>
        public BindingList<IGroupIdentification> GroupIdentificationList
        {
            get { return m_GroupIdentificationList; }
        }

        /// <summary>
        /// Gets the BindingList of private frames.  Only valid in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>
        /// The BindingList of private frames.  Only valid in ID3v2.3 and ID3v2.4.
        /// </value>
        public BindingList<IPrivateFrame> PrivateFrameList
        {
            get { return m_PrivateFrameList; }
        }

        /// <summary>
        /// Gets or sets whether the track is part of a compilation.  TCMP/TCP.
        /// <para>Note: This is a non-standard ID3v2 frame that is used by some tag editors, including iTunes.</para>
        /// </summary>
        /// <value>
        /// 	<c>true</c> if track is part of a compilation set; otherwise, <c>false</c>.
        /// </value>
        public bool IsPartOfCompilation
        {
            get
            {
                int value;
                if (int.TryParse(m_IsPartOfCompilation.Value, out value) && value == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                m_IsPartOfCompilation.Value = (value ? "1" : "");
            }
        }

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
        public string ReleaseTimestamp
        {
            get { return m_ReleaseTimestamp.Value; }
            set { m_ReleaseTimestamp.Value = value; }
        }

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
        public string OriginalReleaseTimestamp
        {
            get { return m_OriginalReleaseTimestamp.Value; }
            set { m_OriginalReleaseTimestamp.Value = value; }
        }

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
        public string RecordingTimestamp
        {
            get { return m_RecordingTimestamp.Value; }
            set { m_RecordingTimestamp.Value = value; }
        }

        /// <summary>
        /// Gets or sets the date and time the audio was encoded.  Only valid in ID3v2.4.  TDEN in ID3v2.4.
        /// </summary>
        /// <value>
        /// The date and time the audio was encoded.  Only valid in ID3v2.4.  TDEN in ID3v2.4.
        /// </value>
        public string EncodingTimestamp
        {
            get { return m_EncodingTimestamp.Value; }
            set { m_EncodingTimestamp.Value = value; }
        }

        /// <summary>
        /// Gets or sets the date and time the audio was tagged.  Only valid in ID3v2.4.  TDTG in ID3v2.4.
        /// </summary>
        /// <value>
        /// The date and time the audio was tagged.  Only valid in ID3v2.4.  TDTG in ID3v2.4.
        /// </value>
        public string TaggingTimestamp
        {
            get { return m_TaggingTimestamp.Value; }
            set { m_TaggingTimestamp.Value = value; }
        }

        /// <summary>
        /// Gets or sets the mood.  Only valid in ID3v2.4.  TMOO in ID3v2.4.
        /// </summary>
        /// <value>The mood.  Only valid in ID3v2.4.  TMOO in ID3v2.4.</value>
        public string Mood
        {
            get { return m_Mood.Value; }
            set { m_Mood.Value = value; }
        }

        /// <summary>
        /// Gets or sets the album sort order, which defines a string which should be used
        /// instead of the album name for sorting purposes.  Only supported in ID3v2.4.  TSOA in ID3v2.4.
        /// </summary>
        /// <value>
        /// The album sort order, which defines a string which should be used
        /// instead of the album name for sorting purposes.  Only supported in ID3v2.4.  TSOA in ID3v2.4.
        /// </value>
        public string AlbumSortOrder
        {
            get { return m_AlbumSortOrder.Value; }
            set { m_AlbumSortOrder.Value = value; }
        }

        /// <summary>
        /// Gets or sets the artist sort order, which defines a string which should be used
        /// instead of the artist name for sorting purposes.  Also known as 'Performer Sort Order'.
        /// Only supported in ID3v2.4.  TSOP in ID3v2.4.
        /// </summary>
        /// <value>
        /// The artist sort order, which defines a string which should be used
        /// instead of the artist name for sorting purposes.  Also known as 'Performer Sort Order'.
        /// Only supported in ID3v2.4.  TSOP in ID3v2.4.
        /// </value>
        public string ArtistSortOrder
        {
            get { return m_ArtistSortOrder.Value; }
            set { m_ArtistSortOrder.Value = value; }
        }

        /// <summary>
        /// Gets or sets the title sort order, which defines a string which should be used
        /// instead of the title name for sorting purposes.  Only supported in ID3v2.4.  TSOT in ID3v2.4.
        /// </summary>
        /// <value>The title sort order.</value>
        public string TitleSortOrder
        {
            get { return m_TitleSortOrder.Value; }
            set { m_TitleSortOrder.Value = value; }
        }

        /// <summary>
        /// Gets or sets the produced notice, which is a string which must begin with a
        /// year and a space character.  Intended for
        /// the production copyright holder of the original sound, not the audio
        /// file itself.  Only supported in ID3v2.4.  TPRO in ID3v2.4.
        /// </summary>
        /// <value>The produced notice.</value>
        public string ProducedNotice
        {
            get { return m_ProducedNotice.Value; }
            set { m_ProducedNotice.Value = value; }
        }

        /// <summary>
        /// Gets or sets the set subtitle. Intended for the subtitle of the 'part of
        /// a set' (for example, a specific disc in a multi disc album) this track belongs to.
        /// Only supported in ID3v2.4. TSST in ID3v2.4.
        /// </summary>
        /// <value>
        /// The set subtitle. Intended for the subtitle of the 'part of
        /// a set' (for example, a specific disc in a multi disc album) this track belongs to.
        /// Only supported in ID3v2.4. TSST in ID3v2.4.
        /// </value>
        public string SetSubtitle
        {
            get { return m_SetSubtitle.Value; }
            set { m_SetSubtitle.Value = value; }
        }

        /// <summary>
        /// Gets or sets the series category. Used for iTunes Podcasts.
        /// Only supported in ID3v2.3+. TCAT in ID3v2.3.
        /// </summary>
        /// <value>The series category. TCAT</value>
        public string PodcastSeriesCategory
        {
            get { return m_PodcastSeriesCategory.Value; }
            set { m_PodcastSeriesCategory.Value = value; }
        }

        /// <summary>
        /// Gets or sets the episode description. Used for iTunes Podcasts.
        /// Only supported in ID3v2.3+. TDES in ID3v2.3.
        /// </summary>
        /// <value>The episode description. TDES</value>
        public string PodcastEpisodeDescription
        {
            get { return m_PodcastEpisodeDescription.Value; }
            set { m_PodcastEpisodeDescription.Value = value; }
        }

        /// <summary>
        /// Gets or sets the episode URL. Used for iTunes Podcasts.
        /// Only supported in ID3v2.3+.  TGID in ID3v2.3.
        /// </summary>
        /// <value>
        /// The episode description. TGID
        /// </value>
        public string PodcastEpisodeUrl
        {
            get { return m_PodcastEpisodeUrl.Value; }
            set { m_PodcastEpisodeUrl.Value = value; }
        }

        /// <summary>
        /// Gets or sets the podcast feed URL. Used in iTunes Podcasts. WFED.
        /// </summary>
        /// <value>The podcast feed URL. WFED.</value>
        public string PodcastFeedUrl
        {
            get { return m_PodcastFeedUrl.Value; }
            set { m_PodcastFeedUrl.Value = value; }
        }

        /// <summary>
        /// Gets the ownership frame.  OWNE.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>
        /// The ownership frame.  OWNE.  Only supported in ID3v2.3 and ID3v2.4.
        /// </value>
        public IOwnership Ownership
        {
            get { return m_Ownership; }
        }

        /// <summary>
        /// Gets the seek next tag frame.  SEEK.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The seek next tag frame.  SEEK.  Only supported in ID3v2.4.</value>
        public ISeekNextTag SeekNextTag
        {
            get { return m_SeekNextTag; }
        }

        /// <summary>
        /// Gets the BindingList of signatures.  SIGN.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>
        /// The BindingList of signatures.  SIGN.  Only supported in ID3v2.4.
        /// </value>
        public BindingList<ISignature> SignatureList
        {
            get { return m_SignatureList; }
        }

        /// <summary>
        /// Gets the musician credits list.  TMCL.  Only supported in ID3v2.4.
        /// </summary>
        /// <value>The musician credits list.  TMCL.  Only supported in ID3v2.4.</value>
        public IMusicianCreditsList MusicianCreditsList
        {
            get { return m_MusicianCreditsList; }
        }

        /// <summary>
        /// Gets the BindingList of audio text frames.  ATXT.  Only supported in ID3v2.3 and ID3v2.4.
        /// </summary>
        /// <value>
        /// The BindingList of audio text frames.  ATXT.  Only supported in ID3v2.3 and ID3v2.4.
        /// </value>
        public BindingList<IAudioText> AudioTextList
        {
            get { return m_AudioTextList; }
        }
    }
}

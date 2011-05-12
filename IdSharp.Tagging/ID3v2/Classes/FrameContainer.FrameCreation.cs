using System;
using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames;
using IdSharp.Tagging.ID3v2.Frames.Lists;

namespace IdSharp.Tagging.ID3v2
{
    public abstract partial class FrameContainer : IFrameContainer
    {
        private readonly FrameBinder _frameBinder;

        private readonly List<UnknownFrame> _unknownFrames;

        //private List<IFrame> m_ReadFrames = new List<IFrame>();

        private readonly Dictionary<string, IFrame> _id3v24SingleOccurrenceFrames;
        private readonly Dictionary<string, IBindingList> _id3v24MultipleOccurrenceFrames;
        private readonly Dictionary<string, IFrame> _id3v23SingleOccurrenceFrames;
        private readonly Dictionary<string, IBindingList> _id3v23MultipleOccurrenceFrames;
        private readonly Dictionary<string, IFrame> _id3v22SingleOccurrenceFrames;
        private readonly Dictionary<string, IBindingList> _id3v22MultipleOccurrenceFrames;

        private readonly Dictionary<string, string> _id3v24FrameAliases;
        private readonly Dictionary<string, string> _id3v23FrameAliases;

        private readonly AttachedPictureBindingList m_AttachedPictureList;
        private readonly UserDefinedUrlBindingList m_UserDefinedUrlList;
        private readonly CommentsBindingList m_CommentsList;
        private readonly CommentsBindingList m_iTunesCommentsList;
        private readonly UrlBindingList m_ArtistUrlList;
        private readonly UrlBindingList m_CommercialInfoUrlList;
        private readonly UserDefinedTextBindingList m_UserDefinedTextList;
        private readonly RelativeVolumeAdjustmentBindingList m_RelativeVolumeAdjustmentList; // TODO: this is a single occurrence in 2.3 and 2.2
        private readonly UnsynchronizedLyricsBindingList m_UnsynchronizedLyricsList;
        private readonly GeneralEncapsulatedObjectBindingList m_GeneralEncapsulatedObjectList;
        private readonly UniqueFileIdentifierBindingList m_UniqueFileIdentifierList;
        private readonly PrivateFrameBindingList m_PrivateFrameList;
        private readonly PopularimeterBindingList m_PopularimeterList;
        private readonly TermsOfUseBindingList m_TermsOfUseList;
        private readonly LinkedInformationBindingList m_LinkedInformationList;
        private readonly CommercialBindingList m_CommercialInfoList;
        private readonly EncryptionMethodBindingList m_EncryptionMethodList;
        private readonly GroupIdentificationBindingList m_GroupIdentificationList;
        private readonly SignatureBindingList m_SignatureList;
        private readonly AudioEncryptionBindingList m_AudioEncryptionList;
        private readonly EncryptedMetaFrameBindingList m_EncryptedMetaFrameList;
        private readonly SynchronizedTextBindingList m_SynchronizedLyricsList;
        private readonly EqualizationListBindingList m_EqualizationList;
        private readonly AudioTextBindingList m_AudioTextList;

        private readonly TextFrame m_Genre;
        private readonly LanguageFrame m_Languages;
        private readonly MusicCDIdentifier m_MusicCDIdentifier;
        private readonly InvolvedPersonList m_InvolvedPersonList;
        private readonly RecommendedBufferSize m_RecommendedBufferSize;
        private readonly Ownership m_Ownership;
        private readonly PositionSynchronization m_PositionSynchronization;
        private readonly SeekNextTag m_SeekNextTag;
        private readonly MusicianCreditsList m_MusicianCreditsList;
        private readonly EventTiming m_EventTiming;
        private readonly MpegLookupTable m_MpegLookupTable;
        private readonly Reverb m_Reverb;
        private readonly SynchronizedTempoCodes m_SynchronizedTempoCodes;
        private readonly AudioSeekPointIndex m_AudioSeekPointIndex;
        private readonly PlayCount m_PlayCount;
        private readonly Podcast m_IsPodcast;

        private readonly IUrlFrame m_AudioFileUrl;
        private readonly IUrlFrame m_AudioSourceUrl;
        private readonly IUrlFrame m_InternetRadioStationUrl;
        private readonly IUrlFrame m_PaymentUrl;
        private readonly IUrlFrame m_CopyrightUrl;
        private readonly IUrlFrame m_PublisherUrl;

        private readonly TextFrame m_Title;
        private readonly TextFrame m_Album;
        private readonly TextFrame m_EncodedByWho;
        private readonly TextFrame m_Artist;
        private readonly TextFrame m_AlbumArtist;
        private readonly TextFrame m_Year;

        private readonly TextFrame m_Composer;
        private readonly TextFrame m_OriginalArtist;
        private readonly TextFrame m_Copyright;
        private readonly TextFrame m_RemixedBy;
        private readonly TextFrame m_Publisher;
        private readonly TextFrame m_InternetRadioStationName;
        private readonly TextFrame m_InternetRadioStationOwner;
        //private readonly TextFrame _accompaniment;
        private readonly TextFrame m_Conductor;
        private readonly TextFrame m_Lyricist;
        private readonly TextFrame m_OriginalLyricist;
        private readonly TextFrame m_TrackNumber;
        private readonly TextFrame m_BPM;
        private readonly TextFrame m_FileType;
        private readonly TextFrame m_DiscNumber;
        private readonly TextFrame m_ISRC;
        private readonly TextFrame m_EncoderSettings;
        private readonly TextFrame m_IsPartOfCompilation;
        private readonly TextFrame m_ReleaseTimestamp;
        private readonly TextFrame m_OriginalReleaseTimestamp;
        private readonly TextFrame m_RecordingTimestamp;
        private readonly TextFrame m_DateRecorded;
        private readonly TextFrame m_TimeRecorded;
        private readonly TextFrame m_PlaylistDelayMilliseconds;
        private readonly TextFrame m_InitialKey;
        private readonly TextFrame m_EncodingTimestamp;
        private readonly TextFrame m_TaggingTimestamp;
        private readonly TextFrame m_ContentGroup;
        private readonly TextFrame m_Mood;
        private readonly TextFrame m_LengthMilliseconds;
        private readonly TextFrame m_MediaType;
        private readonly TextFrame m_FileSizeExcludingTag;
        private readonly TextFrame m_OriginalReleaseYear;
        private readonly TextFrame m_OriginalSourceTitle;
        private readonly TextFrame m_OriginalFileName;
        private readonly TextFrame m_FileOwnerName;
        private readonly TextFrame m_RecordingDates;
        private readonly TextFrame m_Subtitle;
        private readonly TextFrame m_AlbumSortOrder;
        private readonly TextFrame m_ArtistSortOrder;
        private readonly TextFrame m_TitleSortOrder;
        private readonly TextFrame m_ProducedNotice;
        private readonly TextFrame m_SetSubtitle;
        private readonly TextFrame m_PodcastSeriesCategory;
        private readonly TextFrame m_PodcastEpisodeDescription;
        private readonly TextFrame m_PodcastEpisodeUrl;
        private readonly TextFrame m_PodcastFeedUrl;


        /// <summary>
        /// Initializes a new instance of the <see cref="FrameContainer"/> class.
        /// </summary>
        internal FrameContainer()
        {
            _frameBinder = new FrameBinder(this);
            _unknownFrames = new List<UnknownFrame>();

            _id3v24SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            _id3v24MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();
            _id3v23SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            _id3v23MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();
            _id3v22SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            _id3v22MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();

            _id3v24FrameAliases = new Dictionary<string, string>();
            _id3v23FrameAliases = new Dictionary<string, string>();

            // Binding lists
            m_AttachedPictureList = new AttachedPictureBindingList();
            m_UserDefinedUrlList = new UserDefinedUrlBindingList();
            m_CommentsList = new CommentsBindingList();
            m_iTunesCommentsList = new CommentsBindingList();
            m_CommercialInfoUrlList = new UrlBindingList("WCOM", "WCOM", "WCM");
            m_ArtistUrlList = new UrlBindingList("WOAR", "WOAR", "WAR");
            m_UserDefinedTextList = new UserDefinedTextBindingList();
            m_RelativeVolumeAdjustmentList = new RelativeVolumeAdjustmentBindingList();
            m_UnsynchronizedLyricsList = new UnsynchronizedLyricsBindingList();
            m_GeneralEncapsulatedObjectList = new GeneralEncapsulatedObjectBindingList();
            m_UniqueFileIdentifierList = new UniqueFileIdentifierBindingList();
            m_PrivateFrameList = new PrivateFrameBindingList();
            m_PopularimeterList = new PopularimeterBindingList();
            m_TermsOfUseList = new TermsOfUseBindingList();
            m_LinkedInformationList = new LinkedInformationBindingList();
            m_CommercialInfoList = new CommercialBindingList();
            m_EncryptionMethodList = new EncryptionMethodBindingList();
            m_GroupIdentificationList = new GroupIdentificationBindingList();
            m_SignatureList = new SignatureBindingList();
            m_AudioEncryptionList = new AudioEncryptionBindingList();
            m_EncryptedMetaFrameList = new EncryptedMetaFrameBindingList();
            m_SynchronizedLyricsList = new SynchronizedTextBindingList();
            m_EqualizationList = new EqualizationListBindingList();
            m_AudioTextList = new AudioTextBindingList();

            // Add binding lists to multiple occurence frames dictionary
            AddMultipleOccurrenceFrame("APIC", "APIC", "PIC", m_AttachedPictureList);
            AddMultipleOccurrenceFrame("WXXX", "WXXX", "WXX", m_UserDefinedUrlList);
            AddMultipleOccurrenceFrame("COMM", "COMM", "COM", m_CommentsList);
            AddMultipleOccurrenceFrame(null, null, null, m_iTunesCommentsList);
            AddMultipleOccurrenceFrame("WCOM", "WCOM", "WCM", m_CommercialInfoUrlList);
            //"CommercialInfoUrl", new MethodInvoker(ValidateCommercialInfoUrl)); // TODO
            AddMultipleOccurrenceFrame("WOAR", "WOAR", "WAR", m_ArtistUrlList);
            AddMultipleOccurrenceFrame("TXXX", "TXXX", "TXX", m_UserDefinedTextList);
            AddMultipleOccurrenceFrame("RVA2", "RVAD", "RVA", m_RelativeVolumeAdjustmentList);
            AddMultipleOccurrenceFrame("USLT", "USLT", "ULT", m_UnsynchronizedLyricsList);
            AddMultipleOccurrenceFrame("GEOB", "GEOB", "GEO", m_GeneralEncapsulatedObjectList);
            AddMultipleOccurrenceFrame("UFID", "UFID", "UFI", m_UniqueFileIdentifierList);
            AddMultipleOccurrenceFrame("PRIV", "PRIV", null, m_PrivateFrameList);
            AddMultipleOccurrenceFrame("POPM", "POPM", "POP", m_PopularimeterList);
            AddMultipleOccurrenceFrame("USER", "USER", null, m_TermsOfUseList);
            AddMultipleOccurrenceFrame("LINK", "LINK", "LNK", m_LinkedInformationList);
            //"ArtistUrl", new MethodInvoker(ValidateArtistUrl)); // TODO
            AddMultipleOccurrenceFrame("AENC", "AENC", "CRA", m_AudioEncryptionList);
            AddMultipleOccurrenceFrame(null, null, "CRM", m_EncryptedMetaFrameList); // replaced in ID3v2.3 and ID3v2.4 with encryptable frames
            AddMultipleOccurrenceFrame("SYLT", "SYLT", "SLT", m_SynchronizedLyricsList);
            AddMultipleOccurrenceFrame("EQU2", "EQUA", "EQU", m_EqualizationList); // todo: not a multi-occur frame in 2.2/2.3
            AddMultipleOccurrenceFrame("COMR", "COMR", null, m_CommercialInfoList); // todo: not a multi-occur in 2.3
            AddMultipleOccurrenceFrame("ENCR", "ENCR", null, m_EncryptionMethodList);
            AddMultipleOccurrenceFrame("GRID", "GRID", null, m_GroupIdentificationList);
            AddMultipleOccurrenceFrame("SIGN", "SIGN", null, m_SignatureList); // todo: not in ID3v2.3
            AddMultipleOccurrenceFrame("ATXT", "ATXT", null, m_AudioTextList);

            // Text frames
            m_Title = CreateTextFrame("TIT2", "TIT2", "TT2", "Title", null);
            m_Album = CreateTextFrame("TALB", "TALB", "TAL", "Album", null);
            m_EncodedByWho = CreateTextFrame("TENC", "TENC", "TEN", "EncodedByWho", null);
            m_Artist = CreateTextFrame("TPE1", "TPE1", "TP1", "Artist", null);
            m_AlbumArtist = CreateTextFrame("TPE2", "TPE2", "TP2", "AlbumArtist", null);
            // Note: TYER is not a valid frame in ID3v2.4, but many ID3v2.4 tags contain this frame anyway.
            // It's here to allow reading, but should not be written back (use TDRL instead).
            m_Year = CreateTextFrame("TYER", "TYER", "TYE", "Year", ValidateYear);
            // Note: TDAT is not a valid frame in ID3v2.4, but many ID3v2.4 tags contain this frame anyway.
            // It's here to allow reading, but should not be written back (use TDRC instead).
            m_DateRecorded = CreateTextFrame("TDAT", "TDAT", "TDA", "DateRecorded", ValidateDateRecorded);
            // Note: TIME is not a valid frame in ID3v2.4, but many ID3v2.4 tags contain this frame anyway.
            // It's here to allow reading, but should not be written back (use TDRC instead).
            m_TimeRecorded = CreateTextFrame("TIME", "TIME", "TIM", "TimeRecorded", ValidateTimeRecorded);
            m_Genre = CreateTextFrame("TCON", "TCON", "TCO", "Genre", null);
            m_Composer = CreateTextFrame("TCOM", "TCOM", "TCM", "Composer", null);
            m_OriginalArtist = CreateTextFrame("TOPE", "TOPE", "TOA", "OriginalArtist", null);
            m_Copyright = CreateTextFrame("TCOP", "TCOP", "TCR", "Copyright", ValidateCopyright);
            m_RemixedBy = CreateTextFrame("TPE4", "TPE4", "TP4", "RemixedBy", null);
            m_Publisher = CreateTextFrame("TPUB", "TPUB", "TPB", "Publisher", null);
            m_InternetRadioStationName = CreateTextFrame("TRSN", "TRSN", null, "InternetRadioStationName", null);
            m_InternetRadioStationOwner = CreateTextFrame("TRSO", "TRSO", null, "InternetRadioStationOwner", null);
            //m_Accompaniment = CreateTextFrame("TPE2", "TPE2", "TP2", "Accompaniment", null);
            m_Conductor = CreateTextFrame("TPE3", "TPE3", "TP3", "Conductor", null);
            m_Lyricist = CreateTextFrame("TEXT", "TEXT", "TXT", "Lyricist", null);
            m_OriginalLyricist = CreateTextFrame("TOLY", "TOLY", "TOL", "OriginalLyricist", null);
            m_TrackNumber = CreateTextFrame("TRCK", "TRCK", "TRK", "TrackNumber", ValidateTrackNumber);
            m_BPM = CreateTextFrame("TBPM", "TBPM", "TBP", "BPM", ValidateBPM);
            m_FileType = CreateTextFrame("TFLT", "TFLT", "TFT", "FileType", null);
            m_DiscNumber = CreateTextFrame("TPOS", "TPOS", "TPA", "DiscNumber", ValidateDiscNumber);
            m_EncoderSettings = CreateTextFrame("TSSE", "TSSE", "TSS", "EncoderSettings", null);
            m_ISRC = CreateTextFrame("TSRC", "TSRC", "TRC", "ISRC", ValidateISRC);
            // Note: TCMP/TCP is an unofficial frame.  Used by iTunes and other taggers.
            m_IsPartOfCompilation = CreateTextFrame("TCMP", "TCMP", "TCP", "IsPartOfCompilation", null);
            // TDRL is not technically supported in ID3v2.3
            m_ReleaseTimestamp = CreateTextFrame("TDRL", "TDRL", null, "ReleaseTimestamp", ValidateReleaseTimestamp);
            // TDRC is not technically supported in ID3v2.3
            m_RecordingTimestamp = CreateTextFrame("TDRC", "TDRC", null, "RecordingTimestamp", ValidateRecordingTimestamp);
            // TDOR is not technically supported in ID3v2.3
            m_OriginalReleaseTimestamp = CreateTextFrame("TDOR", "TDOR", null, "OriginalReleaseTimestamp", null/*todo:new MethodInvoker(ValidateOriginalReleaseTimestamp)*/);
            m_PlaylistDelayMilliseconds = CreateTextFrame("TDLY", "TDLY", "TDY", "PlaylistDelayMilliseconds", null);
            m_InitialKey = CreateTextFrame("TKEY", "TKEY", "TKE", "InitialKey", null/*TODO: new MethodInvoker(ValidateInitialKey)*/);
            // TDEN is not technically supported in ID3v2.3
            m_EncodingTimestamp = CreateTextFrame("TDEN", "TDEN", null, "EncodingTimestamp", null/*TODO*/);
            // TDTG is not technically supported in ID3v2.3
            m_TaggingTimestamp = CreateTextFrame("TDTG", "TDTG", null, "TaggingTimestamp", null/*TODO*/);
            m_ContentGroup = CreateTextFrame("TIT1", "TIT1", "TT1", "ContentGroup", null);
            // TMOO is not technically supported in ID3v2.3
            m_Mood = CreateTextFrame("TMOO", "TMOO", null, "Mood", null);
            m_LengthMilliseconds = CreateTextFrame("TLEN", "TLEN", "TLE", "LengthMilliseconds", null);
            m_MediaType = CreateTextFrame("TMED", "TMED", "TMT", "MediaType", null);
            // TODO: technically not supported in ID3v2.4, but probably written by some impl. anyway
            m_FileSizeExcludingTag = CreateTextFrame(null, "TSIZ", "TSI", "FileSizeExcludingTag", null);
            // Not technically in ID3v2.4
            m_OriginalReleaseYear = CreateTextFrame("TORY", "TORY", "TOR", "OriginalReleaseYear", null/*TODO*/);
            m_OriginalSourceTitle = CreateTextFrame("TOAL", "TOAL", "TOT", "OriginalSourceTitle", null);
            m_OriginalFileName = CreateTextFrame("TOFN", "TOFN", "TOF", "OriginalFileName", null);
            m_FileOwnerName = CreateTextFrame("TOWN", "TOWN", null, "FileOwnerName", null);
            // Not technically in ID3v2.4
            m_RecordingDates = CreateTextFrame("TRDA", "TRDA", "TRD", "RecordingDates", null/*TODO*/);
            m_Subtitle = CreateTextFrame("TIT3", "TIT3", "TT3", "Subtitle", null);
            // Technically only supported in ID3v2.4, but some ID3v2.3 implementations use this frame
            m_AlbumSortOrder = CreateTextFrame("TSOA", "TSOA", null, "AlbumSortOrder", null);
            // Technically only supported in ID3v2.4, but some ID3v2.3 implementations use this frame
            m_ArtistSortOrder = CreateTextFrame("TSOP", "TSOP", null, "ArtistSortOrder", null);
            // Technically only supported in ID3v2.4, but some ID3v2.3 implementations use this frame
            m_TitleSortOrder = CreateTextFrame("TSOT", "TSOT", null, "TitleSortOrder", null);
            // Technically only supported in ID3v2.4, but some ID3v2.3 implementations use this frame
            m_ProducedNotice = CreateTextFrame("TPRO", "TPRO", null, "ProducedNotice", null/*todo - same as copyright validation*/);
            // Technically only supported in ID3v2.4, but some ID3v2.3 implementations use this frame
            m_SetSubtitle = CreateTextFrame("TSST", "TSST", null, "SetSubtitle", null);
            m_PodcastSeriesCategory = CreateTextFrame("TCAT", "TCAT", null, "SeriesCategory", null);
            m_PodcastEpisodeDescription = CreateTextFrame("TDES", "TDES", null, "EpisodeDescription", null);
            m_PodcastEpisodeUrl = CreateTextFrame("TGID", "TGID", null, "EpisodeUrl", null);
            m_PodcastFeedUrl = CreateTextFrame("WFED", "WFED", null, "PodcastFeedUrl", null);

            m_PositionSynchronization = CreatePositionSynchronizationFrame("POSS", "POSS", null, "PositionSynchronization", null);
            m_Ownership = CreateOwnershipFrame("OWNE", "OWNE", null, "Ownership", null);
            m_RecommendedBufferSize = CreateRecommendedBufferSizeFrame("RBUF", "RBUF", "BUF", "RecommendedBufferSize", null /*todo*/);
            m_InvolvedPersonList = CreateInvolvedPersonListFrame("TIPL", "IPLS", "IPL", "InvolvedPersonList", null/*TODO - needs validation?*/);
            m_Languages = CreateLanguageFrame("TLAN", "TLAN", "TLA", "Languages", null);
            m_MusicCDIdentifier = CreateMusicCDIdentifierFrame("MCDI", "MCDI", "MCI", "MusicCDIdentifier", null/*TODO?*/);
            m_EventTiming = CreateEventTimingFrame("ETCO", "ETCO", "ETC", "EventTiming", null/*TODO - needs validation?*/);
            m_MpegLookupTable = CreateMpegLookupTableFrame("MLLT", "MLLT", "MLL", "MpegLookupTable", null);
            m_Reverb = CreateReverbFrame("RVRB", "RVRB", "REV", "Reverb", null);
            m_SynchronizedTempoCodes = CreateSynchronizedTempoCodesFrame("SYTC", "SYTC", "STC", "SynchronizedTempoCodeList", null);
            m_SeekNextTag = CreateSeekFrame("SEEK", "SEEK", null, "SeekNextTag", null);
            // Technically only supported in ID3v2.4, but some ID3v2.3 implementations use this frame
            m_MusicianCreditsList = CreateMusicianCreditsListFrame("TMCL", "TMCL", null, "MusicianCreditsList", null);
            m_AudioSeekPointIndex = CreateAudioSeekPointIndexFrame("ASPI", "ASPI", null, "AudioSeekPointIndex", null);
            m_PlayCount = CreateFrame<PlayCount>("PCNT", "PCNT", "CNT", "PlayCount");
            m_IsPodcast = CreateFrame<Podcast>("PCST", "PCST", "PCS", "Podcast");
            
            // TODO: TYER->TDRL, TDAT,TIME->TDRC (in setters, not here)

            // URL frames
            m_CopyrightUrl = CreateUrlFrame("WCOP", "WCOP", "WCP", "CopyrightUrl", ValidateCopyrightUrl);
            m_AudioFileUrl = CreateUrlFrame("WOAF", "WOAF", "WAF", "AudioFileUrl", ValidateAudioFileUrl);
            m_AudioSourceUrl = CreateUrlFrame("WOAS", "WOAS", "WAS", "AudioSourceUrl", ValidateAudioSourceUrl);
            m_InternetRadioStationUrl = CreateUrlFrame("WORS", "WORS", null, "InternetRadioStationUrl", ValidateInternetRadioStationUrl);
            m_PaymentUrl = CreateUrlFrame("WPAY", "WPAY", null, "PaymentUrl", ValidatePaymentUrl);
            m_PublisherUrl = CreateUrlFrame("WPUB", "WPUB", "WPB", "PublisherUrl", ValidatePublisherUrl);

            // Frame aliases - handles incorrectly named frames

            // ID3v2.4 (Bad frame, Good frame)
            _id3v24FrameAliases.Add("RVAD", "RVA2");
            _id3v24FrameAliases.Add("IPLS", "TIPL");
            _id3v24FrameAliases.Add("EQUA", "EQU2");
            // ID3v2.3 (Bad frame, Good frame)
            _id3v23FrameAliases.Add("RVA2", "RVAD");
            _id3v23FrameAliases.Add("TIPL", "IPLS");
            _id3v23FrameAliases.Add("EQU2", "EQUA");
        }

        private T CreateFrame<T>(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property)
            where T : IFrame, new()
        {
            T frame = new T();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, frame, "TODO", property, null);
            return frame;
        }

        /*private PlayCount CreatePlayCountFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            PlayCount audioSeekPointIndex = new PlayCount();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, audioSeekPointIndex, "TODO", property, validator);
            return audioSeekPointIndex;
        }*/

        private AudioSeekPointIndex CreateAudioSeekPointIndexFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            AudioSeekPointIndex audioSeekPointIndex = new AudioSeekPointIndex();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, audioSeekPointIndex, "TODO", property, validator);
            return audioSeekPointIndex;
        }

        private SynchronizedTempoCodes CreateSynchronizedTempoCodesFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            SynchronizedTempoCodes synchronizedTempoCodes = new SynchronizedTempoCodes();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, synchronizedTempoCodes, "TODO", property, validator);
            return synchronizedTempoCodes;
        }

        private Reverb CreateReverbFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            Reverb reverb = new Reverb();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, reverb, "TODO", property, validator);
            return reverb;
        }

        private MpegLookupTable CreateMpegLookupTableFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            MpegLookupTable mpegLookupTable = new MpegLookupTable();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, mpegLookupTable, "TODO", property, validator);
            return mpegLookupTable;
        }

        private EventTiming CreateEventTimingFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            EventTiming eventTiming = new EventTiming();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, eventTiming, "TODO", property, validator);
            return eventTiming;
        }

        private MusicianCreditsList CreateMusicianCreditsListFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            MusicianCreditsList musicianCreditsList = new MusicianCreditsList();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, musicianCreditsList, "TODO", property, validator);
            return musicianCreditsList;
        }

        private SeekNextTag CreateSeekFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            SeekNextTag seekNextTag = new SeekNextTag();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, seekNextTag, "TODO", property, validator);
            return seekNextTag;
        }

        private RecommendedBufferSize CreateRecommendedBufferSizeFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            RecommendedBufferSize recommendedBufferSize = new RecommendedBufferSize();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, recommendedBufferSize, "TODO", property, validator);
            return recommendedBufferSize;
        }

        private Ownership CreateOwnershipFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            Ownership ownership = new Ownership();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, ownership, "TODO", property, validator);
            return ownership;
        }

        private PositionSynchronization CreatePositionSynchronizationFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            PositionSynchronization positionSynchronization = new PositionSynchronization();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, positionSynchronization, "TODO", property, validator);
            return positionSynchronization;
        }

        private InvolvedPersonList CreateInvolvedPersonListFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            InvolvedPersonList involvedPersonList = new InvolvedPersonList();
            Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, involvedPersonList, "TODO", property, validator);
            return involvedPersonList;
        }

        private void Bind(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, IFrame frame, string frameProperty, string property, Action validator)
        {
            _frameBinder.Bind(frame, frameProperty, property, validator);

            if (id3v24FrameID != null) _id3v24SingleOccurrenceFrames.Add(id3v24FrameID, frame);
            if (id3v23FrameID != null) _id3v23SingleOccurrenceFrames.Add(id3v23FrameID, frame);
            if (id3v22FrameID != null) _id3v22SingleOccurrenceFrames.Add(id3v22FrameID, frame);
        }

        private MusicCDIdentifier CreateMusicCDIdentifierFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            MusicCDIdentifier musicCDIdentifier = new MusicCDIdentifier();
            _frameBinder.Bind(musicCDIdentifier, "TOC", property, validator);

            if (id3v24FrameID != null) _id3v24SingleOccurrenceFrames.Add(id3v24FrameID, musicCDIdentifier);
            if (id3v23FrameID != null) _id3v23SingleOccurrenceFrames.Add(id3v23FrameID, musicCDIdentifier);
            if (id3v22FrameID != null) _id3v22SingleOccurrenceFrames.Add(id3v22FrameID, musicCDIdentifier);

            return musicCDIdentifier;
        }

        private LanguageFrame CreateLanguageFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            LanguageFrame languageFrame = new LanguageFrame();
            _frameBinder.Bind(languageFrame, "Items", property, validator); // todo: may need work, Items is a BindingList

            if (id3v24FrameID != null) _id3v24SingleOccurrenceFrames.Add(id3v24FrameID, languageFrame);
            if (id3v23FrameID != null) _id3v23SingleOccurrenceFrames.Add(id3v23FrameID, languageFrame);
            if (id3v22FrameID != null) _id3v22SingleOccurrenceFrames.Add(id3v22FrameID, languageFrame);

            return languageFrame;
        }

        private void AddMultipleOccurrenceFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, IBindingList bindingList)
        {
            if (id3v24FrameID != null) _id3v24MultipleOccurrenceFrames.Add(id3v24FrameID, bindingList);
            if (id3v23FrameID != null) _id3v23MultipleOccurrenceFrames.Add(id3v23FrameID, bindingList);
            if (id3v22FrameID != null) _id3v22MultipleOccurrenceFrames.Add(id3v22FrameID, bindingList);
        }

        private TextFrame CreateTextFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            TextFrame textFrame = new TextFrame(id3v24FrameID, id3v23FrameID, id3v22FrameID);
            _frameBinder.Bind(textFrame, "Value", property, validator);

            if (id3v24FrameID != null) _id3v24SingleOccurrenceFrames.Add(id3v24FrameID, textFrame);
            if (id3v23FrameID != null) _id3v23SingleOccurrenceFrames.Add(id3v23FrameID, textFrame);
            if (id3v22FrameID != null) _id3v22SingleOccurrenceFrames.Add(id3v22FrameID, textFrame);

            return textFrame;
        }

        private UrlFrame CreateUrlFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            UrlFrame urlFrame = new UrlFrame(id3v24FrameID, id3v23FrameID, id3v22FrameID);
            _frameBinder.Bind(urlFrame, "Value", property, validator);

            if (id3v24FrameID != null) _id3v24SingleOccurrenceFrames.Add(id3v24FrameID, urlFrame);
            if (id3v23FrameID != null) _id3v23SingleOccurrenceFrames.Add(id3v23FrameID, urlFrame);
            if (id3v22FrameID != null) _id3v22SingleOccurrenceFrames.Add(id3v22FrameID, urlFrame);

            return urlFrame;
        }

        private Dictionary<string, IFrame> GetSingleOccurrenceFrames(ID3v2TagVersion tagVersion)
        {
            // Arranged in order of commonness
            if (tagVersion == ID3v2TagVersion.ID3v23)
                return _id3v23SingleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v22)
                return _id3v22SingleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v24)
                return _id3v24SingleOccurrenceFrames;
            else
                throw new ArgumentException("Unknown ID3v2 tag version");
        }

        private Dictionary<string, IBindingList> GetMultipleOccurrenceFrames(ID3v2TagVersion tagVersion)
        {
            // Arranged in order of commonness
            if (tagVersion == ID3v2TagVersion.ID3v23)
                return _id3v23MultipleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v22)
                return _id3v22MultipleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v24)
                return _id3v24MultipleOccurrenceFrames;
            else
                throw new ArgumentException("Unknown ID3v2 tag version");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2.Frames;
using IdSharp.Tagging.ID3v2.Frames.Lists;
using IdSharp.Tagging.Utils;
using IdSharp.Tagging.Utils.Events;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// ID3v2 Frame Container
    /// </summary>
    public abstract class FrameContainer : IFrameContainer
    {
        #region <<< Public Events >>>

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when invalid data is assigned to a property.
        /// </summary>
        public event InvalidDataEventHandler InvalidData;

        #endregion <<< Public Events >>>

        #region <<< Private Fields >>>

        private readonly FrameBinder m_FrameBinder;

        private readonly List<UnknownFrame> m_UnknownFrames;

        //private List<IFrame> m_ReadFrames = new List<IFrame>();

        private readonly Dictionary<string, IFrame> m_ID3v24SingleOccurrenceFrames;
        private readonly Dictionary<string, IBindingList> m_ID3v24MultipleOccurrenceFrames;
        private readonly Dictionary<string, IFrame> m_ID3v23SingleOccurrenceFrames;
        private readonly Dictionary<string, IBindingList> m_ID3v23MultipleOccurrenceFrames;
        private readonly Dictionary<string, IFrame> m_ID3v22SingleOccurrenceFrames;
        private readonly Dictionary<string, IBindingList> m_ID3v22MultipleOccurrenceFrames;

        private readonly Dictionary<string, string> m_ID3v24FrameAliases;
        private readonly Dictionary<string, string> m_ID3v23FrameAliases;

        private readonly AttachedPictureBindingList m_AttachedPictureList;
        private readonly UserDefinedUrlBindingList m_UserDefinedUrlList;
        private readonly CommentsBindingList m_CommentsList;
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
        //private readonly TextFrame m_Accompaniment;
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

        #endregion <<< Private Fields >>>

        #region <<< Constructor >>>

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameContainer"/> class.
        /// </summary>
        internal FrameContainer()
        {
            m_FrameBinder = new FrameBinder(this);
            m_UnknownFrames = new List<UnknownFrame>();

            m_ID3v24SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            m_ID3v24MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();
            m_ID3v23SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            m_ID3v23MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();
            m_ID3v22SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            m_ID3v22MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();

            m_ID3v24FrameAliases = new Dictionary<string, string>();
            m_ID3v23FrameAliases = new Dictionary<string, string>();

            // Binding lists
            m_AttachedPictureList = new AttachedPictureBindingList();
            m_UserDefinedUrlList = new UserDefinedUrlBindingList();
            m_CommentsList = new CommentsBindingList();
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
            m_ID3v24FrameAliases.Add("RVAD", "RVA2");
            m_ID3v24FrameAliases.Add("IPLS", "TIPL");
            m_ID3v24FrameAliases.Add("EQUA", "EQU2");
            // ID3v2.3 (Bad frame, Good frame)
            m_ID3v23FrameAliases.Add("RVA2", "RVAD");
            m_ID3v23FrameAliases.Add("TIPL", "IPLS");
            m_ID3v23FrameAliases.Add("EQU2", "EQUA");
        }

        #endregion <<< Constructor >>>

        #region <<< Private Methods >>>

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
            m_FrameBinder.Bind(frame, frameProperty, property, validator);

            if (id3v24FrameID != null) m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, frame);
            if (id3v23FrameID != null) m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, frame);
            if (id3v22FrameID != null) m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, frame);
        }

        private MusicCDIdentifier CreateMusicCDIdentifierFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            MusicCDIdentifier musicCDIdentifier = new MusicCDIdentifier();
            m_FrameBinder.Bind(musicCDIdentifier, "TOC", property, validator);

            if (id3v24FrameID != null) m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, musicCDIdentifier);
            if (id3v23FrameID != null) m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, musicCDIdentifier);
            if (id3v22FrameID != null) m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, musicCDIdentifier);

            return musicCDIdentifier;
        }

        private LanguageFrame CreateLanguageFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            LanguageFrame languageFrame = new LanguageFrame();
            m_FrameBinder.Bind(languageFrame, "Items", property, validator); // todo: may need work, Items is a BindingList

            if (id3v24FrameID != null) m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, languageFrame);
            if (id3v23FrameID != null) m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, languageFrame);
            if (id3v22FrameID != null) m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, languageFrame);

            return languageFrame;
        }

        private void AddMultipleOccurrenceFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, IBindingList bindingList)
        {
            if (id3v24FrameID != null) m_ID3v24MultipleOccurrenceFrames.Add(id3v24FrameID, bindingList);
            if (id3v23FrameID != null) m_ID3v23MultipleOccurrenceFrames.Add(id3v23FrameID, bindingList);
            if (id3v22FrameID != null) m_ID3v22MultipleOccurrenceFrames.Add(id3v22FrameID, bindingList);
        }

        private TextFrame CreateTextFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            TextFrame textFrame = new TextFrame(id3v24FrameID, id3v23FrameID, id3v22FrameID);
            m_FrameBinder.Bind(textFrame, "Value", property, validator);

            if (id3v24FrameID != null) m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, textFrame);
            if (id3v23FrameID != null) m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, textFrame);
            if (id3v22FrameID != null) m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, textFrame);

            return textFrame;
        }

        private UrlFrame CreateUrlFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, Action validator)
        {
            UrlFrame urlFrame = new UrlFrame(id3v24FrameID, id3v23FrameID, id3v22FrameID);
            m_FrameBinder.Bind(urlFrame, "Value", property, validator);

            if (id3v24FrameID != null) m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, urlFrame);
            if (id3v23FrameID != null) m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, urlFrame);
            if (id3v22FrameID != null) m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, urlFrame);

            return urlFrame;
        }

        private Dictionary<string, IFrame> GetSingleOccurrenceFrames(ID3v2TagVersion tagVersion)
        {
            // Arranged in order of commonness
            if (tagVersion == ID3v2TagVersion.ID3v23)
                return m_ID3v23SingleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v22)
                return m_ID3v22SingleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v24)
                return m_ID3v24SingleOccurrenceFrames;
            else
                throw new ArgumentException("Unknown ID3v2 tag version");
        }

        private Dictionary<string, IBindingList> GetMultipleOccurrenceFrames(ID3v2TagVersion tagVersion)
        {
            // Arranged in order of commonness
            if (tagVersion == ID3v2TagVersion.ID3v23)
                return m_ID3v23MultipleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v22)
                return m_ID3v22MultipleOccurrenceFrames;
            else if (tagVersion == ID3v2TagVersion.ID3v24)
                return m_ID3v24MultipleOccurrenceFrames;
            else
                throw new ArgumentException("Unknown ID3v2 tag version");
        }

        #region <<< Validation >>>

        private void ValidateTimeRecorded()
        {
            // TODO
        }

        private void ValidateDateRecorded()
        {
            // TODO
        }

        private void ValidateRecordingTimestamp()
        {
            string value = RecordingTimestamp;
            if (value != null)
            {
                // yyyy-MM-dd
                if (value.Length >= 10)
                {
                    // MMdd
                    DateRecorded = value.Substring(5, 2) + value.Substring(8, 2);
                }
                // yyyy-MM
                else if (value.Length >= 7)
                {
                    // MMdd
                    DateRecorded = value.Substring(6, 2) + "00";
                }
                else
                {
                    DateRecorded = null;
                }

                // yyyy-MM-ddTHH:mm
                if (value.Length >= 16)
                {
                    // HHmm
                    TimeRecorded = value.Substring(11, 2) + value.Substring(14, 2);
                }
                // yyyy-MM-ddTHH
                else if (value.Length >= 13)
                {
                    // HHmm
                    TimeRecorded = value.Substring(11, 2) + "00";
                }
                else
                {
                    TimeRecorded = null;
                }

                // yyyy-MM-ddTHH:mm:ss
                if (value.Length >= 19)
                {
                    /* Nowhere to store seconds */
                }

                // TODO: Fire warnings on invalid data
            }
            else
            {
                DateRecorded = null;
                TimeRecorded = null;
            }
        }

        private void ValidateReleaseTimestamp()
        {
            string value = ReleaseTimestamp;
            if (value != null)
            {
                // yyyy
                if (value.Length >= 4)
                {
                    Year = value.Substring(0, 4);
                }
                else
                {
                    Year = null;
                }

                // TODO: Fire warnings on invalid data
            }
            else
            {
                Year = null;
            }
        }

        private void ValidateISRC()
        {
            string value = ISRC;
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length != 12)
                {
                    FireWarning("ISRC", "ISRC value should be 12 characters in length");
                }
            }
        }

        private void ValidateBPM()
        {
            string value = BPM;

            if (!string.IsNullOrEmpty(value))
            {
                uint tmpBPM;
                if (!uint.TryParse(value, out tmpBPM))
                {
                    FireWarning("BPM", "Value should be numeric");
                }
            }
        }

        private void ValidateTrackNumber()
        {
            // NOTE: Track Num is commonly used as a show number in the podcast world. Given where we are with podcasting as this date, 
            // many podcast shows have now exceeded 255.
            ValidateFractionValue("TrackNumber", TrackNumber, "Value should contain either the track number or track number/total tracks in the format ## or ##/##\nExample: 1 or 1/14");
        }

        private void ValidateDiscNumber()
        {
            ValidateFractionValue("DiscNumber", DiscNumber, "Value should contain either the disc number or disc number/total discs in the format ## or ##/##\nExample: 1 or 1/2");
        }

        private void ValidateFractionValue(string propertyName, string value, string message)
        {
            if (!string.IsNullOrEmpty(value))
            {
                bool isValid = true;

                string[] valueArray = value.Split('/');
                if (valueArray.Length > 2)
                {
                    isValid = false;
                }
                else
                {
                    int i = 0;
                    uint extractedFirstPart = 0;
                    uint extractedSecondPart = 0;
                    foreach (string tmpValuePart in valueArray)
                    {
                        uint tmpIntValue;
                        if (!uint.TryParse(tmpValuePart, out tmpIntValue))
                        {
                            isValid = false;
                            break;
                        }
                        else
                        {
                            if (i == 0) extractedFirstPart = tmpIntValue;
                            else if (i == 1) extractedSecondPart = tmpIntValue;
                        }
                        i++;
                    }

                    // If first # is 0
                    if (extractedFirstPart == 0)
                        isValid = false;
                    // If ##/## used
                    else if (i == 2)
                    {
                        // If first partis greater than second part
                        if (extractedFirstPart > extractedSecondPart)
                        {
                            isValid = false;
                        }
                    }
                }

                if (isValid == false)
                {
                    FireWarning(propertyName, message);
                }
            }
        }

        private void ValidateCopyright()
        {
            string value = Copyright;

            if (!string.IsNullOrEmpty(value))
            {
                bool isValid = false;
                if (value.Length >= 6)
                {
                    string yearString = value.Substring(0, 4);
                    int year;
                    if (int.TryParse(yearString, out year) && year >= 1000 && year <= 9999)
                    {
                        if (value[4] == ' ') isValid = true;
                    }
                }

                if (!isValid)
                {
                    FireWarning("Copyright", string.Format("The copyright field should begin with a year followed by the copyright owner{0}Example: 2007 Sony Records", Environment.NewLine));
                }
            }
        }

        private void ValidateYear()
        {
            string value = Year;

            if (!string.IsNullOrEmpty(value))
            {
                int tmpYear;
                if (!int.TryParse(value, out tmpYear) || tmpYear < 1000 || tmpYear >= 10000)
                {
                    FireWarning("Year", string.Format("The year field should be a 4 digit number{0}Example: 2007", Environment.NewLine));
                }
            }
        }

        private void ValidateUrl(string propertyName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
                {
                    FireWarning(propertyName, "Value is not a valid relative or absolute URL");
                }
            }
        }

        private void ValidatePublisherUrl()
        {
            ValidateUrl("PublisherUrl", PublisherUrl);
        }

        private void ValidateCopyrightUrl()
        {
            ValidateUrl("CopyrightUrl", CopyrightUrl);
        }

        private void ValidatePaymentUrl()
        {
            ValidateUrl("PaymentUrl", PaymentUrl);
        }

        // TODO: Add to UrlBindingList
        /*private void ValidateArtistUrl()
        {
            //ValidateUrl("ArtistUrl", this.ArtistUrl);
        }

        private void ValidateCommercialInfoUrl()
        {
            //ValidateUrl("CommercialInfoUrl", this.CommercialInfoUrl);
        }*/

        private void ValidateInternetRadioStationUrl()
        {
            ValidateUrl("InternetRadioStationUrl", InternetRadioStationUrl);
        }

        private void ValidateAudioSourceUrl()
        {
            ValidateUrl("AudioSourceUrl", AudioSourceUrl);
        }

        private void ValidateAudioFileUrl()
        {
            ValidateUrl("AudioFileUrl", AudioFileUrl);
        }

        #endregion <<< Validation >>>

        #endregion <<< Private Methods >>>

        #region <<< Protected Methods >>>

        /// <summary>
        /// Fires the InvalidData event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="message">The message.</param>
        protected void FireWarning(string propertyName, string message)
        {
            // TODO - Add to log
            InvalidDataEventHandler tmpInvalidDataEventHandler = InvalidData;
            if (tmpInvalidDataEventHandler != null)
            {
                tmpInvalidDataEventHandler(this, new InvalidDataEventArgs(propertyName, message));
            }
        }

        #endregion <<< Protected Methods >>>

        #region <<< Public Methods >>>

        internal void Read(Stream stream, ID3v2TagVersion tagVersion, TagReadingInfo tmpTagReadingInfo, int tmpReadUntil, int tmpFrameIDSize)
        {
            Dictionary<string, IBindingList> tmpMultipleOccurrenceFrames = GetMultipleOccurrenceFrames(tagVersion);
            Dictionary<string, IFrame> tmpSingleOccurrenceFrames = GetSingleOccurrenceFrames(tagVersion);

            int tmpBytesRead = 0;
            while (tmpBytesRead < tmpReadUntil)
            {
                byte[] tmpFrameIDBytes = stream.Read(tmpFrameIDSize);

                // If character is not a letter or number, padding reached, audio began,
                // or otherwise the frame is not readable
                if (tmpFrameIDSize == 4)
                {
                    if (tmpFrameIDBytes[0] < 0x30 || tmpFrameIDBytes[0] > 0x5A ||
                        tmpFrameIDBytes[1] < 0x30 || tmpFrameIDBytes[1] > 0x5A ||
                        tmpFrameIDBytes[2] < 0x30 || tmpFrameIDBytes[2] > 0x5A ||
                        tmpFrameIDBytes[3] < 0x30 || tmpFrameIDBytes[3] > 0x5A)
                    {
                        // TODO: Try to keep reading and look for a valid frame
                        if (tmpFrameIDBytes[0] != 0 && tmpFrameIDBytes[0] != 0xFF)
                        {
                            string msg = string.Format("Out of range FrameID - 0x{0:X}|0x{1:X}|0x{2:X}|0x{3:X}",
                                tmpFrameIDBytes[0], tmpFrameIDBytes[1], tmpFrameIDBytes[2], tmpFrameIDBytes[3]);
                            if (ByteUtils.ISO88591GetString(tmpFrameIDBytes) != "MP3e")
                            {
                                string tmpBadFrameID = ByteUtils.ISO88591GetString(tmpFrameIDBytes).TrimEnd('\0');
                                Trace.WriteLine(msg + " - " + tmpBadFrameID);
                            }
                        }

                        break;
                    }
                }
                else if (tmpFrameIDSize == 3)
                {
                    if (tmpFrameIDBytes[0] < 0x30 || tmpFrameIDBytes[0] > 0x5A ||
                        tmpFrameIDBytes[1] < 0x30 || tmpFrameIDBytes[1] > 0x5A ||
                        tmpFrameIDBytes[2] < 0x30 || tmpFrameIDBytes[2] > 0x5A)
                    {
                        // TODO: Try to keep reading and look for a valid frame
                        if (tmpFrameIDBytes[0] != 0 && tmpFrameIDBytes[0] != 0xFF)
                        {
                            string msg = string.Format("Out of range FrameID - 0x{0:X}|0x{1:X}|0x{2:X}",
                                tmpFrameIDBytes[0], tmpFrameIDBytes[1], tmpFrameIDBytes[2]);
                            Trace.WriteLine(msg);
                            Trace.WriteLine(ByteUtils.ISO88591GetString(tmpFrameIDBytes));
                        }

                        break;
                    }
                }

                string tmpFrameID = ByteUtils.ISO88591GetString(tmpFrameIDBytes);

                // TODO: Take out
                //Console.WriteLine(tmpFrameID); // TODO: take out
                /*
                    COMM Frames:
                    SoundJam_CDDB_TrackNumber
                    SoundJam_CDDB_1
                    iTunNORM
                    iTunSMPB
                    iTunes_CDDB_IDs                 
                 */

                IFrame tmpFrame;
                do
                {
                    IBindingList tmpBindingList;
                    if (tmpSingleOccurrenceFrames.TryGetValue(tmpFrameID, out tmpFrame))
                    {
                        tmpFrame.Read(tmpTagReadingInfo, stream);
                        tmpBytesRead += tmpFrame.FrameHeader.FrameSizeTotal;
                        //m_ReadFrames.Add(tmpFrame);
                    }
                    else if (tmpMultipleOccurrenceFrames.TryGetValue(tmpFrameID, out tmpBindingList))
                    {
                        tmpFrame = (IFrame)tmpBindingList.AddNew();
                        tmpFrame.Read(tmpTagReadingInfo, stream);
                        //m_ReadFrames.Add(tmpFrame);
                        tmpBytesRead += tmpFrame.FrameHeader.FrameSizeTotal;
                    }
                    else
                    {
                        if (tagVersion == ID3v2TagVersion.ID3v24)
                        {
                            string tmpNewFrameID;
                            if (m_ID3v24FrameAliases.TryGetValue(tmpFrameID, out tmpNewFrameID))
                                tmpFrameID = tmpNewFrameID;
                            else
                                break;
                        }
                        else if (tagVersion == ID3v2TagVersion.ID3v23)
                        {
                            string tmpNewFrameID;
                            if (m_ID3v23FrameAliases.TryGetValue(tmpFrameID, out tmpNewFrameID))
                                tmpFrameID = tmpNewFrameID;
                            else
                                break;
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (tmpFrame == null);

                // Frame is unknown
                if (tmpFrame == null)
                {
                    if (tmpFrameID != "NCON" &&  // non standard (old music match)
                        tmpFrameID != "MJMD" && // Non standard frame (Music Match XML)
                        tmpFrameID != "TT22" && // 000.00 - maybe meant to be 3 letter TT2 frame, and value be 2000.00? still makes no sense
                        tmpFrameID != "PCST" && // null data
                        tmpFrameID != "TCAT" && // category (ie, comedy) (distorted view)
                        tmpFrameID != "TKWD" && // looks like blog tags "comedy funny weird", etc (distorted view)
                        tmpFrameID != "TDES" && // xml file - used by distortedview.com
                        tmpFrameID != "TGID" && // url (from distortedview)
                        tmpFrameID != "WFED" && // url (thanks distortedview)
                        tmpFrameID != "CM1" && // some kind of comment, seen in ID3v2.2
                        tmpFrameID != "TMB" && // ripped by something other, not in spec
                        tmpFrameID != "RTNG" &&
                        tmpFrameID != "XDOR" && // year
                        tmpFrameID != "XSOP" && // looks like artist, "Allman Brothers Band, The"
                        tmpFrameID != "TENK") // itunes encoder (todo: add alias?)
                    {
                        /*String msg = String.Format("Unrecognized FrameID '{0}' (not critical)", tmpFrameID);
                        Trace.WriteLine(msg);*/
                    }

                    UnknownFrame tmpUNKN = new UnknownFrame(tmpFrameID, tmpTagReadingInfo, stream);
                    m_UnknownFrames.Add(tmpUNKN);
                    //m_ReadFrames.Add(tmpUNKN);
                    tmpBytesRead += tmpUNKN.FrameHeader.FrameSizeTotal;
                }
            }

            // Process genre // TODO: may need cleanup
            if (!string.IsNullOrEmpty(m_Genre.Value))
            {
                if (m_Genre.Value.StartsWith("("))
                {
                    int closeIndex = m_Genre.Value.IndexOf(')');
                    if (closeIndex != -1)
                    {
                        if (closeIndex != m_Genre.Value.Length - 1)
                        {
                            // Take text description
                            m_Genre.Value = m_Genre.Value.Substring(closeIndex + 1, m_Genre.Value.Length - (closeIndex + 1));
                        }
                        else
                        {
                            // Lookup genre value
                            string innerValue = m_Genre.Value.Substring(1, closeIndex - 1);
                            int innerValueResult;
                            if (int.TryParse(innerValue, out innerValueResult))
                            {
                                if (GenreHelper.GenreByIndex.Length > innerValueResult && innerValueResult >= 0)
                                {
                                    m_Genre.Value = GenreHelper.GenreByIndex[innerValueResult];
                                }
                                else
                                {
                                    Trace.WriteLine("Unrecognized genre");
                                }
                            }
                            else
                            {
                                Trace.WriteLine("Unrecognized genre");
                            }
                        }
                    }
                }
            }
        }

        internal List<IFrame> GetAllFrames(ID3v2TagVersion tagVersion)
        {
            Dictionary<string, IBindingList> multipleOccurrenceFrames = GetMultipleOccurrenceFrames(tagVersion);
            Dictionary<string, IFrame> singleOccurenceFrames = GetSingleOccurrenceFrames(tagVersion);

            List<IFrame> allFrames = new List<IFrame>();
            allFrames.AddRange(singleOccurenceFrames.Select(p => p.Value));
            foreach (IBindingList bindingList in multipleOccurrenceFrames.Values)
                allFrames.AddRange(bindingList.Cast<IFrame>());

            allFrames.AddRange(m_UnknownFrames.Cast<IFrame>());

            foreach (IFrame frame in new List<IFrame>(allFrames))
            {
                if (frame.GetBytes(tagVersion).Length == 0)
                    allFrames.Remove(frame);
            }

            return allFrames;
        }

        internal List<IFrame> GetAllFrames(ID3v2TagVersion tagVersion, string frameID)
        {
            if (string.IsNullOrEmpty(frameID))
                throw new ArgumentNullException("frameID");

            return GetAllFrames(tagVersion, new List<string> { frameID });
        }

        internal List<IFrame> GetAllFrames(ID3v2TagVersion tagVersion, IEnumerable<string> frameIDs)
        {
            if (frameIDs == null)
                throw new ArgumentNullException("frameIDs");

            List<IFrame> allFrames = GetAllFrames(tagVersion);
            foreach (IFrame frame in new List<IFrame>(allFrames))
            {
                bool found = false;
                foreach (string frameID in frameIDs)
                {
                    if (frame.GetFrameID(ID3v2TagVersion.ID3v22) == frameID ||
                        frame.GetFrameID(ID3v2TagVersion.ID3v23) == frameID ||
                        frame.GetFrameID(ID3v2TagVersion.ID3v24) == frameID)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    allFrames.Remove(frame);
            }

            return allFrames;
        }

        internal byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            using (MemoryStream tmpFrames = new MemoryStream())
            {
                Dictionary<string, IBindingList> tmpMultipleOccurrenceFrames = GetMultipleOccurrenceFrames(tagVersion);
                Dictionary<string, IFrame> tmpSingleOccurrenceFrames = GetSingleOccurrenceFrames(tagVersion);

                foreach (IFrame frame in tmpSingleOccurrenceFrames.Values)
                {
                    byte[] rawData = frame.GetBytes(tagVersion);
                    tmpFrames.Write(rawData, 0, rawData.Length);
                }

                foreach (IBindingList bindingList in tmpMultipleOccurrenceFrames.Values)
                {
                    for (int i = 0; i < bindingList.Count; i++)
                    {
                        IFrame frame = (IFrame)bindingList[i];

                        byte[] rawData = frame.GetBytes(tagVersion);
                        tmpFrames.Write(rawData, 0, rawData.Length);
                    }
                }

                foreach (UnknownFrame tmpUNKN in m_UnknownFrames)
                {
                    /*if (m_ReadFrames.Contains(tmpUNKN))
                        continue;*/

                    byte[] tmpRawData = tmpUNKN.GetBytes(tagVersion);
                    tmpFrames.Write(tmpRawData, 0, tmpRawData.Length);
                }

                return tmpFrames.ToArray();
            }
        }

        /// <summary>
        /// Forces the <see cref="INotifyPropertyChanged.PropertyChanged"/> event to fire.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public void SendPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion <<< Public Methods >>>

        #region <<< Public Properties >>>

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
                    m_PlaylistDelayMilliseconds.Value = null;
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
        /// Gets the BindingList of comment frames.
        /// </summary>
        /// <value>The BindingList of comment frames.</value>
        public BindingList<IComments> CommentsList
        {
            get { return m_CommentsList; }
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
        /// Gets or sets the set subtitle.  Intended for the subtitle of the 'part of
        /// a set' (for example, a specific disc in a multi disc album) this track belongs to.
        /// Only supported in ID3v2.4.  TSST in ID3v2.4.
        /// </summary>
        /// <value>
        /// The set subtitle.  Intended for the subtitle of the 'part of
        /// a set' (for example, a specific disc in a multi disc album) this track belongs to.
        /// Only supported in ID3v2.4.  TSST in ID3v2.4.
        /// </value>
        public string SetSubtitle
        {
            get { return m_SetSubtitle.Value; }
            set { m_SetSubtitle.Value = value; }
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

        #endregion <<< Public Properties >>>
    }
}

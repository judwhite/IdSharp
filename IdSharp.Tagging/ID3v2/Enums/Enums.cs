using System;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// The text encoding type used in a frame.
    /// </summary>
    public enum EncodingType : byte
    {
        /// <summary>
        /// ISO-8859-1
        /// </summary>
        ISO88591 = 0,
        /// <summary>
        /// UCS-2 in ID3v2.2 and ID3v2.3.  UTF-16 in ID3v2.4.  BOM required.
        /// </summary>
        Unicode = 1,
        /// <summary>
        /// UTF-16 Big Endian, without BOM.  Supported only in ID3v2.4.
        /// </summary>
        UTF16BE = 2,
        /// <summary>
        /// UTF-8.  Supported only in ID3v2.4.
        /// </summary>
        UTF8 = 3
    }

    // TODO: This makes more sense as a List/Dictionary
    /*public enum FileType
    {
        MPG,
        MPG_1,
        MPG_2,
        MPG_3,
        MPG_25,
        MPG_AAC,
        VQF,
        PCM,
        Other
    }*/

    // TODO: This makes more sense as a List/Dictionary
    /*public enum MediaType
    {
        DIG,
        DIG_A,
        ANA,
        ANA_WAC,
        ANA_8CA,
        CD,
        CD_A,
        CD_DD,
        CD_AD,
        CD_AA,
        LD,
        LD_A,
        TT,
        TT_33,
        TT_45,
        TT_71,
        TT_76,
        TT_78,
        TT_80,
        MD,
        MD_A,
        DAT,
        DAT_A,
        DAT_1,
        DAT_2,
        DAT_3,
        DAT_4,
        DAT_5,
        DAT_6,
        DCC,
        DCC_A,
        DVD,
        DVD_A,
        TV,
        TV_PAL,
        TV_NTSC,
        TV_SECAM,
        VID_PAL,
        VID_NTSC,
        VID_SECAM,
        VID_VHS,
        VID_SVHS,
        VID_BETA,
        VID_PAL_VHS,
        VID_NTSC_VHS,
        VID_SECAM_VHS,
        VID_PAL_SVHS,
        VID_NTSC_SVHS,
        VID_SECAM_SVHS,
        VID_PAL_BETA,
        VID_NTSC_BETA,
        VID_SECAM_BETA,
        RAD,
        RAD_FM,
        RAD_AM,
        RAD_LW,
        RAD_MW,
        TEL,
        TEL_I,
        MC,
        MC_4,
        MC_9,
        MC_I,
        MC_II,
        MC_III,
        MC_IV,
        REE,
        REE_9,
        REE_19,
        REE_38,
        REE_76,
        REE_I,
        REE_II,
        REE_III,
        REE_IV
    }*/

    /// <summary>
    /// The time stamp format used in an <see cref="IEventTiming">ETCO/ETC</see>, <see cref="ISynchronizedTempoCodes">SYTC/STC</see>, <see cref="ISynchronizedText">SYLT/SLT</see> or <see cref="IPositionSynchronization">POSS</see> frame.
    /// </summary>
    public enum TimestampFormat : byte
    {
        /// <summary>
        /// Absolute time, 32 bit sized, using MPEG frames as unit.
        /// </summary>
        MpegFrames = 1,
        /// <summary>
        /// Absolute time, 32 bit sized, using milliseconds as unit.
        /// </summary>
        Milliseconds = 2
    }

    /// <summary>
    /// The music event in an <see cref="IEventTimingItem">element</see> of an <see cref="IEventTiming">ETCO/ETC</see> frame.
    /// </summary>
    public enum MusicEvent : byte
    {
        /// <summary>
        /// Padding (has no meaning).
        /// </summary>
        Padding = 0x00,
        /// <summary>
        /// End of initial silence.
        /// </summary>
        EndOfInitialSilence = 0x01,
        /// <summary>
        /// Intro start.
        /// </summary>
        IntroStart = 0x02,
        /// <summary>
        /// Mainpart start.
        /// </summary>
        MainPartStart = 0x03,
        /// <summary>
        /// Outro start.
        /// </summary>
        OutroStart = 0x04,
        /// <summary>
        /// Outro end.
        /// </summary>
        OutroEnd = 0x05,
        /// <summary>
        /// Verse start.
        /// </summary>
        VerseStart = 0x06,
        /// <summary>
        /// Refrain start.
        /// </summary>
        RefrainStart = 0x07,
        /// <summary>
        /// Interlude start.
        /// </summary>
        InterludeStart = 0x08,
        /// <summary>
        /// Theme start.
        /// </summary>
        ThemeStart = 0x09,
        /// <summary>
        /// Variation start.
        /// </summary>
        VariationStart = 0x0A,
        /// <summary>
        /// Key change.
        /// </summary>
        KeyChange = 0x0B,
        /// <summary>
        /// Time change.
        /// </summary>
        TimeChange = 0x0C,
        /// <summary>
        /// Momentary unwanted noise (pop in the audio, etc).
        /// </summary>
        MomentaryUnwantedNoise = 0x0D,
        /// <summary>
        /// Sustained noise.
        /// </summary>
        SustainedNoise = 0x0E,
        /// <summary>
        /// Sustained noise end.
        /// </summary>
        SustainedNoiseEnd = 0x0F,
        /// <summary>
        /// Intro end.
        /// </summary>
        IntroEnd = 0x10,
        /// <summary>
        /// Mainpart end.
        /// </summary>
        MainPartEnd = 0x11,
        /// <summary>
        /// Verse end.
        /// </summary>
        VerseEnd = 0x12,
        /// <summary>
        /// Refrain end.
        /// </summary>
        RefrainEnd = 0x13,
        /// <summary>
        /// Theme end.
        /// </summary>
        ThemeEnd = 0x14,
        /// <summary>
        /// Profanity.  Only valid in ID3v2.4.
        /// </summary>
        Profanity = 0x15,
        /// <summary>
        /// Profanity end.  Only valid in ID3v2.4.
        /// </summary>
        ProfanityEnd = 0x16,
        /// <summary>
        /// User defined event 1.
        /// </summary>
        UserEvent1 = 0xE0,
        /// <summary>
        /// User defined event 2.
        /// </summary>
        UserEvent2 = 0xE1,
        /// <summary>
        /// User defined event 3.
        /// </summary>
        UserEvent3 = 0xE2,
        /// <summary>
        /// User defined event 4.
        /// </summary>
        UserEvent4 = 0xE3,
        /// <summary>
        /// User defined event 5.
        /// </summary>
        UserEvent5 = 0xE4,
        /// <summary>
        /// User defined event 6.
        /// </summary>
        UserEvent6 = 0xE5,
        /// <summary>
        /// User defined event 7.
        /// </summary>
        UserEvent7 = 0xE6,
        /// <summary>
        /// User defined event 8.
        /// </summary>
        UserEvent8 = 0xE7,
        /// <summary>
        /// User defined event 9.
        /// </summary>
        UserEvent9 = 0xE8,
        /// <summary>
        /// User defined event 10.
        /// </summary>
        UserEvent10 = 0xE9,
        /// <summary>
        /// User defined event 11.
        /// </summary>
        UserEvent11 = 0xEA,
        /// <summary>
        /// User defined event 12.
        /// </summary>
        UserEvent12 = 0xEB,
        /// <summary>
        /// User defined event 13.
        /// </summary>
        UserEvent13 = 0xEC,
        /// <summary>
        /// User defined event 14.
        /// </summary>
        UserEvent14 = 0xED,
        /// <summary>
        /// User defined event 15.
        /// </summary>
        UserEvent15 = 0xEE,
        /// <summary>
        /// User defined event 16.
        /// </summary>
        UserEvent16 = 0xEF,
        /// <summary>
        /// Audio end (start of silence).
        /// </summary>
        AudioEnd = 0xFD,
        /// <summary>
        /// Audio file ends.
        /// </summary>
        AudioFileEnds = 0xFE
    }

    /// <summary>
    /// Text content type of an <see cref="ISynchronizedTextItem">element</see> in the <see cref="ISynchronizedText">SYLT/SLT</see> frame.
    /// </summary>
    public enum TextContentType : byte
    {
        /// <summary>
        /// Other
        /// </summary>
        Other = 0x00,
        /// <summary>
        /// Lyrics
        /// </summary>
        Lyrics = 0x01,
        /// <summary>
        /// Text transcription
        /// </summary>
        TextTranscription = 0x02,
        /// <summary>
        /// Movement/part name.
        /// </summary>
        MovementPartName = 0x03,
        /// <summary>
        /// Events.  E.g. Don Quijote enters the stage".
        /// </summary>
        Event = 0x04,
        /// <summary>
        /// Chord.  E.g. "Bb F Fsus".
        /// </summary>
        Chord = 0x05,
        /// <summary>
        /// Trivia/pop up information.  Only valid in ID3v2.3 and higher.
        /// </summary>
        TriviaPopup = 0x06,
        /// <summary>
        /// URLs to webpages.  Only valid in ID3v2.4.
        /// </summary>
        URLsToWebpages = 0x07,
        /// <summary>
        /// URLs to images.  Only valid in ID3v2.4.
        /// </summary>
        URLsToImages = 0x08
    }

    /// <summary>
    /// Volume adjustment direction.  Used by the <see cref="IRelativeVolumeAdjustment"/> (RVA2/RVAD/RVA) frame.
    /// </summary>
    public enum VolumeAdjustmentDirection : byte
    {
        /// <summary>
        /// Represents a volume decrement.
        /// </summary>
        Decrement = 0,
        /// <summary>
        /// Represents a volume increment.
        /// </summary>
        Increment = 1
    }

    /// <summary>
    /// Picture type used by the <see cref="IAttachedPicture">APIC/PIC</see> frame.
    /// </summary>
    public enum PictureType : byte
    {
        /// <summary>
        /// Other.
        /// </summary>
        [Description("Other")]
        Other = 0x00,
        /// <summary>
        /// 32x32 pixels 'file icon' (PNG only).
        /// </summary>
        [Description("File icon (32x32 PNG)")]
        FileIcon32x32Png = 0x01,
        /// <summary>
        /// Other file icon.
        /// </summary>
        [Description("Other file icon")]
        OtherFileIcon = 0x02,
        /// <summary>
        /// Cover (front).
        /// </summary>
        [Description("Cover (front)")]
        CoverFront = 0x03,
        /// <summary>
        /// Cover (back).
        /// </summary>
        [Description("Cover (back)")]
        CoverBack = 0x04,
        /// <summary>
        /// Leaflet page.
        /// </summary>
        [Description("Leaflet page")]
        LeafletPage = 0x05,
        /// <summary>
        /// Media (e.g. label side of CD).
        /// </summary>
        [Description("Media (e.g. label side of CD)")]
        MediaLabelSideOfCD = 0x06,
        /// <summary>
        /// Lead artist/lead performer/soloist.
        /// </summary>
        [Description("Lead artist/performer")]
        LeadArtistPerformer = 0x07,
        /// <summary>
        /// Artist/performer.
        /// </summary>
        [Description("Artist/performer")]
        ArtistPerformer = 0x08,
        /// <summary>
        /// Conductor.
        /// </summary>
        [Description("Conductor")]
        Conductor = 0x09,
        /// <summary>
        /// Band/Orchestra.
        /// </summary>
        [Description("Band/orchestra")]
        BandOrchestra = 0x0A,
        /// <summary>
        /// Composer.
        /// </summary>
        [Description("Composer")]
        Composer = 0x0B,
        /// <summary>
        /// Lyricist/text writer.
        /// </summary>
        [Description("Lyricist")]
        Lyricist = 0x0C,
        /// <summary>
        /// Recording location.
        /// </summary>
        [Description("Recording location")]
        RecordingLocation = 0x0D,
        /// <summary>
        /// During recording.
        /// </summary>
        [Description("During recording")]
        DuringRecording = 0x0E,
        /// <summary>
        /// During performance.
        /// </summary>
        [Description("During performance")]
        DuringPerformance = 0x0F,
        /// <summary>
        /// Movie/video screen capture.
        /// </summary>
        [Description("Movie/video screen capture")]
        MovieVideoScreenCapture = 0x10,
        /// <summary>
        /// A bright colored fish.
        /// </summary>
        [Description("A bright colored fish")]
        ABrightColoredFish = 0x11,
        /// <summary>
        /// Illustration.
        /// </summary>
        [Description("Illustration")]
        Illustration = 0x12,
        /// <summary>
        /// Band/artist logotype.
        /// </summary>
        [Description("Band/artist logo")]
        BandArtistLogo = 0x13,
        /// <summary>
        /// Publisher/Studio logotype.
        /// </summary>
        [Description("Publisher/studio logo")]
        PublisherStudioLogo = 0x14
    }

    /// <summary>
    /// Describes how the audio is delivered when bought.  See the <see cref="ICommercial">COMR</see> frame.
    /// </summary>
    public enum ReceivedAs : byte
    {
        /// <summary>
        /// Other.
        /// </summary>
        Other = 0x00,
        /// <summary>
        /// Standard CD album with other songs.
        /// </summary>
        StandardCDAlbumWithOtherSongs = 0x01,
        /// <summary>
        /// Compressed audio on CD.
        /// </summary>
        CompressedAudioOnCD = 0x02,
        /// <summary>
        /// File over the internet.
        /// </summary>
        FileOverTheInternet = 0x03,
        /// <summary>
        /// Stream over the internet.
        /// </summary>
        StreamOverTheInternet = 0x04,
        /// <summary>
        /// As note sheets.
        /// </summary>
        AsNoteSheets = 0x05,
        /// <summary>
        /// As note sheets in a book with other sheets.
        /// </summary>
        AsNoteSheetsInABookWithOtherSheets = 0x06,
        /// <summary>
        /// Music on other media.
        /// </summary>
        MusicOnOtherMedia = 0x07,
        /// <summary>
        /// Non-musical merchandise.
        /// </summary>
        NonmusicalMerchandise = 0x08
    }

    /// <summary>
    /// ID3v2 tag version.
    /// </summary>
    public enum ID3v2TagVersion : byte
    {
        /// <summary>
        /// ID3v2.2
        /// </summary>
        [Description("ID3v2.2")]
        ID3v22 = 2,
        /// <summary>
        /// ID3v2.3
        /// </summary>
        [Description("ID3v2.3")]
        ID3v23 = 3,
        /// <summary>
        /// ID3v2.4
        /// </summary>
        [Description("ID3v2.4")]
        ID3v24 = 4
    }

    /// <summary>
    /// Options for reading tags.
    /// </summary>
    [Flags]
    public enum TagVersionOptions
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,
        /// <summary>
        /// Read ID3v2.4 FrameID sizes as non-syncsafe integers.  The standard is to use syncsafe integers, a change between previous 
        /// versions and ID3v2.4, however not all tag writers follow this rule.  This value is determined during an initial scan of an
        /// ID3v2.4 tag which attempts to determine the frame-size storage method in a number of ways, including jumping to the next 
        /// position indicated by both types of frame sizes and looking for another valid frame ID, or by determining the new position
        /// is the beginning of padding or the beginning of audio data.
        /// </summary>
        UseNonSyncSafeFrameSizeID3v24 = 1,
        /// <summary>
        /// Read one byte past the designated end of frame.  Notable offender is a piece of software calling itself CDCOPY.
        /// </summary>
        AddOneByteToSize = 2,
        /// <summary>
        /// Stream is unsynchronized.
        /// </summary>
        Unsynchronized = 4
    }

    /// <summary>
    /// Tag size restrictions.  Only valid in ID3v2.4.
    /// </summary>
    public enum TagSizeRestriction : byte
    {
        /// <summary>
        /// No more than 128 frames and 1 MB total tag size.
        /// </summary>
        NoMore128Frames1MBTotal = 0,
        /// <summary>
        /// No more than 64 frames and 128 KB total tag size.
        /// </summary>
        NoMore64Frames128KBTotal = 1,
        /// <summary>
        /// No more than 32 frames and 40 KB total tag size.
        /// </summary>
        NoMore32Frames40KBTotal = 2,
        /// <summary>
        /// No more than 32 frames and 4 KB total tag size.
        /// </summary>
        NoMore32Frames4KBTotal = 3
    }

    /// <summary>
    /// Text encoding restrictions.  Only valid in ID3v2.4.
    /// </summary>
    public enum TextEncodingRestriction : byte
    {
        /// <summary>
        /// No restrictions.
        /// </summary>
        NoRestrictions = 0,
        /// <summary>
        /// Strings are only encoded with ISO-8859-1 or UTF-8.
        /// </summary>
        ISO88591orUTF8 = 1
    }

    /// <summary>
    /// Text field size restrictions.  Note that the restriction applies to the number of characters, not the number of bytes.
    /// If a text frame consists of multiple strings, the restriction applies to the sum of all strings in the frame.
    /// Only valid in ID3v2.4.
    /// </summary>
    public enum TextFieldsSizeRestriction : byte
    {
        /// <summary>
        /// No restrictions.
        /// </summary>
        NoRestrictions = 0,
        /// <summary>
        /// No string is longer than 1024 characters.
        /// </summary>
        NoMore1024Chars = 1,
        /// <summary>
        /// No string is longer than 128 characters.
        /// </summary>
        NoMore128Chars = 2,
        /// <summary>
        /// No string is longer than 30 characters.
        /// </summary>
        NoMore30Chars = 3
    }

    /// <summary>
    /// Image encoding restrictions.  Only valid in ID3v2.4.
    /// </summary>
    public enum ImageEncodingRestriction : byte
    {
        /// <summary>
        /// No restrictions.
        /// </summary>
        NoRestrictions = 0,
        /// <summary>
        /// Images are encoded only with PNG or JPEG.
        /// </summary>
        PngOrJpg = 1
    }

    /// <summary>
    /// Image size restrictions.  Only valid in ID3v2.4.
    /// </summary>
    public enum ImageSizeRestriction : byte
    {
        /// <summary>
        /// No restrictions.
        /// </summary>
        NoRestrictions = 0,
        /// <summary>
        /// All images are 256x256 pixels or smaller.
        /// </summary>
        Max256x256 = 1,
        /// <summary>
        /// All images are 64x64 pixels or smaller.
        /// </summary>
        Max64x64 = 2,
        /// <summary>
        /// All images are exactly 64x64 pixels, unless required otherwise.
        /// </summary>
        Exactly64x64 = 3
    }

    /// <summary>
    /// Audio scrambling mode.  See <see cref="IAudioText"/>.
    /// </summary>
    public enum AudioScramblingMode
    {
        /// <summary>
        /// Return audio data using the 'unsynchronization' method, Return audio data using the 'unscramble' method
        /// </summary>
        Default,
        /// <summary>
        /// Return audio data using the 'unsynchronization' method.  The method is intended to be applied to all MPEG audio.
        /// </summary>
        Unsynchronization,
        /// <summary>
        /// Return audio data using the 'unscramble' method.  The method is intended to be applied to all non-MPEG audio.
        /// </summary>
        Scrambling,
        /// <summary>
        /// Return audio data as it is stored in the frame.
        /// </summary>
        None
    }

    /// <summary>
    /// Interpolation method.  See <see cref="IEqualizationList"/>.
    /// </summary>
    public enum InterpolationMethod
    {
        /// <summary>
        /// No interpolation is made. A jump from one adjustment level to another occurs in the middle between two adjustment points.
        /// </summary>
        Band = 0,
        /// <summary>
        /// Interpolation between adjustment points is linear.
        /// </summary>
        Linear = 1
    }

    /// <summary>
    /// Channel type.  See <see cref="IRelativeVolumeAdjustment"/>.
    /// </summary>
    internal enum ChannelType : byte
    {
        Other,
        MasterVolume,
        FrontRight,
        FrontLeft,
        BackRight,
        BackLeft,
        FrontCenter,
        BackCenter,
        Subwoofer
    }
}

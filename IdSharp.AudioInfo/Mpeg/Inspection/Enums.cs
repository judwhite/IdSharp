using System;

namespace IdSharp.AudioInfo.Inspection
{
    // MPEG version codes
    internal enum MpegVersion : byte
    {
        Version25 = 0,
        Unknown = 1,
        Version2 = 2,
        Version1 = 3
    }

    // MPEG layer codes
    internal enum MpegLayer : byte
    {
        Unknown = 0,
        LayerIII = 1,
        LayerII = 2,
        LayerI = 3
    }

    // Channel mode codes
    internal enum MpegChannel : byte
    {
        Stereo = 0,
        JointStereo = 1,
        DualChannel = 2,
        Mono = 3,
        Unknown = 4
    }

    // Extension mode codes (for Joint Stereo)
    internal enum JointStereoExtensionMode : byte
    {
        Off = 0,
        IS = 1,
        MS = 2,
        On = 3,
        Unknown = 4
    }

    // Emphasis mode codes
    internal enum Emphasis : byte
    {
        None = 0,
        _5015 = 1,
        Unknown = 2,
        CCIT = 3
    }

    // Sample rate codes
    internal enum SampleRateLevel : ushort // UInt16
    {
        Level3 = 0,
        Level2 = 1,
        Level1 = 2,
        Unknown = 3
    }

    // Encoder codes
    internal enum MpegEncoder
    {
        Unknown,
        Xing,
        FhG,
        LAME,
        Blade,
        GoGo,
        Shine,
        QDesign
    }

    // VBR header ID for Xing/FhG
    internal static class VBRHeaderID
    {
        private const String m_Xing = "Xing";
        private const String m_FhG = "VBRI";

        public static String Xing { get { return m_Xing; } }
        public static String FhG { get { return m_FhG; } }
    }

    // VBR Vendor ID strings
    internal static class VBRVendorID
    {
        private const string m_LAME = "LAME";
        private const string m_GoGoNew = "GOGO";
        private const string m_GoGoOld = "MPGE";

        public static string LAME { get { return m_LAME; } }
        public static string GoGoNew { get { return m_GoGoNew; } }
        public static string GoGoOld { get { return m_GoGoOld; } }
    }

    /// <summary>
    /// Preset guess status
    /// </summary>
    public enum UsePresetGuess
    {
        /// <summary>
        /// Not needed
        /// </summary>
        NotNeeded,
        /// <summary>
        /// Use guess
        /// </summary>
        UseGuess,
        /// <summary>
        /// Unabled to guess
        /// </summary>
        UnableToGuess
    }

    /// <summary>
    /// Lame Preset
    /// </summary>
    public enum LamePreset
    {
        /// <summary>
        /// --[alt-]preset insane
        /// </summary>
        Insane,
        /// <summary>
        /// --[alt-]preset extreme
        /// </summary>
        Extreme,
        /// <summary>
        /// --[alt-]preset fast extreme
        /// </summary>
        FastExtreme,
        /// <summary>
        /// --[alt-]preset standard
        /// </summary>
        Standard,
        /// <summary>
        /// --[alt-]preset fast standard
        /// </summary>
        FastStandard,
        /// <summary>
        /// --preset medium
        /// </summary>
        Medium,
        /// <summary>
        /// --preset medium
        /// </summary>
        FastMedium,
        /// <summary>
        /// --r3mix
        /// </summary>
        R3mix,
        /// <summary>
        /// --preset studio
        /// </summary>
        Studio,
        /// <summary>
        /// --preset cd 
        /// </summary>
        CD,
        /// <summary>
        /// --preset hifi
        /// </summary>
        Hifi,
        /// <summary>
        /// --preset tape
        /// </summary>
        Tape,
        /// <summary>
        /// --preset radio
        /// </summary>
        Radio,
        /// <summary>
        /// --preset fm 
        /// </summary>
        FM,
        /// <summary>
        /// --preset tape OR --preset radio OR --preset fm
        /// the above three presets generate identical files for
        /// versions of lame from 3.90.3 and 3.93 up
        /// </summary>
        TapeRadioFM, 
        /// <summary>
        /// --preset voice
        /// </summary>
        Voice,
        /// <summary>
        /// --preset mw-us
        /// </summary>
        MWUS,
        /// <summary>
        /// --preset phon+ OR --preset lw OR --preset mw-eu OR --preset sw
        /// </summary>
        MWEU,
        /// <summary>
        /// --preset phone
        /// </summary>
        Phone,
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown
    };

    internal enum LameVersionGroup
    {
        lvg390_3901_392,
        lvg3902_391,
        lvg393,
        lvg3931_3903up,
        lvg394up,
        None
    };

}

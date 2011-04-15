using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IdSharp.AudioInfo.Inspection
{
    // Xing/FhG VBR header data
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct VBRData
    {
        public bool Found;                  // True if VBR header found
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] ID;                      // Header ID: "Xing" or "VBRI"
        public int Frames;                   // Total number of frames
        public int Bytes;                    // Total number of bytes
        public Byte Scale;                     // VBR scale (1..100)
        public String VendorID;                // Vendor ID (if present)
    }

    // MPEG frame header data
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct FrameData
    {
        public bool Found;                  // True if frame found
        public int Position;                   // Frame position in the file
        public UInt16 Size;                    // Frame size (bytes)
        public bool Xing;                   // True if Xing encoder
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] Data;                    // The whole frame header data
        public MpegVersion VersionID;          // MPEG version ID
        public MpegLayer LayerID;              // MPEG layer ID
        public bool ProtectionBit;          // True if protected by CRC
        public UInt16 BitRateID;               // Bit rate ID
        public SampleRateLevel SampleRateID;   // Sample rate ID
        public bool PaddingBit;             // True if frame padded
        public bool PrivateBit;             // Extra information
        public MpegChannel ModeID;             // Channel mode ID
        public JointStereoExtensionMode ModeExtensionID;           // Mode extension ID (for Joint Stereo)
        public bool CopyrightBit;           // True if audio copyrighted
        public bool OriginalBit;            // True if original media
        public Emphasis EmphasisID;            // Emphasis ID
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LameTag
    {
        public Byte Quality;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] Encoder;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] VersionString;
        public Byte TagRevision_EncodingMethod;
        public Byte Lowpass;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] ReplayGain;
        public Byte EncodingFlags_ATHType;
        public Byte Bitrate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] EncoderDelays;
        public Byte MiscInfo;
        public Byte MP3Gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] Surround_Preset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] MusicLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] MusicCRC;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] InfoTagCRC;
        public Byte NoiseShaping;
        public Byte StereoMode;

        public static LameTag FromBinaryReader(BinaryReader br)
        {
            LameTag tmpLameTag = new LameTag();
            
            tmpLameTag.Quality = br.ReadByte();
            tmpLameTag.Encoder = br.ReadBytes(4);
            tmpLameTag.VersionString = br.ReadBytes(5);
            tmpLameTag.TagRevision_EncodingMethod = (byte)(br.ReadByte() & 0x0F);
            tmpLameTag.Lowpass = br.ReadByte();
            tmpLameTag.ReplayGain = br.ReadBytes(8);
            tmpLameTag.EncodingFlags_ATHType = (byte)(br.ReadByte() & 0x0F);
            tmpLameTag.Bitrate = br.ReadByte();
            tmpLameTag.EncoderDelays = br.ReadBytes(3);
            tmpLameTag.MiscInfo = br.ReadByte();
            tmpLameTag.MP3Gain = br.ReadByte();
            tmpLameTag.Surround_Preset = br.ReadBytes(2);
            tmpLameTag.MusicLength = br.ReadBytes(4);
            tmpLameTag.MusicCRC = br.ReadBytes(2);
            tmpLameTag.InfoTagCRC = br.ReadBytes(2);

            tmpLameTag.NoiseShaping = (byte)(tmpLameTag.MiscInfo & 0x03);
            tmpLameTag.StereoMode = (byte)((tmpLameTag.MiscInfo & 0x1C) >> 2);

            return tmpLameTag;
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct OldLameHeader
    {
        public Byte UnusedByte;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] Encoder;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] VersionString;

        public static OldLameHeader FromBinaryReader(BinaryReader br)
        {
            OldLameHeader tmpOldLameHeader = new OldLameHeader();
            tmpOldLameHeader.UnusedByte = br.ReadByte();
            tmpOldLameHeader.Encoder = br.ReadBytes(4);
            tmpOldLameHeader.VersionString = br.ReadBytes(16);
            return tmpOldLameHeader;
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct StartOfFile
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13, ArraySubType = UnmanagedType.AsAny)]
        public Byte[] Misc1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
	    public Byte[] Info1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
	    public Byte[] Misc2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
	    public Byte[] Info2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.AsAny)]
	    public Byte[] Misc3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.AsAny)]
	    public Byte[] Info3;

        public static StartOfFile FromBinaryReader(BinaryReader br)
        {
            StartOfFile tmpStartOfFile = new StartOfFile();
            tmpStartOfFile.Misc1 = br.ReadBytes(13);
            tmpStartOfFile.Info1 = br.ReadBytes(4);
            tmpStartOfFile.Misc2 = br.ReadBytes(4);
            tmpStartOfFile.Info2 = br.ReadBytes(4);
            tmpStartOfFile.Misc3 = br.ReadBytes(11);
            tmpStartOfFile.Info3 = br.ReadBytes(4);
            return tmpStartOfFile;
        }
    };

}

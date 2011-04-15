using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace IdSharp.AudioInfo.Inspection
{
    /// <summary>
    /// Basic LAME tag reader
    /// </summary>
    public sealed class BasicLameTagReader
    {
        private const byte Info1Offset = 0x0D;
        private const byte Info2Offset = 0x15;
        private const byte Info3Offset = 0x24;
        private const byte LAMETagOffset = 0x77;

        private LameTag _tag;
        
        private readonly LamePreset _presetGuess;
        private readonly bool _isPresetGuessNonBitrate;
        private readonly bool _isLameTagFound;
        private readonly ushort _preset;
        private readonly string _versionString;
        private readonly string _versionStringNonLameTag;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicLameTagReader"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public BasicLameTagReader(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            // Initialize
            _isLameTagFound = true;

            _tag = new LameTag();

            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                int startPos = ID3v2.GetTagSize(br.BaseStream);

                // Seek past ID3v2 tag
                br.BaseStream.Seek(startPos, SeekOrigin.Begin);

                // Get StartOfFile structure
                StartOfFile startOfFile = StartOfFile.FromBinaryReader(br);

                // Seek past ID3v2 tag
                br.BaseStream.Seek(startPos, SeekOrigin.Begin);

                string info1 = Encoding.ASCII.GetString(startOfFile.Info1);
                string info2 = Encoding.ASCII.GetString(startOfFile.Info2);
                string info3 = Encoding.ASCII.GetString(startOfFile.Info3);

		        if (info1 == "Xing" || info1 == "Info")
                {
                    br.BaseStream.Seek(Info1Offset, SeekOrigin.Current);
                }
		        else if (info2 == "Xing" || info2 == "Info")
                {
                    br.BaseStream.Seek(Info2Offset, SeekOrigin.Current);
                }
		        else if (info3 == "Xing" || info3 == "Info")
                {
                    br.BaseStream.Seek(Info3Offset, SeekOrigin.Current);
                }
		        else
                {
                    _isLameTagFound = true;
                }

                // Read LAME tag structure
                br.BaseStream.Seek(LAMETagOffset, SeekOrigin.Current);
                _tag = LameTag.FromBinaryReader(br);
                
                // Read old LAME header
                br.BaseStream.Seek(0 - Marshal.SizeOf(typeof(LameTag)), SeekOrigin.Current);
                OldLameHeader oldLameHeader = OldLameHeader.FromBinaryReader(br);
                _versionStringNonLameTag = Encoding.ASCII.GetString(oldLameHeader.VersionString);
            }

            // Set version string
            if (_tag.VersionString[1] == '.')
            {
                byte[] versionString = new byte[6];
                int i;
                for (i = 0; i < 4 || (i == 4 && _tag.VersionString[i] == 'b'); i++) 
                    versionString[i] = _tag.VersionString[i];
                Array.Resize(ref versionString, i);
                _versionString = Encoding.ASCII.GetString(versionString);
            }
            else
            {
                _versionString = "";
            }

            // If encoder is not LAME, set IsLameTagFound to false
            // TODO : How about other encoders that use the LAME tag?
		    if (Encoding.ASCII.GetString(_tag.Encoder) != "LAME")
            {
                _isLameTagFound = false;
            }

            // Set preset WORD
            _preset = (ushort)(((_tag.Surround_Preset[0] << 8) + _tag.Surround_Preset[1]) & 0x07FF);

            // Guess preset
            _presetGuess = (new PresetGuesser()).GuessPreset(
                VersionStringNonLameTag, /*m_Tag.VersionString*/
                _tag.Bitrate,
                _tag.Quality,
                _tag.TagRevision_EncodingMethod,
                _tag.NoiseShaping,
                _tag.StereoMode,
                _tag.EncodingFlags_ATHType,
                _tag.Lowpass,
                out _isPresetGuessNonBitrate);

        }

        /// <summary>
        /// Returns the version string from the LAME tag
        /// </summary>
        public string VersionString
        {
            get
            {
                return _versionString;
            }
        }

        /// <summary>
        /// Returns the version string from the old LAME header (pre-3.90)
        /// </summary>
        public string VersionStringNonLameTag
        {
            get
            {
                // In versions of LAME before the LAME tag was introduced, a 16 byte version
                // string was written into the LAME header, starting at the same position as
                // the current version string.
                return _versionStringNonLameTag;
            }
        }

        /// <summary>
        /// Returns Encoding Method Byte
        /// </summary>
        public byte EncodingMethod 
        { 
            get { return _tag.TagRevision_EncodingMethod; } 
        }

        /// <summary>
        /// Returns Preset WORD
        /// </summary>
        public ushort Preset 
        { 
            get { return _preset; } 
        }

        /// <summary>
        /// Returns guessed preset enum
        /// </summary>
        public LamePreset PresetGuess 
        { 
            get { return _presetGuess; } 
        }

        /// <summary>
        /// Returns bitrate from the LAME tag (not the actual bitrate for VBR files)
        /// </summary>
        public byte Bitrate 
        { 
            get { return _tag.Bitrate; } 
        }

        /// <summary>
        /// Returns true if the preset is guessed to be a command-line modified version of a preset.
        /// Only applies to LAME encoded MP3's that do not have preset info stored in the LAME tag.
        /// </summary>
        public bool IsPresetGuessNonBitrate 
        { 
            get { return _isPresetGuessNonBitrate; } 
        }

        /// <summary>
        /// Returns true if a LAME tag is present
        /// </summary>
        public bool IsLameTagFound 
        { 
            get { return _isLameTagFound; } 
        }
    }
}

using System;

namespace IdSharp.AudioInfo.Inspection
{
    /// <summary>
    /// DescriptiveLameTagReader
    /// </summary>
    public sealed class DescriptiveLameTagReader
    {
        private readonly MpegAudio _mpeg;
        private readonly BasicLameTagReader _basicReader;

        private UsePresetGuess _usePresetGuess;
        private string _preset;
        private string _presetGuess;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptiveLameTagReader"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public DescriptiveLameTagReader(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            _mpeg = new MpegAudio(path);

            _basicReader = new BasicLameTagReader(path);
            DeterminePresetRelatedValues();
        }

        /// <summary>
        /// Returns true if a LAME tag is present
        /// </summary>
        public bool IsLameTagFound 
        {
            get { return _basicReader.IsLameTagFound; } 
        }

        /// <summary>
        /// Returns the version string from the LAME tag
        /// </summary>
        public string VersionString
        { 
            get { return _basicReader.VersionString; } 
        }

        /// <summary>
        /// Returns the version string from the old LAME header (pre-3.90)
        /// </summary>
        public string VersionStringNonLameTag 
        { 
            get { return _basicReader.VersionStringNonLameTag; } 
        }
        
        /// <summary>
        /// Returns the preset guessing method used
        /// </summary>
        public UsePresetGuess UsePresetGuess 
        { 
            get { return _usePresetGuess; } 
        }

        /// <summary>
        /// Returns true if the preset is guessed to be a command-line modified version of a preset.
        /// 
        /// If this value is true, the LameTagInfoPreset property will have the string "(modified)" appended, 
        /// otherwise it wil have "(guessed)" appended.
        /// 
        /// Only applies to LAME encoded MP3's that do not have preset info stored in the LAME tag.
        /// </summary>
        public bool IsPresetGuessNonBitrate 
        { 
            get { return _basicReader.IsPresetGuessNonBitrate; } 
        }

        /// <summary>
        /// Returns the actual encoder preset (not guessed)
        /// </summary>
        public string Preset 
        { 
            get { return _preset; } 
        }

        /// <summary>
        /// Returns the guessed encoder preset
        /// </summary>
        public string PresetGuess 
        { 
            get { return _presetGuess; } 
        }

        /// <summary>
        /// Returns MPEG version and Layer
        /// </summary>
        public string LameTagInfoVersion
        {
            get
            {
                return _mpeg.Version + " " + _mpeg.Layer;
            }
        }

        /// <summary>
        /// Returns encoder
        /// </summary>
        public string LameTagInfoEncoder
        {
            get
            {
                string tmpLameTagInfoVersion;

                if (IsLameTagFound == false)
                {
                    tmpLameTagInfoVersion = _mpeg.Encoder;
                }
                else
                {
                    tmpLameTagInfoVersion = "LAME";

                    if (string.Compare(VersionString, "3.90") < 0) // FirstLameWithTag
                    {
                        // this file was encoded by an earlier version of LAME and
                        // the header only contains a version string.
                        if (!string.IsNullOrEmpty(VersionStringNonLameTag))
                        {
                            if (VersionStringNonLameTag[1] == '.')
                            {
                                tmpLameTagInfoVersion += " " + VersionStringNonLameTag;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(VersionString) == false)
                        {
                            tmpLameTagInfoVersion += " " + VersionString;
                        }
                    }
                }

                return tmpLameTagInfoVersion;
            }
        }

        /// <summary>
        /// Returns preset information
        /// </summary>
        public string LameTagInfoPreset
        {
            get
            {
                string tmpLameTagInfoPreset = "";

                if (IsLameTagFound)
                {
                    if (string.Compare(VersionString, "3.90") >= 0) // FirstLameWithTag
                    {
                        tmpLameTagInfoPreset = Preset;

                        if (UsePresetGuess == UsePresetGuess.UseGuess)
                        {
                            if (IsPresetGuessNonBitrate)
                            {
                                tmpLameTagInfoPreset = PresetGuess + " (modified)";
                            }
                            else
                            {
                                tmpLameTagInfoPreset = PresetGuess + " (guessed)";
                            }
                        }
                    }
                }

                return tmpLameTagInfoPreset;
            }
        }

        /// <summary>
        /// Determine Preset or PresetGuess values
        /// </summary>
        private void DeterminePresetRelatedValues()
        {
            _preset = DeterminePreset(out _usePresetGuess);

            if (_usePresetGuess == UsePresetGuess.NotNeeded)
            {
                _presetGuess = "";
            }
            else
            {
                _presetGuess = DeterminePresetGuess(ref _usePresetGuess);

                if (_basicReader.IsPresetGuessNonBitrate)
                {
                    _presetGuess += string.Format(" -b {0}", _basicReader.Bitrate);
                }
            }
        }

        /// <summary>
        /// Determine Preset
        /// </summary>
        /// <param name="usePresetGuess">Method sets this variable to the UsePresetGuess used</param>
        /// <returns>Preset string</returns>
        private string DeterminePreset(out UsePresetGuess usePresetGuess)
        {
            usePresetGuess = UsePresetGuess.NotNeeded;

            string result;
            int preset = _basicReader.Preset;

            if (preset >= 8 && preset <= 320)
            {
                result = preset.ToString();
                if (_basicReader.EncodingMethod == 1)
                    result = "cbr " + result;
                usePresetGuess = UsePresetGuess.UseGuess;
            }
            else
            {
                switch (preset)
                {
                    case 0:
                        result = "<not stored>";
                        usePresetGuess = UsePresetGuess.UseGuess;
                        break;

                    case 410:
                        result = "V9";
                        break;

                    case 420:
                        result = "V8";
                        break;

                    case 430:
                        result = "V7";
                        break;

                    case 440:
                        result = "V6";
                        break;

                    case 450:
                        result = "V5";
                        break;

                    case 460:
                        result = "V4: preset medium";
                        break;

                    case 470:
                        result = "V3";
                        break;

                    case 480:
                        result = "V2: preset standard";
                        break;

                    case 490:
                        result = "V1";
                        break;

                    case 500:
                        result = "V0: preset extreme";
                        break;

                    case 1000:
                        result = "r3mix";
                        break;

                    case 1001:
                        result = "--alt-preset standard";
                        break;

                    case 1002:
                        result = "--alt-preset extreme";
                        break;

                    case 1003:
                        result = "--alt-preset insane";
                        break;

                    case 1004:
                        result = "--alt-preset fast standard";
                        break;

                    case 1005:
                        result = "--alt-preset fast extreme";
                        break;

                    case 1006:
                        result = "preset medium";
                        break;

                    case 1007:
                        result = "preset fast medium";
                        break;

                    case 1010:
                        result = "preset portable";	// only used in alpha versions (3.94 alpha 14 and below)
                        break;

                    case 1015:
                        result = "preset radio";	// only used in alpha versions (3.94 alpha 14 and below)
                        break;

                    default:
                        result = string.Format("<unrecognised value {0}>", preset);
                        usePresetGuess = UsePresetGuess.UseGuess;
                        break;
                }
            }

            if (_basicReader.EncodingMethod == 4)
            {
                if (preset == 410 || preset == 420 || preset == 430 || preset == 440 || preset == 450 ||
                    preset == 460 || preset == 470 || preset == 480 || preset == 490 || preset == 500)
                {
                    result += " (fast mode)";
                }
            }

            return result;
        }

        private string DeterminePresetGuess(ref UsePresetGuess usePresetGuess)
        {
            string tmpResult;

            switch (_basicReader.PresetGuess)
            {
                case LamePreset.Insane:
                    tmpResult = "--alt-preset insane";
                    break;

                case LamePreset.Extreme:
                    tmpResult = "--alt-preset extreme";
                    break;

                case LamePreset.FastExtreme:
                    tmpResult = "--alt-preset fast extreme";
                    break;

                case LamePreset.Standard:
                    tmpResult = "--alt-preset standard";
                    break;

                case LamePreset.FastStandard:
                    tmpResult = "--alt-preset fast standard";
                    break;

                case LamePreset.Medium:
                    tmpResult = "preset medium";
                    break;

                case LamePreset.FastMedium:
                    tmpResult = "preset fast medium";
                    break;

                case LamePreset.R3mix:
                    tmpResult = "r3mix";
                    break;

                case LamePreset.Studio:
                    tmpResult = "preset studio";
                    break;

                case LamePreset.CD:
                    tmpResult = "preset cd";
                    break;

                case LamePreset.Hifi:
                    tmpResult = "preset hifi";
                    break;

                case LamePreset.Tape:
                    tmpResult = "preset tape";
                    break;

                case LamePreset.Radio:
                    tmpResult = "preset radio";
                    break;

                case LamePreset.FM:
                    tmpResult = "preset fm";
                    break;

                case LamePreset.TapeRadioFM:
                    tmpResult = "preset tape OR preset radio OR preset fm";
                    break;

                case LamePreset.Voice:
                    tmpResult = "preset voice";
                    break;

                case LamePreset.MWUS:
                    tmpResult = "preset mw-us";
                    break;

                case LamePreset.MWEU:
                    tmpResult = "preset phon+ OR preset lw OR preset mw-eu OR preset sw";
                    break;

                case LamePreset.Phone:
                    tmpResult = "preset phone";
                    break;

                default:
                    tmpResult = "";
                    // Only display "unable to guess" if the preset was not recorded (ie. the
                    // field value is zero.
                    if (_basicReader.Preset == 0)
                        usePresetGuess = UsePresetGuess.UnableToGuess;
                    else
                        usePresetGuess = UsePresetGuess.NotNeeded;
                    break;
            }

            return tmpResult;
        }
    }
}

using System;
using System.Collections.Generic;

namespace IdSharp.AudioInfo.Inspection
{
    internal sealed class PresetGuesser
    {
        private static readonly List<PresetGuessRow> _presetGuessTable = new List<PresetGuessRow>();

        static PresetGuesser()
        {
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 255, 58, 1, 1, 3, 2, 205, LamePreset.Insane));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 255, 58, 1, 1, 3, 2, 206, LamePreset.Insane));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 255, 57, 1, 1, 3, 4, 205, LamePreset.Insane));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 0, 78, 3, 2, 3, 2, 195, LamePreset.Extreme));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, 0, 78, 3, 2, 3, 2, 196, LamePreset.Extreme));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 78, 3, 1, 3, 2, 196, LamePreset.Extreme));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 0, 78, 4, 2, 3, 2, 195, LamePreset.FastExtreme));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 0, 78, 4, 2, 3, 2, 196, LamePreset.FastExtreme));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 0, 78, 3, 2, 3, 4, 190, LamePreset.Standard));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 78, 3, 1, 3, 4, 190, LamePreset.Standard));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 0, 78, 4, 2, 3, 4, 190, LamePreset.FastStandard));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 68, 3, 2, 3, 4, 180, LamePreset.Medium));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 68, 4, 2, 3, 4, 180, LamePreset.FastMedium));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 0, 88, 4, 1, 3, 3, 195, LamePreset.R3mix));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 0, 88, 4, 1, 3, 3, 196, LamePreset.R3mix));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 255, 99, 1, 1, 1, 2, 0, LamePreset.Studio));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 255, 58, 2, 1, 3, 2, 206, LamePreset.Studio));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 255, 58, 2, 1, 3, 2, 205, LamePreset.Studio));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 255, 57, 2, 1, 3, 4, 205, LamePreset.Studio));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 192, 88, 1, 1, 1, 2, 0, LamePreset.CD));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 192, 58, 2, 2, 3, 2, 196, LamePreset.CD));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 192, 58, 2, 2, 3, 2, 195, LamePreset.CD));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 192, 57, 2, 1, 3, 4, 195, LamePreset.CD));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 160, 78, 1, 1, 3, 2, 180, LamePreset.Hifi));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 160, 58, 2, 2, 3, 2, 180, LamePreset.Hifi));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 160, 57, 2, 1, 3, 4, 180, LamePreset.Hifi));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 128, 67, 1, 1, 3, 2, 180, LamePreset.Tape));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 128, 67, 1, 1, 3, 2, 150, LamePreset.Radio));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 112, 67, 1, 1, 3, 2, 150, LamePreset.FM));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 112, 58, 2, 2, 3, 2, 160, LamePreset.TapeRadioFM));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 112, 57, 2, 1, 3, 4, 160, LamePreset.TapeRadioFM));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 56, 58, 2, 2, 0, 2, 100, LamePreset.Voice));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 56, 57, 2, 1, 0, 4, 150, LamePreset.Voice));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 40, 65, 1, 1, 0, 2, 75, LamePreset.MWUS));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, 40, 65, 1, 1, 0, 2, 76, LamePreset.MWUS));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 40, 58, 2, 2, 0, 2, 70, LamePreset.MWUS));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 40, 57, 2, 1, 0, 4, 105, LamePreset.MWUS));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 24, 58, 2, 2, 0, 2, 40, LamePreset.MWEU));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 24, 58, 2, 2, 0, 2, 39, LamePreset.MWEU));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 24, 57, 2, 1, 0, 4, 59, LamePreset.MWEU));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 16, 58, 2, 2, 0, 2, 38, LamePreset.Phone));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 16, 58, 2, 2, 0, 2, 37, LamePreset.Phone));
            _presetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 16, 57, 2, 1, 0, 4, 56, LamePreset.Phone));
        }

        public LamePreset GuessPreset(String AVersionString, Byte ABitrate, Byte AQuality,
	            Byte AEncodingMethod, Byte ANoiseShaping, Byte AStereoMode,
                Byte AATHType, Byte ALowpassDiv100, out bool ANonBitrate)
        {
            LamePreset Result;

    	    String VersionString4 = AVersionString.Substring(0, 4);
            String VersionString5 = AVersionString.Substring(0, 5);

    	    // Both 3.90 and 3.90.1 record the version string as '3.90', whereas later
		    // varieties of 3.90 from 3.90.2 on record it as '3.90.'
            if ((VersionString4 == "3.90" && VersionString5 != "3.90.") || (VersionString4 == "3.92"))
            {
                Result = GuessForVersion(LameVersionGroup.lvg390_3901_392, ABitrate, AQuality, AEncodingMethod,
                            ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
    	    else if (VersionString5 == "3.90.")
            {
			    // Both 3.90.2 and 3.90.3 record the version string as '3.90.' so need to test both.
                Result = BestGuessTwoVersions(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, ABitrate, AQuality, AEncodingMethod,
                            ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
    	    else if (VersionString4 == "3.91")
            {
                Result = GuessForVersion(LameVersionGroup.lvg3902_391, ABitrate, AQuality, AEncodingMethod,
                            ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
	        else if (VersionString4 == "3.93")
	        {
			    // Both 3.93 and 3.93.1 record the version string as '3.93' so need to
			    // test both.
                Result = BestGuessTwoVersions(LameVersionGroup.lvg3931_3903up, LameVersionGroup.lvg393, ABitrate, AQuality, AEncodingMethod,
                            ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
	        }
	        else if (String.Compare(VersionString4, "3.94") >= 0)
            {
			    // 3.94 and 3.95[.x] are identical for preset guessing.
			    // 3.95.1 is the latest version at the time of writing.
                Result = GuessForVersion(LameVersionGroup.lvg394up, ABitrate, AQuality, AEncodingMethod,
                            ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
	        else
            {
		        Result = LamePreset.Unknown;
                ANonBitrate = false;
            }

            return Result;
        }

        private LamePreset GuessForVersion(LameVersionGroup AVersionGroup, Byte ABitrate, Byte AQuality,
	            Byte AEncodingMethod, Byte ANoiseShaping, Byte AStereoMode,
                Byte AATHType, Byte ALowpassDiv100, out bool ANonBitrate)
        {
            LamePreset Result = LamePreset.Unknown;

            LamePreset NonBitrateResult = LamePreset.Unknown;

            ANonBitrate = false;

		    foreach (PresetGuessRow row in _presetGuessTable)
            {
			    if
                    ((row.HasVersionGroup(AVersionGroup)) &&
				    (row.TVs[1] == AQuality) &&
				    (row.TVs[2] == AEncodingMethod) &&
				    (row.TVs[3] == ANoiseShaping) &&
				    (row.TVs[4] == AStereoMode) &&
				    (row.TVs[5] == AATHType) &&
				    (row.TVs[6] == ALowpassDiv100))
			    {
				    if (row.TVs[0] == ABitrate)
                    {
					    Result = row.Res;
					    break;
				    }
				    // Non-bitrate based guessing is only relevant to the VBR presets.
				    else if (AEncodingMethod == 3 || AEncodingMethod == 4)
                    {
					    NonBitrateResult = row.Res;
                    }
			    }
            }

            if (Result == LamePreset.Unknown && NonBitrateResult != LamePreset.Unknown)
		    {
			    ANonBitrate = true;
			    Result = NonBitrateResult;
		    }

            return Result;
        }

        private LamePreset BestGuessTwoVersions(LameVersionGroup AGroup1, LameVersionGroup AGroup2, Byte ABitrate,
                Byte AQuality, Byte AEncodingMethod, Byte ANoiseShaping, Byte AStereoMode,
                Byte AATHType, Byte ALowpassDiv100, out bool ANonBitrate)
        {
            LamePreset Result;

            // A bitrate-based guess is better than a non-bitrate guess which in turn is
            // better than no guess at all.

		    LamePreset FirstPreset, SecondPreset;
            bool FirstNonBitrate, SecondNonBitrate;

		    FirstPreset = GuessForVersion(AGroup1, ABitrate, AQuality, AEncodingMethod,
                            ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
		    FirstNonBitrate = ANonBitrate;

            SecondPreset = GuessForVersion(AGroup2, ABitrate, AQuality, AEncodingMethod,
                            ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
		    SecondNonBitrate = ANonBitrate;

            if (FirstPreset == LamePreset.Unknown || (FirstNonBitrate && (SecondPreset != LamePreset.Unknown)))
		    {
			    Result = SecondPreset;
			    ANonBitrate = SecondNonBitrate;
		    }
		    else
		    {
			    Result = FirstPreset;
			    ANonBitrate = FirstNonBitrate;
		    }

            return Result;
        }
    }
}

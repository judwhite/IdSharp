using System;
using IdSharp.Common.Events;

namespace IdSharp.Tagging.ID3v2
{
    public abstract partial class FrameContainer : IFrameContainer
    {
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
    }
}

using System;
using System.ComponentModel;


namespace IdSharp.Tagging.APEv2 {
    
    /// <summary>
    /// A collection of tag items related to ReplayGain
    /// </summary>
    public class ReplayGainTagItems {

        #region Exposed constants

        /// <summary>
        /// The prefix used to indicate that the tag item is related to ReplayGain
        /// </summary>
        public const string TAG_PREFIX = "REPLAYGAIN_";

        #endregion Exposed constants


        #region Private variables

        private decimal? _albumGain;
        private decimal? _albumPeak;
        private decimal? _trackGain;
        private decimal? _trackPeak;

        #endregion Private variables


        #region Exposed events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Exposed events


        #region Exposed properties

        /// <summary>
        /// Gets or sets the gain to be applied to the track for normalization in the context of the album
        /// </summary>
        /// <value>Album's gain</value>
        public decimal? AlbumGain {
            get { return _albumGain; }
            set { _albumGain = value; RaisePropertyChanged("AlbumGain"); }
        }

        /// <summary>
        /// Gets or sets the track's "linear" peak value in the context of the album
        /// </summary>
        /// <value>Album's peak value</value>
        public decimal? AlbumPeak {
            get { return _albumPeak; }
            set { _albumPeak = value; RaisePropertyChanged("AlbumPeak"); }
        }

        /// <summary>
        /// Gets or sets the gain to be applied to the track for normalization
        /// </summary>
        /// <value>Track's gain</value>
        public decimal? TrackGain {
            get { return _trackGain; }
            set { _trackGain = value; RaisePropertyChanged("TrackGain"); }
        }

        /// <summary>
        /// Gets or sets the track's "linear" peak value
        /// </summary>
        /// <value>Track's peak value</value>
        public decimal? TrackPeak {
            get { return _trackPeak; }
            set { _trackPeak = value; RaisePropertyChanged("TrackPeak"); }
        }

        /// <summary>
        /// Gets the track's "decibel" peak value in the context of the album
        /// </summary>
        /// <value>Album's peak value</value>
        public decimal? AlbumPeakInDecibels {
            get { return ConvertToDecibels(AlbumPeak); }
        }

        /// <summary>
        /// Gets the track's "decibel" peak value
        /// </summary>
        /// <value>Track's peak value</value>
        public decimal? TrackPeakInDecibels {
            get { return ConvertToDecibels(TrackPeak); }
        }


        internal string AlbumGainText {
            get { return ConvertValue(_albumGain, " dB"); }
            set { AlbumGain = ConvertValue(value, " dB"); }
        }

        internal string AlbumPeakText {
            get { return ConvertValue(_albumPeak); }
            set { AlbumPeak = ConvertValue(value); }
        }

        internal string TrackGainText {
            get { return ConvertValue(_trackGain, " dB"); }
            set { TrackGain = ConvertValue(value, " dB"); }
        }

        internal string TrackPeakText {
            get { return ConvertValue(_trackPeak); }
            set { TrackPeak = ConvertValue(value); }
        }

        #endregion Exposed properties


        #region Exposed methods

        internal void SetField(string key, string value) {

            if (!key.StartsWith("REPLAYGAIN_"))
                return;

            if (key == "REPLAYGAIN_ALBUM_GAIN")
                AlbumGainText = value;
            else if (key == "REPLAYGAIN_ALBUM_PEAK")
                AlbumPeakText = value;
            else if (key == "REPLAYGAIN_TRACK_GAIN")
                TrackGainText = value;
            else if (key == "REPLAYGAIN_TRACK_PEAK")
                TrackPeakText = value;
        }

        #endregion Exposed methods


        #region Private methods

        private decimal? ConvertValue(string value, string textToRemove) {

            if (value == null)
                return null;

            if (!string.IsNullOrEmpty(textToRemove))
                value = value.Replace(textToRemove, "");

            decimal result;

            if (decimal.TryParse(value, out result))
                return result;

            return null;
        }

        private decimal? ConvertValue(string value) {

            return ConvertValue(value, null);
        }

        private string ConvertValue(decimal? value, string textToAdd) {

            if (!value.HasValue)
                return null;

            return value.Value.ToString("#0.000000") + (textToAdd != null ? textToAdd : "");
        }

        private string ConvertValue(decimal? value) {

            return ConvertValue(value, null);
        }

        private decimal? ConvertToDecibels(decimal? value) {

            if (!value.HasValue)
                return null;
            else
                return 20 * (decimal) Math.Log10((double) value);
        }

        private decimal? ConvertFromDecibels(decimal? value) {

            if (!value.HasValue)
                return null;
            else
                return (decimal) Math.Pow(10, (double) (value / 20));
        }


        private void RaisePropertyChanged(string propertyName) {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Private methods
    }
}
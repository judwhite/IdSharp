using System;
using System.ComponentModel;


namespace IdSharp.Tagging.APEv2 {
    
    /// <summary>
    /// A collection of tag items related to MP3Gain
    /// </summary>
    public class MP3GainTagItems {

        #region Exposed constants

        /// <summary>
        /// The prefix used to indicate that the tag item is related to MP3Gain
        /// </summary>
        public const string TAG_PREFIX = "MP3GAIN_";

        #endregion Exposed constants


        #region Private variables

        private short? _trackMin;
        private short? _trackMax;
        private short? _albumMin;
        private short? _albumMax;
        private short? _undoLeftChannel;
        private short? _undoRightChannel;
        private bool? _undoWrap;

        #endregion Private variables


        #region Exposed events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Exposed events


        #region Exposed properties

        /// <summary>
        /// Gets or sets the track's minimum gain
        /// </summary>
        /// <value>Track's minimum gain</value>
        public short? TrackMinimumGain {
            get { return _trackMin; }
            set { _trackMin = value; RaisePropertyChanged("TrackMinimumGain"); }
        }

        /// <summary>
        /// Gets or sets the track's maximum gain
        /// </summary>
        /// <value>Track's maximum gain</value>
        public short? TrackMaximumGain {
            get { return _trackMax; }
            set { _trackMax = value; RaisePropertyChanged("TrackMaximumGain"); }
        }

        /// <summary>
        /// Gets or sets the track's minimum gain in the context of the album
        /// </summary>
        /// <value>Album's minimum gain</value>
        public short? AlbumMinimumGain {
            get { return _albumMin; }
            set { _albumMin = value; RaisePropertyChanged("AlbumMinimumGain"); }
        }

        /// <summary>
        /// Gets or sets the track's maximum gain in the context of the album
        /// </summary>
        /// <value>Album's maximum gain</value>
        public short? AlbumMaximumGain {
            get { return _albumMax; }
            set { _albumMax = value; RaisePropertyChanged("AlbumMaximumGain"); }
        }

        /// <summary>
        /// Gets or sets the number of steps necessary to undo the gain adjustment of the track's left channel
        /// </summary>
        /// <value>Left channel's undo amount</value>
        public short? UndoLeftChannelAdjustment {
            get { return _undoLeftChannel; }
            set { _undoLeftChannel = value; RaisePropertyChanged("UndoLeftChannelAdjustment"); }
        }

        /// <summary>
        /// Gets or sets the number of steps necessary to undo the gain adjustment of the track's right channel
        /// </summary>
        /// <value>Right channel's undo amount</value>
        public short? UndoRightChannelAdjustment {
            get { return _undoRightChannel; }
            set { _undoRightChannel = value; RaisePropertyChanged("UndoRightChannelAdjustment"); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if wrapping occurred during the gain adjustment on the track
        /// </summary>
        /// <value>Undo wrap flag</value>
        public bool? UndoWrapFlag {
            get { return _undoWrap; }
            set { _undoWrap = value; RaisePropertyChanged("UndoWrapFlag"); }
        }

        /// <summary>
        /// Gets the decibels of gain adjustment performed on the track's left channel
        /// </summary>
        /// <value>Left channel's undo amount</value>
        public decimal? UndoLeftChannelAdjustmentInDecibels {
            get { return ConvertToDecibels(UndoLeftChannelAdjustment); }
        }

        /// <summary>
        /// Gets the decibels of gain adjustment performed on the track's right channel
        /// </summary>
        /// <value>Right channel's undo amount</value>
        public decimal? UndoRightChannelAdjustmentInDecibels {
            get { return ConvertToDecibels(UndoRightChannelAdjustment); }
        }


        internal string TrackMinMaxText {
            get {
                if (_trackMin.HasValue && _trackMax.HasValue)
                    return ConvertValue(_trackMin.Value) + "," + ConvertValue(_trackMax.Value);
                else
                    return null;
            }
            set {
                TrackMinimumGain = ConvertValue(value, 0);
                TrackMaximumGain = ConvertValue(value, 1);
            }
        }

        internal string AlbumMinMaxText {
            get {
                if (_albumMin.HasValue && _albumMax.HasValue)
                    return ConvertValue(_albumMin.Value) + "," + ConvertValue(_albumMax.Value);
                else
                    return null;
            }
            set {
                AlbumMinimumGain = ConvertValue(value, 0);
                AlbumMaximumGain = ConvertValue(value, 1);
            }
        }

        internal string UndoText {
            get {
                if (_undoLeftChannel.HasValue && _undoRightChannel.HasValue && _undoWrap.HasValue)
                    return ConvertValue(_undoLeftChannel.Value, true) + "," +
                        ConvertValue(_undoRightChannel.Value, true) + "," + (_undoWrap.Value ? "W" : "N");
                else
                    return null;
            }
            set {
                UndoLeftChannelAdjustment = ConvertValue(value, 0);
                UndoRightChannelAdjustment = ConvertValue(value, 1);
                
                string val = ConvertValueToString(value, 2);

                if (string.IsNullOrWhiteSpace(val))
                    UndoWrapFlag = null;
                else if (val.ToUpper() == "N")
                    UndoWrapFlag = false;
                else if (val.ToUpper() == "W")
                    UndoWrapFlag = true;
                else
                    UndoWrapFlag = null;
            }
        }

        #endregion Exposed properties


        #region Exposed methods

        internal void SetField(string key, string value) {

            if (!key.StartsWith(TAG_PREFIX))
                return;

            if (key == "MP3GAIN_MINMAX")
                TrackMinMaxText = value;
            else if (key == "MP3GAIN_ALBUM_MINMAX")
                AlbumMinMaxText = value;
            else if (key == "MP3GAIN_UNDO")
                UndoText = value;
        }

        #endregion Exposed methods


        #region Private methods

        private string ConvertValueToString(string value, int position) {

            if (value == null)
                return null;

            string[] parts = value.Split(new char[] { ',' });

            if (parts.Length <= position)
                return null;

            return parts[position];
        }

        private short? ConvertValue(string value, int position) {

            string part = ConvertValueToString(value, position);

            if (string.IsNullOrWhiteSpace(part))
                return null;

            short result;

            if (short.TryParse(part, out result))
                return result;

            return null;
        }

        private string ConvertValue(short? value, bool includeSign) {

            if (!value.HasValue)
                return null;

            return value.Value.ToString((includeSign ? "+" : "") + "000;-000;");
        }

        private string ConvertValue(short? value) {

            return ConvertValue(value, false);
        }

        private decimal? ConvertToDecibels(short? value) {

            if (!value.HasValue)
                return null;
            else
                return value.Value * (decimal) 1.5;
        }

        private short? ConvertFromDecibels(decimal? value) {

            if (!value.HasValue)
                return null;
            else
                return (short) (value.Value / (decimal) 1.5);
        }


        private void RaisePropertyChanged(string propertyName) {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Private methods
    }
}
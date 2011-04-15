using System.Collections.ObjectModel;
using IdSharp.AudioInfo;
using IdSharp.AudioInfo.Inspection;
using IdSharp.Tagging.Harness.Wpf.Events;
using IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel
{
    public class ID3v2ViewModel : ViewModelBase, IID3v2ViewModel
    {
        private static readonly ObservableCollection<string> _genreCollection;
        private static readonly ObservableCollection<ID3v2TagVersion> _id3v2VersionCollection;

        private ID3v2.ID3v2 _id3v2;
        private string _fileName;
        private string _artist;
        private string _title;
        private string _album;
        private string _genre;
        private string _year;
        private string _track;
        private decimal? _playLength;
        private decimal? _bitrate;
        private string _encoderPreset;
        private ID3v2TagVersion? _id3v2Version;
        private bool _canSave;

        static ID3v2ViewModel()
        {
            _genreCollection = new ObservableCollection<string>(GenreHelper.GetSortedGenreList());
            _id3v2VersionCollection = new ObservableCollection<ID3v2TagVersion> { ID3v2TagVersion.ID3v22, ID3v2TagVersion.ID3v23, ID3v2TagVersion.ID3v24 };
        }

        public ID3v2ViewModel()
        {
            EventDispatcher.Subscribe<string>(EventType.LoadFile, OnLoadFile);
            EventDispatcher.Subscribe(EventType.SaveFile, OnSaveFile);
        }

        public ObservableCollection<string> GenreCollection
        {
            get { return _genreCollection; }
        }

        public ID3v2TagVersion? ID3v2Version
        {
            get { return _id3v2Version; }
            set
            {
                if (_id3v2Version != value)
                {
                    _id3v2Version = value;
                    SendPropertyChanged("ID3v2Version");
                }
            }
        }

        public string FileName
        {
            get { return _fileName; }
            private set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    SendPropertyChanged("FileName");
                }
            }
        }

        public string Artist
        {
            get { return _artist; }
            set
            {
                if (_artist != value)
                {
                    _artist = value;
                    SendPropertyChanged("Artist");
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    SendPropertyChanged("Title");
                }
            }
        }

        public string Album
        {
            get { return _album; }
            set
            {
                if (_album != value)
                {
                    _album = value;
                    SendPropertyChanged("Album");
                }
            }
        }

        public string Genre
        {
            get { return _genre; }
            set
            {
                if (_genre != value)
                {
                    _genre = value;
                    SendPropertyChanged("Genre");
                }
            }
        }

        public string Year
        {
            get { return _year; }
            set
            {
                if (_year != value)
                {
                    _year = value;
                    SendPropertyChanged("Year");
                }
            }
        }

        public string Track
        {
            get { return _track; }
            set
            {
                if (_track != value)
                {
                    _track = value;
                    SendPropertyChanged("Track");
                }
            }
        }

        public decimal? PlayLength
        {
            get { return _playLength; }
            private set
            {
                if (_playLength != value)
                {
                    _playLength = value;
                    SendPropertyChanged("PlayLength");
                }
            }
        }

        public decimal? Bitrate
        {
            get { return _bitrate; }
            private set
            {
                if (_bitrate != value)
                {
                    _bitrate = value;
                    SendPropertyChanged("Bitrate");
                }
            }
        }

        public string EncoderPreset
        {
            get { return _encoderPreset; }
            private set
            {
                if (_encoderPreset != value)
                {
                    _encoderPreset = value;
                    SendPropertyChanged("EncoderPreset");
                }
            }
        }

        public bool CanSave
        {
            get { return _canSave; }
            private set
            {
                if (_canSave != value)
                {
                    _canSave = value;
                    SendPropertyChanged("CanSave");
                }
            }
        }

        public ObservableCollection<ID3v2TagVersion> ID3v2VersionCollection
        {
            get { return _id3v2VersionCollection; }
        }

        private void OnLoadFile(string fileName)
        {
            _id3v2 = new ID3v2.ID3v2(fileName);
            IAudioFile audioFile = AudioFile.Create(fileName, true);
            DescriptiveLameTagReader lameTagReader = new DescriptiveLameTagReader(fileName);

            FileName = fileName;
            Artist = _id3v2.Artist;
            Title = _id3v2.Title;
            Album = _id3v2.Album;
            Genre = _id3v2.Genre;
            Year = _id3v2.Year;
            Track = _id3v2.TrackNumber;
            ID3v2Version = _id3v2.Header.TagVersion;

            PlayLength = audioFile.TotalSeconds;
            Bitrate = audioFile.Bitrate;
            EncoderPreset = string.Format("{0} {1}", lameTagReader.LameTagInfoEncoder, lameTagReader.UsePresetGuess == UsePresetGuess.NotNeeded ? lameTagReader.Preset : lameTagReader.PresetGuess);

            CanSave = true;

            // TODO: Pictures
        }

        private void OnSaveFile()
        {
            _id3v2.Artist = Artist;
            _id3v2.Title = Title;
            _id3v2.Album = Album;
            _id3v2.Genre = Genre;
            _id3v2.Year = Year;
            _id3v2.TrackNumber = Track;
            _id3v2.Header.TagVersion = ID3v2Version.Value;

            // TODO: Pictures

            _id3v2.Save(_fileName);

            OnLoadFile(_fileName);
        }
    }
}

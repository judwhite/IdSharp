using System.Collections.ObjectModel;
using System.IO;
using IdSharp.Tagging.Harness.Wpf.Events;
using IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces;
using IdSharp.Tagging.ID3v1;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel
{
    public class ID3v1ViewModel : ViewModelBase, IID3v1ViewModel
    {
        private static readonly ObservableCollection<string> _genreCollection;
        private static readonly ObservableCollection<ID3v1TagVersion> _id3v1VersionCollection;

        private IID3v1Tag _id3v1;
        private string _fullFileName;
        private string _fileName;
        private bool _canSave;

        static ID3v1ViewModel()
        {
            _genreCollection = new ObservableCollection<string>(GenreHelper.GetSortedGenreList());
            _id3v1VersionCollection = new ObservableCollection<ID3v1TagVersion> { ID3v1TagVersion.ID3v10, ID3v1TagVersion.ID3v11 };
        }

        public ID3v1ViewModel()
        {
            EventDispatcher.Subscribe<string>(EventType.LoadFile, OnLoadFile);
            EventDispatcher.Subscribe(EventType.SaveFile, OnSaveFile);
        }

        public ObservableCollection<string> GenreCollection
        {
            get { return _genreCollection; }
        }

        public ID3v1TagVersion? ID3v1Version
        {
            get { return _id3v1 == null ? null : (ID3v1TagVersion?)_id3v1.TagVersion; }
            set
            {
                if (_id3v1.TagVersion != value)
                {
                    _id3v1.TagVersion = value.Value;
                    SendPropertyChanged("ID3v1Version");
                    SendPropertyChanged("Track");
                    SendPropertyChanged("Comment");
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
            get { return _id3v1 == null ? null : _id3v1.Artist; }
            set
            {
                if (_id3v1.Artist != value)
                {
                    _id3v1.Artist = value;
                    SendPropertyChanged("Artist");
                }
            }
        }

        public string Title
        {
            get { return _id3v1 == null ? null : _id3v1.Title; }
            set
            {
                if (_id3v1.Title != value)
                {
                    _id3v1.Title = value;
                    SendPropertyChanged("Title");
                }
            }
        }

        public string Album
        {
            get { return _id3v1 == null ? null : _id3v1.Album; }
            set
            {
                if (_id3v1.Album != value)
                {
                    _id3v1.Album = value;
                    SendPropertyChanged("Album");
                }
            }
        }

        public string Genre
        {
            get { return _id3v1 == null ? null : GenreHelper.GenreByIndex[_id3v1.GenreIndex]; }
            set
            {
                if (Genre != value)
                {
                    _id3v1.GenreIndex = GenreHelper.GetGenreIndex(value);
                    SendPropertyChanged("Genre");
                }
            }
        }

        public string Year
        {
            get { return _id3v1 == null ? null : _id3v1.Year; }
            set
            {
                if (_id3v1.Year != value)
                {
                    _id3v1.Year = value;
                    SendPropertyChanged("Year");
                }
            }
        }

        public int? Track
        {
            get { return _id3v1 == null ? null : _id3v1.TrackNumber; }
            set
            {
                if (_id3v1.TrackNumber != value)
                {
                    _id3v1.TrackNumber = value;
                    SendPropertyChanged("Track");
                }
            }
        }

        public string Comment
        {
            get { return _id3v1 == null ? null : _id3v1.Comment; }
            set
            {
                if (_id3v1.Comment != value)
                {
                    _id3v1.Comment = value;
                    SendPropertyChanged("Comment");
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

        public ObservableCollection<ID3v1TagVersion> ID3v1VersionCollection
        {
            get { return _id3v1VersionCollection; }
        }

        private void OnLoadFile(string fileName)
        {
            _id3v1 = new ID3v1Tag(fileName);

            _fullFileName = fileName;
            FileName = Path.GetFileName(fileName);
            SendPropertyChanged("Artist");
            SendPropertyChanged("Title");
            SendPropertyChanged("Album");
            SendPropertyChanged("Genre");
            SendPropertyChanged("Year");
            SendPropertyChanged("Track");
            SendPropertyChanged("Comment");
            SendPropertyChanged("ID3v1Version");

            CanSave = true;
        }

        private void OnSaveFile()
        {
            _id3v1.Save(_fullFileName);
        }
    }
}

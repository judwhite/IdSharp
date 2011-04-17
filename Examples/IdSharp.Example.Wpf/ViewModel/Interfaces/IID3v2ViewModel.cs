using System.Collections.ObjectModel;
using System.ComponentModel;
using IdSharp.Tagging.Harness.Wpf.Model;
using IdSharp.Tagging.ID3v2;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces
{
    public interface IID3v2ViewModel : INotifyPropertyChanged
    {
        string FileName { get; }
        string Artist { get; set; }
        string Title { get; set; }
        string Album { get; set; }
        string Genre { get; set; }
        string Year { get; set; }
        string Track { get; set; }
        string Comment { get; set; }
        decimal? PlayLength { get; }
        decimal? Bitrate { get; }
        string EncoderPreset { get; }
        bool CanSave { get; }
        ObservableCollection<string> GenreCollection { get; }
        ObservableCollection<ID3v2TagVersion> ID3v2VersionCollection { get; }
        ID3v2TagVersion? ID3v2Version { get; }
        Picture CurrentPicture { get; set; }
        ObservableCollection<Picture> PictureCollection { get; }
        ObservableCollection<PictureType> PictureTypeCollection { get; }
    }
}

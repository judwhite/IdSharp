using System.Collections.ObjectModel;
using System.ComponentModel;
using IdSharp.Tagging.ID3v1;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces
{
    public interface IID3v1ViewModel : INotifyPropertyChanged
    {
        string FileName { get; }
        string Artist { get; set; }
        string Title { get; set; }
        string Album { get; set; }
        string Genre { get; set; }
        string Year { get; set; }
        int? Track { get; set; }
        string Comment { get; set; }
        bool CanSave { get; }
        ObservableCollection<string> GenreCollection { get; }
        ObservableCollection<ID3v1TagVersion> ID3v1VersionCollection { get; }
        ID3v1TagVersion? ID3v1Version { get; }
    }
}

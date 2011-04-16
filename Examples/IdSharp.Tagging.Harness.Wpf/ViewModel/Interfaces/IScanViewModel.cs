using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using IdSharp.Tagging.Harness.Wpf.Model;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces
{
    public interface IScanViewModel : INotifyPropertyChanged
    {
        string Directory { get; set; }
        bool IsScanning { get; }
        ICommand ScanCommand { get; }
        ICommand CancelCommand { get; }
        ICommand BrowseCommand { get; }
        int PercentComplete { get; }
        ObservableCollection<Track> TrackCollection { get; }
    }
}

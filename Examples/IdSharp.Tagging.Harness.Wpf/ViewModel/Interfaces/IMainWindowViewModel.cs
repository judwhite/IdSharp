using System.ComponentModel;
using System.Windows.Input;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces
{
    public interface IMainWindowViewModel : INotifyPropertyChanged
    {
        string Version { get; }
        ICommand LoadCommand { get; }
        ICommand SaveCommand { get; }
        ICommand RemoveID3v2Command { get; }
        ICommand RemoveID3v1Command { get; }
        bool CanSave { get; }
        bool CanRemoveID3v1 { get; }
        bool CanRemoveID3v2 { get; }
    }
}

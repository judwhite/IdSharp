using System.Reflection;
using System.Windows;
using System.Windows.Input;
using IdSharp.Tagging.Harness.Wpf.Commands;
using IdSharp.Tagging.Harness.Wpf.Events;
using IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces;
using Microsoft.Win32;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private readonly OpenFileDialog _openFileDialog;
        private readonly DelegateCommand _loadCommand;
        private readonly DelegateCommand _saveCommand;
        private readonly DelegateCommand _removeID3v1Command;
        private readonly DelegateCommand _removeID3v2Command;
        private readonly DelegateCommand _closeCommand;

        private string _fileName;

        private bool _canSave;
        private bool _canRemoveID3v1;
        private bool _canRemoveID3v2;

        public MainWindowViewModel()
        {
            AssemblyName assemblyName = AssemblyName.GetAssemblyName("IdSharp.Tagging.dll");
            Version = string.Format("IdSharp library version: {0}   PLEASE TEST ON BACKUPS", assemblyName.Version);

            _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "*.mp3|*.mp3";

            _loadCommand = new DelegateCommand(Load);
            _saveCommand = new DelegateCommand(Save, () => CanSave);
            _removeID3v1Command = new DelegateCommand(RemoveID3v1, () => CanRemoveID3v1);
            _removeID3v2Command = new DelegateCommand(RemoveID3v2, () => CanRemoveID3v2);
            _closeCommand = new DelegateCommand(CloseApplication);
        }

        public string Version
        {
            get;
            private set;
        }

        public ICommand LoadCommand
        {
            get { return _loadCommand; }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand; }
        }

        public ICommand RemoveID3v2Command
        {
            get { return _removeID3v2Command; }
        }

        public ICommand RemoveID3v1Command
        {
            get { return _removeID3v1Command; }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }

        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                if (_canSave != value)
                {
                    _canSave = value;
                    SendPropertyChanged("CanSave");
                }
            }
        }

        public bool CanRemoveID3v1
        {
            get { return _canRemoveID3v1; }
            set
            {
                if (_canRemoveID3v1 != value)
                {
                    _canRemoveID3v1 = value;
                    SendPropertyChanged("CanRemoveID3v1");
                }
            }
        }

        public bool CanRemoveID3v2
        {
            get { return _canRemoveID3v2; }
            set
            {
                if (_canRemoveID3v2 != value)
                {
                    _canRemoveID3v2 = value;
                    SendPropertyChanged("CanRemoveID3v2");
                }
            }
        }

        private void CloseApplication()
        {
            Application.Current.MainWindow.Close();
        }

        private void Load()
        {
            if (_openFileDialog.ShowDialog() != true)
                return;

            LoadFile(_openFileDialog.FileName);
        }

        private void LoadFile(string fileName)
        {
            _fileName = fileName;
            EventDispatcher.Publish(EventType.LoadFile, _fileName);

            CanSave = true;
            CanRemoveID3v2 = ID3v2.ID3v2.DoesTagExist(_fileName);
            CanRemoveID3v1 = ID3v1.ID3v1.DoesTagExist(_fileName);

            _saveCommand.RaiseCanExecuteChanged();
            _removeID3v2Command.RaiseCanExecuteChanged();
            _removeID3v1Command.RaiseCanExecuteChanged();
        }

        private void Save()
        {
            EventDispatcher.Publish(EventType.SaveFile);
        }

        private void RemoveID3v1()
        {
            if (MessageBox.Show("Remove ID3v1 tag?", "Remove ID3v1 tag", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            ID3v1.ID3v1.RemoveTag(_fileName);

            LoadFile(_fileName);
        }

        private void RemoveID3v2()
        {
            if (MessageBox.Show("Remove ID3v2 tag?", "Remove ID3v2 tag", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            ID3v2.ID3v2.RemoveTag(_fileName);

            LoadFile(_fileName);
        }
    }
}

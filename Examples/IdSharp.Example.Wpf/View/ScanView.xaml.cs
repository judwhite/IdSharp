using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IdSharp.Tagging.Harness.Wpf.Commands;
using IdSharp.Tagging.Harness.Wpf.Events;
using IdSharp.Tagging.Harness.Wpf.Model;
using IdSharp.Tagging.Harness.Wpf.ViewModel;

namespace IdSharp.Tagging.Harness.Wpf.View
{
    /// <summary>
    /// Interaction logic for ScanView.xaml
    /// </summary>
    public partial class ScanView : UserControl
    {
        public ScanView()
        {
            InitializeComponent();

            DataContext = new ScanViewModel();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = (DependencyObject)e.OriginalSource;
            var row = WpfApplicationHelper.TryFindParent<DataGridRow>(source);

            if (row == null) 
                return;

            Track track = row.DataContext as Track;
            if (track == null)
                return;

            EventDispatcher.Publish(EventType.LoadFile, track.FullFileName);

            e.Handled = true;
        }
    }
}

using System.Windows.Controls;
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
    }
}

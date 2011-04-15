using System.Windows.Controls;
using IdSharp.Tagging.Harness.Wpf.ViewModel;

namespace IdSharp.Tagging.Harness.Wpf.View
{
    /// <summary>
    /// Interaction logic for ID3v2View.xaml
    /// </summary>
    public partial class ID3v2View : UserControl
    {
        public ID3v2View()
        {
            InitializeComponent();

            DataContext = new ID3v2ViewModel();
        }
    }
}

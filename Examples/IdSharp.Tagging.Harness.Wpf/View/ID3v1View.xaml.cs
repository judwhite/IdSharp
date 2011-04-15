using System.Windows.Controls;
using IdSharp.Tagging.Harness.Wpf.ViewModel;

namespace IdSharp.Tagging.Harness.Wpf.View
{
    /// <summary>
    /// Interaction logic for ID3v1View.xaml
    /// </summary>
    public partial class ID3v1View : UserControl
    {
        public ID3v1View()
        {
            InitializeComponent();

            DataContext = new ID3v1ViewModel();
        }
    }
}

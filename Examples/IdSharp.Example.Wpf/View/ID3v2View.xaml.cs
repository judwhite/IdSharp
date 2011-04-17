using System.Windows;
using System.Windows.Controls;
using IdSharp.Common.Events;
using IdSharp.Tagging.Harness.Wpf.Model;
using IdSharp.Tagging.Harness.Wpf.ViewModel;
using Microsoft.Win32;

namespace IdSharp.Tagging.Harness.Wpf.View
{
    /// <summary>
    /// Interaction logic for ID3v2View.xaml
    /// </summary>
    public partial class ID3v2View : UserControl
    {
        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog();

        public ID3v2View()
        {
            InitializeComponent();

            DataContext = new ID3v2ViewModel();

            CollectionNavigator.BeforeAdd += CollectionNavigator_BeforeAdd;
            CollectionNavigator.BeforeDelete += CollectionNavigator_BeforeDelete;

            _openFileDialog.Filter = "*.jpg;*.png;*.gif;*.bmp|*.jpg;*.png;*.gif;*.bmp";
        }

        private static void CollectionNavigator_BeforeDelete(object sender, CancelDataEventArgs<object> e)
        {
            if (MessageBox.Show("Delete picture?", "Delete picture", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void CollectionNavigator_BeforeAdd(object sender, CancelDataEventArgs<object> e)
        {
            if (_openFileDialog.ShowDialog() != true)
            {
                e.Cancel = true;
                return;
            }

            Picture picture = new Picture(_openFileDialog.FileName);
            if (picture.ImageSource != null)
                e.Data = picture;
            else
                e.Cancel = true;
        }
    }
}

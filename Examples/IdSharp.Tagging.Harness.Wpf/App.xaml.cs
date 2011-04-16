using System.Windows;
using System.Windows.Threading;
using IdSharp.Tagging.Harness.Wpf.Commands;

namespace IdSharp.Tagging.Harness.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            WpfApplicationHelper.EnabledSelectAllOnFocusOnTextBox();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

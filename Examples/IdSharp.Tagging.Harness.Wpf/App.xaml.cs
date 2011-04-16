using System.Windows;
using System.Windows.Threading;

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
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

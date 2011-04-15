using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace IdSharp.Tagging.Harness.Wpf.Commands
{
    /// <summary>
    /// WPF helper class.
    /// </summary>
    public static class WpfApplicationHelper
    {
        /// <summary>
        /// Invokes a method on the UI thread.
        /// </summary>
        /// <param name="method">The method.</param>
        public static void InvokeOnUI(Action method)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, method);
        }

        /// <summary>
        /// Invokes a method with return type <typeparamref name="T"/> on the UI thread.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="method">The method.</param>
        /// <returns>The return value from <paramref name="method"/>.</returns>
        public static T InvokeOnUI<T>(Func<T> method)
        {
            return (T)Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, method);
        }

        /// <summary>
        /// Enables <see cref="Visibility" /> binding for <see cref="DataGridColumn"/>.
        /// </summary>
        public static void EnableVisibilityBindingOnDataGridColumn()
        {
            FrameworkElement.DataContextProperty.AddOwner(typeof(DataGridColumn));
            FrameworkElement.DataContextProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, DataGrid_OnDataContextChanged));
        }

        private static void DataGrid_OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid grid = d as DataGrid;
            if (grid != null)
            {
                foreach (DataGridColumn col in grid.Columns)
                {
                    col.SetValue(FrameworkElement.DataContextProperty, e.NewValue);
                }
            }
        }

        /// <summary>
        /// Enableds automatic select all when a <see cref="TextBox" /> receives focus.
        /// </summary>
        public static void EnabledSelectAllOnFocusOnTextBox()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent, new RoutedEventHandler(TextBox_GotFocus));
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}

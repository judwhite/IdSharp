using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public static System.Windows.Forms.IWin32Window GetIWin32Window(this System.Windows.Media.Visual visual)
        {
            var source = (System.Windows.Interop.HwndSource)PresentationSource.FromVisual(visual);
            System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            private readonly IntPtr _handle;

            public OldWindow(IntPtr handle)
            {
                _handle = handle;
            }

            IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return _handle; }
            }
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Keep in mind that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }
    }
}

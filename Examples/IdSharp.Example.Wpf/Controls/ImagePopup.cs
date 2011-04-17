using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using IdSharp.Tagging.Harness.Wpf.View;

namespace IdSharp.Tagging.Harness.Wpf.Controls
{
    public class ImagePopup : Control
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImagePopup));

        static ImagePopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImagePopup), new FrameworkPropertyMetadata(typeof(ImagePopup)));
        }

        private Image _image;

        public ImagePopup()
        {
            MouseEnter += ImagePopup_MouseEnter;
            MouseLeave += ImagePopup_MouseLeave;
        }

        private void ImagePopup_MouseLeave(object sender, MouseEventArgs e)
        {
            Image tmpImage = _image;

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(100)));
            doubleAnimation.Completed += delegate
                                             {
                                                 MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                                                 mainWindow.MainCanvas.Children.Remove(tmpImage);
                                             };
            tmpImage.BeginAnimation(OpacityProperty, doubleAnimation);
        }

        private void ImagePopup_MouseEnter(object sender, MouseEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Point point = e.GetPosition(mainWindow.MainCanvas);

            _image = new Image { Source = Source, Height = 200, Width = 200 };
            double y = point.Y;
            if (y + _image.Height > mainWindow.ActualHeight - 60)
            {
                y = mainWindow.ActualHeight - _image.Height - 60;
            }
            _image.Margin = new Thickness(point.X, y, 0, 0);
            _image.Opacity = 0;
            _image.IsHitTestVisible = false;
            mainWindow.MainCanvas.Children.Add(_image);

            DoubleAnimation doubleAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(200)));
            _image.BeginAnimation(OpacityProperty, doubleAnimation);
        }

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
    }
}

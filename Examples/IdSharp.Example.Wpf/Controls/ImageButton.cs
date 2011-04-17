using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace IdSharp.Tagging.Harness.Wpf.Controls
{
    public class ImageButton : ButtonBase
    {
        public static readonly DependencyProperty RegularImageSourceProperty = DependencyProperty.Register("RegularImageSource", typeof(string), typeof(ImageButton));
        public static readonly DependencyProperty HotImageSourceProperty = DependencyProperty.Register("HotImageSource", typeof(string), typeof(ImageButton));
        public static readonly DependencyProperty DisabledImageSourceProperty = DependencyProperty.Register("DisabledImageSource", typeof(string), typeof(ImageButton));

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public string RegularImageSource
        {
            get { return (string)GetValue(RegularImageSourceProperty); }
            set { SetValue(RegularImageSourceProperty, value); }
        }

        public string HotImageSource
        {
            get { return (string)GetValue(HotImageSourceProperty); }
            set { SetValue(HotImageSourceProperty, value); }
        }

        public string DisabledImageSource
        {
            get { return (string)GetValue(DisabledImageSourceProperty); }
            set { SetValue(DisabledImageSourceProperty, value); }
        }
    }
}

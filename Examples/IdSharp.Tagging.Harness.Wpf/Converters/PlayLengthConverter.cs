using System;
using System.Globalization;
using System.Windows.Data;

namespace IdSharp.Tagging.Harness.Wpf.Converters
{
    public class PlayLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            int playLength = (int)value;
            return string.Format("{0}:{1:00}", playLength / 60, playLength % 60);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

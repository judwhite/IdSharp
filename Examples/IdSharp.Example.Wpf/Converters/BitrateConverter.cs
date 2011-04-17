using System;
using System.Globalization;
using System.Windows.Data;

namespace IdSharp.Tagging.Harness.Wpf.Converters
{
    public class BitrateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            return string.Format("{0:0} kbps", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

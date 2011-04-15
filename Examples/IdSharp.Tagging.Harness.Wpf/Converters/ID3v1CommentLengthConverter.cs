using System;
using System.Globalization;
using System.Windows.Data;
using IdSharp.Tagging.ID3v1;

namespace IdSharp.Tagging.Harness.Wpf.Converters
{
    public class ID3v1CommentLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ID3v1TagVersion)
            {
                ID3v1TagVersion tagVersion = (ID3v1TagVersion)value;
                if (tagVersion == ID3v1TagVersion.ID3v10)
                    return 30;
                else
                    return 28;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

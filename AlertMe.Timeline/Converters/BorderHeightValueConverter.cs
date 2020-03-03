using System;
using System.Globalization;
using System.Windows.Data;

namespace AlertMe.Timeline.Converters
{
    public class BorderHeightValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var perc = (double)value;
            return perc / 100 * 90;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

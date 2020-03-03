using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AlertMe.Timeline.Converters
{
    public class TimelineProgressVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var percentagePassed = (double)value;
            return percentagePassed == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

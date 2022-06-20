using System;
using System.Globalization;
using System.Windows.Data;

namespace Stacker.Converters
{
    internal class TimeSpanToTimeString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(TimeSpan)) throw new ArgumentException("Must be int");

            int minutes = (int)((TimeSpan)value).TotalMinutes;

            if (minutes % 60 == 0) return $"{minutes / 60}h";
            if (minutes < 60) return $"{minutes}m";

            return $"{minutes / 60}h {minutes % 60}m";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

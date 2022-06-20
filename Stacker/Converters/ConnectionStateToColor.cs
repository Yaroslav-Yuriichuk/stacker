using Stacker.Interfaces;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Stacker.Converters
{
    public class ConnectionStateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(ConnectionState)) throw new ArgumentException("Must be ConnectionState");

            return value switch
            {
                ConnectionState.Connected => "#23DA36",
                ConnectionState.Disconnected => "#D58186",
                ConnectionState.Connecting => "#F0A202",
                _ => throw new NotSupportedException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

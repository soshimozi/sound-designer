using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundDesigner.Converter;

public class RoundingValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var doubleValue = (double)((float)value);
        if (int.TryParse(parameter?.ToString(), out var decimalPlaces))
        {
            //return Math.Round(doubleValue, decimalPlaces);
            var format = "{0:f" + decimalPlaces + "}";
            return String.Format(format, doubleValue);
        }
        else
        {
            return doubleValue;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
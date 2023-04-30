using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundDesigner.Converter;

public class DefaultValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var objectValue = (double)(float)value;

        if (Math.Abs(objectValue) >= 1000)
        {
            return $"{(objectValue / 1000):0.00}k";
        }

        return objectValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
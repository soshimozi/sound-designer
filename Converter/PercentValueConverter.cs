using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundDesigner.Converter;

[ValueConversion(typeof(float), typeof(float))]
internal class PercentValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var convertedValue = ((float)value) / 100;
        return $"{convertedValue:0.0}%";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
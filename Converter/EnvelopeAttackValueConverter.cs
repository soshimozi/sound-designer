using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundDesigner.Converter;

public class EnvelopeAttackValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // TODO: pass value as parameter
        var maxValue = 10.0;
        var minValue = 0.0;
        var minRangeValue = 1.0;
        var maxRangeValue = 100.0;

        var mappedValue = Map((float)value, minRangeValue, maxRangeValue, minValue, maxValue);
        if (mappedValue >= 1)
            return $"{mappedValue:0.0} s";
        else
            return $"{mappedValue*1000:##0} ms";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static float Map(float value, double inputStart, double inputEnd, double outputStart, double outputEnd)
    {
        return (float)(((value - inputStart) / (inputEnd - inputStart)) * (outputEnd - outputStart) + outputStart);
    }
}
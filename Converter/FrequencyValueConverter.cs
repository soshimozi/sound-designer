﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundDesigner.Converter;

internal class FrequencyValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var units = "hz";
        var doubleValue = (double)(float)value;

        if (!(Math.Abs(doubleValue) >= 1000f)) return $"{doubleValue:0.}{units}";

        units = "kHz";
        doubleValue = doubleValue / 1000;
        return $"{doubleValue:0.0}{units}";

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
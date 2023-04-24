using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SoundDesigner.Models;

namespace SoundDesigner.Converters;

public class AudioJackToCanvasLeftConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not AudioJackModel model) return DependencyProperty.UnsetValue;

        return model.X;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
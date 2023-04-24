using SoundDesigner.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoundDesigner.Converters;

public class AudioJackToCanvasTopConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not AudioJackModel model) return DependencyProperty.UnsetValue;

        return model.Y;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
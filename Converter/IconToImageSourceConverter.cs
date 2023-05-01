using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using SoundDesigner.Extension;

namespace SoundDesigner.Converter;

public class IconToImageSourceConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is not Icon icon ? null : icon.ToImageSource();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
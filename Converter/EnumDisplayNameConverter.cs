using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System;

namespace SoundDesigner.Converter;

public class EnumDisplayNameConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var valueString = value.ToString();

        if (string.IsNullOrEmpty(valueString)) return string.Empty;

        var fieldInfo = value.GetType().GetField(valueString);
        if (fieldInfo == null) return Enum.GetName(value.GetType(), value);
        var attributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
        if (attributes.Length <= 0) return Enum.GetName(value.GetType(), value);
        var attribute = attributes[0] as DisplayAttribute;

        if (!string.IsNullOrEmpty(attribute?.Name))
            return attribute.Name;
        else if (!string.IsNullOrEmpty(attribute?.ShortName))
            return attribute.ShortName;
        return Enum.GetName(value.GetType(), value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
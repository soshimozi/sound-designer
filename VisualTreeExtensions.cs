using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace SoundDesigner;

public static class VisualTreeExtensions
{
    public static T? FindAncestor<T>(this DependencyObject? obj) where T : DependencyObject
    {
        while (obj != null && obj is not T)
        {
            obj = VisualTreeHelper.GetParent(obj) ?? default;
        }
        return obj as T;
    }
}
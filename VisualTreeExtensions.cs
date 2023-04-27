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

    public static T? FindChild<T>(this DependencyObject parent, string? name = null)  where T : DependencyObject
    {
        if (parent is T t)
        {
            return t;
        }

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            var foundElement = FindChild<T>(child, name);
            if (foundElement != null)
            {
                return foundElement;
            }
        }

        return null;
    }
}
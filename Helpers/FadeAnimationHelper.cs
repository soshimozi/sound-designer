using System.Windows.Media.Animation;
using System.Windows;
using System;

namespace SoundDesigner.Helpers;

public static class FadeAnimationHelper
{
    public static readonly DependencyProperty AnimateOpacityProperty =
        DependencyProperty.RegisterAttached(
            "AnimateOpacity",
            typeof(bool),
            typeof(FadeAnimationHelper),
            new PropertyMetadata(false, OnAnimateOpacityChanged));

    public static bool GetAnimateOpacity(DependencyObject obj)
    {
        return (bool)obj.GetValue(AnimateOpacityProperty);
    }

    public static void SetAnimateOpacity(DependencyObject obj, bool value)
    {
        obj.SetValue(AnimateOpacityProperty, value);
    }

    private static void OnAnimateOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element)
        {
            RunAnimation(element, (bool)e.NewValue);
        }
    }

    private static void RunAnimation(FrameworkElement element, bool isVisible)
    {
        var doubleAnimation = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(0.3)) // Adjust the duration as needed
        };

        if (isVisible)
        {
            doubleAnimation.From = 0;
            doubleAnimation.To = 1;
        }
        else
        {
            doubleAnimation.From = 1;
            doubleAnimation.To = 0;
        }

        element.BeginAnimation(FrameworkElement.OpacityProperty, doubleAnimation);
    }
}

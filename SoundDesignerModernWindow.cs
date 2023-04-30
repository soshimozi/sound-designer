using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace SoundDesigner;

public class SoundDesignerModernWindow : ModernWindow
{
    public static readonly DependencyProperty IsIndicatorVisibleProperty =
        DependencyProperty.Register("IsIndicatorVisible", typeof(bool), typeof(ModernWindow),
            new PropertyMetadata(false));

    public static readonly DependencyProperty IndicatorImageSourceProperty =
        DependencyProperty.Register("IndicatorImageSource", typeof(ImageSource), typeof(ModernWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IndicatorOpacityProperty =
        DependencyProperty.Register("IndicatorOpacity", typeof(double), typeof(ModernWindow),
            new PropertyMetadata(0.0));

    public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource",
        typeof(ImageSource), typeof(ModernWindow), new PropertyMetadata(null));


    public ImageSource IconSource
    {
        get => (ImageSource)this.GetValue(IconSourceProperty);
        set => this.SetValue(IconSourceProperty, value);

    }

    public bool IsIndicatorVisible
    {
        get => (bool)this.GetValue(IsIndicatorVisibleProperty);
        set => this.SetValue(IsIndicatorVisibleProperty, value);
    }

    public ImageSource IndicatorImageSource
    {
        get => (ImageSource)this.GetValue(IndicatorImageSourceProperty);
        set => this.SetValue(IndicatorImageSourceProperty, value);
    }

    public double IndicatorOpacity
    {
        get => (double)this.GetValue(IndicatorOpacityProperty);
        set => this.SetValue(IndicatorOpacityProperty, value);
    }
}
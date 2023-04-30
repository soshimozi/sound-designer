using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SoundDesigner.Control;

public class HorizontalSwitch : UserControl
{
    private int _step;
    private double _value;
    private BitmapImage _image;

    public HorizontalSwitch(int x, int y, int width, int height, string imagePath, int step, double defaultValue)
    {
        Width = width;
        Height = height;
        _step = step;
        _value = defaultValue;

        _image = new BitmapImage(new Uri(imagePath, UriKind.Relative));

        Margin = new Thickness(x, y, 0, 0);
        ClipToBounds = true;

        MouseDown += OnMouseDown;
        MouseMove += OnMouseMove;
        MouseUp += OnMouseUp;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        int n = (int)Math.Floor(_value * _step);
        if (_value == 1)
            --n;

        drawingContext.DrawImage(_image, new Rect(-n * Width, 0, _image.PixelWidth, Height));
    }

    private void Set(double value)
    {
        if (value < 0) value = 0;
        if (value > 1) value = 1;
        _value = value;
        InvalidateVisual();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        OnMouseMove(sender, e);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            double mouseX = e.GetPosition(this).X;
            Set(Math.Floor((1 - mouseX / Width) * _step) / (_step - 1));
        }
    }

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        // Handle mouse up event if necessary
    }
}
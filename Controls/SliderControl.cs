using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SoundDesigner.Controls;

public class SliderControl : UserControl
{

    private BitmapImage _image;
    private readonly int _steps;


    private double v0, y0;

    // 48 x 128
    public SliderControl(int x, int y, int width, int height, int steps, string imagePath, double defaultValue)
    {
        _image = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        Width = width;
        Height = height;
        _steps = steps;

        Margin = new Thickness(x, y, 0, 0);
        ClipToBounds = true;

        Value = defaultValue;

        MouseMove += SliderControl_OnMouseMove;
        MouseDown += SliderControl_MouseDown;
        MouseUp += SliderControl_OnMouseUp;
    }

    private void SliderControl_OnMouseUp(object sender, MouseEventArgs e)
    {
        ReleaseMouseCapture();
    }

    private void SliderControl_MouseDown(object sender, MouseEventArgs e)
    {
        v0 = Value;
        y0 = e.GetPosition(this).Y;

        CaptureMouse();
    }

    private void SliderControl_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var delta = (y0 - e.GetPosition(this).Y);
            Value = v0 + delta * 0.01;
        }
    }


    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set
        {
            if (value < 0) value = 0;
            if (value >= .99) value = .99;
            SetValue(ValueProperty, value);
        }
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(double), typeof(SliderControl), new PropertyMetadata(0.5, OnValueChanged));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SliderControl slider)
        {
            slider.InvalidateVisual(); // Redraw the slider when the value changes
        }
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);


        int n = (int)Math.Floor(Value * _steps);

        // Your drawing code here, e.g., drawing lines, rectangles, and the image from the sprite sheet
        // Refer to the original JavaScript code to replicate the drawing logic
        drawingContext.DrawImage(_image, new Rect(0, -n * Height, Width, _image.PixelHeight));
    }

    // Your logic for handling mouse events (MouseDown, MouseMove, and MouseUp) goes here
}
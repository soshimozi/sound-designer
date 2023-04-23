using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using JackTest.Controls;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace SoundDesigner.Controls;

public class AudioJack : System.Windows.Controls.Control
{
    //public Color Color { get; set; }

    public Cable? ConnectedFrom { get; set; }
    public List<Cable> ConnectedTo { get; set; }
    public int ToY { get; set; }

    private BitmapImage? _image;
    private static int[] colorValues = new[]
    {
        0xcc0000, 0x0000cc, 0xcc4400,
        0x0088cc, 0xcc8800, 0x0044cc,
        0x555500, 0x00cccc, 0x885500,
        0x4400cc, 0x44cc00, 0x880055,
        0x005500, 0x550055, 0x00cc44,
        0xcc0088, 0x005588, 0xcc0044
    };

    private static Color[] colors = colorValues.Select(c => Color.FromArgb((byte)((c >> 24) & 0xff), (byte) ((c >> 16) & 0xff), (byte) ((c >> 8) & 0xff), (byte) (c & 0xff))).ToArray();

    private static int _col = 0;

    static AudioJack()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AudioJack), new FrameworkPropertyMetadata(typeof(AudioJack)));
    }

    public Color CableColor
    {
        get => (Color)GetValue(CableColorProperty);
        set => SetValue(CableColorProperty, value);
    }

    public static readonly DependencyProperty CableColorProperty =
        DependencyProperty.Register("CableColor", typeof(Color), typeof(AudioJack), new PropertyMetadata(Color.FromRgb(128, 128, 128)));

    public int JackType
    {
        get => (int)GetValue(JackTypeProperty);
        set => SetValue(JackTypeProperty, value);
    }

    public static readonly DependencyProperty JackTypeProperty =
        DependencyProperty.Register("JackType", typeof(int), typeof(AudioJack), new PropertyMetadata(0));

    public AudioJack()
    {
        CableColor = colors[(_col++ % colors.Length)];
    }

    //public AudioJack(string name, int toY, string imagePath, Color? color = null)
    //{

    //    Name = name;
    //    ToY = toY;
    //    Color = color ?? colors[(_col++ % colors.Length)];
    //    ConnectedTo = new List<Cable>();


    //    //_image = (BitmapImage)FindResource("MyImage");
    //    //Height = 32;
    //    //Width = 32;
    //}


    //public override void OnApplyTemplate()
    //{
    //    base.OnApplyTemplate();


    //    //_image = (BitmapImage ?)FindResource("PartJackImage");
    //    //_image = (BitmapImage)Template.FindResource("JackImage");

    //}

    //protected override void OnRender(DrawingContext drawingContext)
    //{
    //    base.OnRender(drawingContext);

    //    var x = ActualWidth / 2;
    //    var y = ActualHeight / 2;

    //    // Draw the outer circle
    //    //var outerFillBrush = new SolidColorBrush(Color.FromArgb(255, 110, 110, 110));
    //    //var outerStrokePen = new Pen(new SolidColorBrush(Color.FromArgb(255, 62, 62, 62)), 1);
    //    //drawingContext.DrawEllipse(outerFillBrush, outerStrokePen, new Point(x, y), 7, 7);

    //    //// Draw the inner circle
    //    //var innerFillBrush = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
    //    //var innerStrokePen = new Pen(new SolidColorBrush(Colors.Black), 1);
    //    //drawingContext.DrawEllipse(innerFillBrush, innerStrokePen, new Point(x, y), 3, 3);

    //    //// Draw the outer border
    //    //var outerBorderPen = new Pen(new SolidColorBrush(Color.FromArgb(255, 243, 243, 243)), 1);
    //    //drawingContext.DrawEllipse(null, outerBorderPen, new Point(x, y), 8, 8);

    //    if (_image == null) return;

    //    drawingContext.DrawImage(_image, new Rect(x - _image.PixelWidth / 2f, y - _image.PixelHeight / 2f, _image.PixelWidth, _image.PixelHeight));
    //}

    public Cable Connect(AudioJack other)
    {
        var startPoint = Center;
        var endPoint = new Point(other.X - X + other.ActualWidth / 2, other.Y - Y + other.ActualHeight / 2);

        if (ToY == 0)
        {
            // we are input, so use color our cable
            //var cable = CreateCable(startPoint, endPoint, CableColor);
            var cable = new Cable(startPoint, endPoint, CableColor);

            other.ConnectedTo.Add(cable);
            ConnectedFrom = cable;

            return cable;
        }
        else
        {
            var cable = CreateCable(startPoint, endPoint, other.CableColor);

            ConnectedTo.Add(cable);
            other.ConnectedFrom = cable;
            return cable;
        }
    }

    private Cable CreateCable(Point startPoint, Point endPoint, Color color)
    {
        var cable = new Cable(startPoint, endPoint, color);

        Canvas.SetLeft(cable, X);
        Canvas.SetTop(cable, Y);

        return cable;
    }

    private Point Center => new Point(ActualWidth / 2, ActualHeight / 2);

    public double X => Canvas.GetLeft(this);
    public double Y => Canvas.GetTop(this);
}
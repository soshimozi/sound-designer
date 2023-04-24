using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using JackTest.Controls;

namespace SoundDesigner.Controls;

public class ControlPanel : System.Windows.Controls.Control
{
    static ControlPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlPanel), new FrameworkPropertyMetadata(typeof(ControlPanel)));
    }

    protected Canvas? PanelCanvas;

    private bool _isDragging = false;
    private Point? offset = null;
    private AudioJack? _draggingFrom = null;
    private Cable? _draggingCable = null;
    public ControlPanel()
    {
        MouseMove += ControlPanel_MouseMove;
        MouseLeftButtonDown += ControlPanel_MouseLeftButtonDown;
        MouseLeftButtonUp += ControlPanel_MouseLeftButtonUp;
    }

    private Dictionary<string, System.Windows.Controls.Control> _controls = new Dictionary<string, System.Windows.Controls.Control>();
    private Dictionary<string, ControlPanel> _panels = new Dictionary<string, ControlPanel>();

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        PanelCanvas = (Canvas)Template.FindName("PART_PanelCanvas", this);
        PanelCanvas.Background = new SolidColorBrush(Color.FromRgb(56, 56, 56));
    }

    private void ControlPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        var from = _draggingFrom;
        var cable = _draggingCable;

        _isDragging = false;
        _draggingFrom = null;
        _draggingCable = null;

        ReleaseMouseCapture();

        if (from == null || PanelCanvas == null || cable == null) return;

        // we are done with this cable for now
        PanelCanvas.Children.Remove(cable);

        var audioJack = FindAudioJackFromHitPoint(e.GetPosition(PanelCanvas), PanelCanvas);
        if (audioJack == null || audioJack == from) return;
        if (audioJack.ToY != from.ToY)
        {
            Connect(from, audioJack);
        }
    }

    private void ControlPanel_MouseMove(object sender, MouseEventArgs e)
    {

        if (!_isDragging || PanelCanvas == null || _draggingCable == null || _draggingFrom == null) return;

        _draggingCable.EndPoint = e.GetPosition(this);
        _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingNoDrop;

        var audioJack = FindAudioJackFromHitPoint(e.GetPosition(PanelCanvas), PanelCanvas);
        if (audioJack == null) return;

        // can only drop inputs on outputs and vice versa
        if (audioJack.ToY != _draggingFrom.ToY)
        {
            _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingCanDrop;
        }

    }

    private void ControlPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (PanelCanvas == null) return;

        var audioJack = FindAudioJackFromHitPoint(e.GetPosition(PanelCanvas), PanelCanvas);
        if(audioJack == null) return;

        // Start dragging the control
        _isDragging = true;
        _draggingFrom = audioJack;

        offset = new Point(audioJack.X,audioJack.Y);

        _draggingCable = new Cable(offset, offset, Color.FromRgb(128,128,128));
        PanelCanvas.Children.Add(_draggingCable);

        if (_draggingFrom.ConnectedFrom != null)
        {
            // if we are an input jack, remove the current cable
            PanelCanvas.Children.Remove(_draggingFrom.ConnectedFrom);
            _draggingFrom.ConnectedFrom = null;
        }

        CaptureMouse();
        ControlPanel_MouseMove(sender, e);
    }

    public void DrawRoundedRectangle(double x, double y, double width, double height, double radius, Brush fill, Brush stroke, bool isStroke = true)
    {
        var rectangle = new Rectangle
        {
            RadiusX = radius,
            RadiusY = radius,
            Fill = fill,
            Stroke = isStroke ? stroke : null,
            Width = width,
            Height = height
        };

        Canvas.SetLeft(rectangle, x);
        Canvas.SetTop(rectangle, y);


        PanelCanvas?.Children.Add(rectangle);
    }

    public void DrawDownArrow(double x, double y, double sz, Color color, Color border)
    {
        var polygon = new Polygon
        {
            Points = new PointCollection
            {
                new Point(x - sz, y),
                new Point(x, y + sz),
                new Point(x + sz, y)
            },
            Fill = new SolidColorBrush(color),
            Stroke = new SolidColorBrush(border),
            StrokeThickness = 0.5
                
        };

        PanelCanvas?.Children.Add(polygon);
    }

    public AudioJack AddJack(string name, int x, int y, int toy, Color? cableColor = null)
    {
        var jack = new AudioJack();
        
        jack.Name = name;
        jack.ToY = toy;
        //jack.CableColor = cableColor ?? Colors.White;
       
        Canvas.SetLeft(jack, x);
        Canvas.SetTop(jack, y);

        PanelCanvas?.Children.Add(jack);
        _controls[name] = jack;
        return jack;
    }

    public SliderControl AddSlider(string name, int x, int y, int width, int height,
        string imageSource, int steps, double defaultValue)
    {
        var slider = new SliderControl(x, y, width, height, steps, imageSource, defaultValue);
        PanelCanvas?.Children.Add(slider);
        _controls[name] = slider;

        return slider;
    }


    public HorizontalSwitch AddHorizontalSwitch(string name, int x, int y, int width, int height,
        string imageSource, int steps, double defaultValue)
    {
        var hsw = new HorizontalSwitch(x, y, width, height, imageSource, steps, defaultValue);
        PanelCanvas?.Children.Add(hsw);
        _controls[name] = hsw;

        return hsw;
    }

    public VerticalSwitch AddVerticalSwitch(string name, int x, int y, int width, int height,
        string imageSource, int steps, double defaultValue)
    {
        var vsw = new VerticalSwitch(x, y, width, height, imageSource, steps, defaultValue);
        PanelCanvas?.Children.Add(vsw);
        _controls[name] = vsw;

        return vsw;
    }

    public RectangleControl AddRectangle(string name, int x, int y, int width, int height)
    {
        var rectangle = new RectangleControl
        {
            Width = width,
            Height = height
        };

        Canvas.SetLeft(rectangle, x);
        Canvas.SetTop(rectangle, y);

        PanelCanvas?.Children.Add(rectangle);

        _controls[name] = rectangle;
        return rectangle;
    }

    public void AddPanel(string name, ControlPanel panel)
    {
        _panels[name] = panel;
    }

    public ControlPanel? GetPanel(string name)
    {
        if (_panels.ContainsKey(name))
        {
            return _panels[name];
        }
        return null;
    }

    public void Connect(AudioJack jackFrom, AudioJack jackTo)
    {
        if (jackFrom.ToY == 0)
        {
            if (jackFrom.ConnectedFrom != null)
            {
                PanelCanvas?.Children.Remove(jackFrom.ConnectedFrom);
            }
        }
        else if (jackTo.ToY == 0)
        {
            if (jackTo.ConnectedFrom != null)
            {
                PanelCanvas?.Children.Remove(jackTo.ConnectedFrom);
            }
        }

        var cable = jackFrom.Connect(jackTo);
        PanelCanvas?.Children.Add(cable);
    }

    public void Connect(string from, string to)
    {
        var jackFrom = _controls.GetValueOrDefault(from) as AudioJack;
        var jackTo = _controls.GetValueOrDefault(to) as AudioJack;
        if (jackFrom == null || jackTo == null) return;

        Connect(jackFrom, jackTo);

    }

    public void Disconnect(string from, string to)
    {
        var jackFrom = _controls.GetValueOrDefault(from) as AudioJack;
        var jackTo = _controls.GetValueOrDefault(to) as AudioJack;
        if (jackFrom == null || jackTo == null) return;

        var cable = jackTo.ConnectedFrom;
        if(cable == null) return;

        jackTo.ConnectedFrom = null;
        jackFrom.ConnectedTo.Remove(cable);

        PanelCanvas?.Children.Remove(cable);
    }

    public void DrawText(string text, double x, double y, Color color, HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top)
    {
        var textBlock = new TextBlock
        {
            Text = text,
            FontSize = 10,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(color)
        };

        var verticalAlignment = valign;
        var horizontalAlignment = halign;

        textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        textBlock.Arrange(new Rect(textBlock.DesiredSize));

        // Set the vertical and horizontal alignment
        textBlock.VerticalAlignment = verticalAlignment;
        textBlock.HorizontalAlignment = horizontalAlignment;

        // Measure the size of the text
        var textWidth = textBlock.ActualWidth;
        var textHeight = textBlock.ActualHeight;

        //Canvas.SetLeft(textBlock, x);
        //Canvas.SetTop(textBlock, y);

        switch (verticalAlignment)
        {
            // Set the position based on the alignment
            case VerticalAlignment.Top:
                Canvas.SetTop(textBlock, y);
                break;
            case VerticalAlignment.Bottom:
                Canvas.SetTop(textBlock, y - textHeight);
                break;
            default:
                Canvas.SetTop(textBlock, y - textHeight / 2);
                break;
        }

        switch (horizontalAlignment)
        {
            case HorizontalAlignment.Left:
                Canvas.SetLeft(textBlock, x);
                break;
            case HorizontalAlignment.Right:
                Canvas.SetLeft(textBlock, x - textWidth);
                break;
            default:
                Canvas.SetLeft(textBlock, x - textWidth / 2);
                break;
        }

        PanelCanvas?.Children.Add(textBlock);
    }

    private static AudioJack? FindAudioJackFromHitPoint(Point hitPoint, Canvas canvas)
    {
        var hitResults = new List<HitTestResult>();
        VisualTreeHelper.HitTest(canvas, null, result =>
        {
            hitResults.Add(result);
            return HitTestResultBehavior.Continue;
        }, new PointHitTestParameters(hitPoint));

        var audioJack = hitResults.Select(result => result.VisualHit)
            .OfType<AudioJack>().FirstOrDefault();

        if (audioJack != null) return audioJack;

        var image = hitResults.Select(result => result.VisualHit)
            .OfType<Image>().FirstOrDefault();

        return image?.FindAncestor<AudioJack>();
    }

}
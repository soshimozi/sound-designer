using System.Collections.Generic;
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
        _isDragging = false;
        if (_draggingFrom == null || PanelCanvas == null || _draggingCable == null) return;

        // we are done with this cable for now
        PanelCanvas.Children.Remove(_draggingCable);
        ReleaseMouseCapture();

        var result = VisualTreeHelper.HitTest(PanelCanvas, e.GetPosition(PanelCanvas));
        if (result is not { VisualHit: AudioJack control }) return;

        if (control != _draggingFrom)
        {
            if (_draggingFrom.ToY == 0 && control.ToY != 0)
            {
                Connect(control.Name, _draggingFrom.Name);
            }
            else if (_draggingFrom.ToY != 0 && control.ToY == 0)
            {
                Connect(_draggingFrom.Name, control.Name);
            }

        }

        _draggingFrom = null;
        _draggingCable = null;

    }

    private void ControlPanel_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging || PanelCanvas == null || _draggingCable == null || _draggingFrom == null) return;


        var result = VisualTreeHelper.HitTest(PanelCanvas, e.GetPosition(PanelCanvas));

        // ignore cable hits
        if (result is { VisualHit: Cable cable }) return;

        // Update the position of the dragged control
        //_draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingCanDrop;
        if (result is not { VisualHit: AudioJack control })
        {
            _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingNoDrop;
        }
        else
        {
            // can't drop on itself
            if (control == _draggingFrom) _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingNoDrop;
            else
            {
                // now we need to check input vs output
                if (_draggingFrom.ToY == 0 && control.ToY != 0)
                {
                    _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingCanDrop;
                } else if (_draggingFrom.ToY != 0 && control.ToY == 0)
                {
                    _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingCanDrop;
                }
                else
                {
                    _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingNoDrop;
                }
            }

        }

        _draggingCable.EndPoint = e.GetPosition(this);
    }

    private void ControlPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (PanelCanvas == null) return;

        // Perform hit testing to see if the mouse is over a control that can be dragged
        var result = VisualTreeHelper.HitTest(PanelCanvas, e.GetPosition(PanelCanvas));
        if (result is not { VisualHit: AudioJack control }) return;

        // Start dragging the control
        _isDragging = true;
        _draggingFrom = control;

        //offset = e.GetPosition(this);

        offset = new Point(_draggingFrom.X, _draggingFrom.Y);

        _draggingCable = new Cable(offset, offset, Color.FromRgb(128,128,128));
        PanelCanvas.Children.Add(_draggingCable);


        if (_draggingFrom.ConnectedFrom != null)
        {
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

    //public AudioJack AddJack(string name, int x, int y, int toy, Color? cableColor = null)
    //{
    //    var jack = new AudioJack(name, toy, "Assets/Possible_SVG_Jack.png", cableColor);

    //    Canvas.SetLeft(jack, x);
    //    Canvas.SetTop(jack, y);

    //    PanelCanvas?.Children.Add(jack);
    //    _controls[name] = jack;
    //    return jack;
    //}

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

    public void Connect(string from, string to)
    {
        var jackFrom = _controls.GetValueOrDefault(from) as AudioJack;
        var jackTo = _controls.GetValueOrDefault(to) as AudioJack;
        if (jackFrom == null || jackTo == null) return;


        if (jackFrom.ToY == 0)
        {
            if (jackFrom.ConnectedFrom != null)
            {
                PanelCanvas?.Children.Remove(jackFrom.ConnectedFrom);
            }
        } else if (jackTo.ToY == 0)
        {
            if (jackTo.ConnectedFrom != null)
            {
                PanelCanvas?.Children.Remove(jackTo.ConnectedFrom);
            }
        }

        var cable = jackFrom.Connect(jackTo);
        PanelCanvas?.Children.Add(cable);
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

}
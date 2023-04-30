using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SoundDesigner.Control;

public class Cable : System.Windows.Controls.Control
{
    public enum DraggableStateEnum
    {
        NoDrag,
        DraggingCanDrop,
        DraggingNoDrop,
    }

    //private Point? _start;
    private Color _color;

    public Cable(Point? start, Point? end, Color color)
    {
        StartPoint = start;
        EndPoint = end;
        _color = color;
    }

    public DraggableStateEnum DraggableState
    {
        get => (DraggableStateEnum)GetValue(DraggableStateProperty);
        set => SetValue(DraggableStateProperty, value);
    }

    public static readonly DependencyProperty DraggableStateProperty = DependencyProperty.Register(
        "DraggableState", typeof(DraggableStateEnum), typeof(Cable), new FrameworkPropertyMetadata(DraggableStateEnum.NoDrag, OnDraggableStateChanged));

    private static void OnDraggableStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement fe)
        {
            fe.InvalidateVisual(); // Redraw the cable when the value changes
        }
    }

    public Point? EndPoint
    {
        get => (Point?)GetValue(EndPointProperty);
        set => SetValue(EndPointProperty, value);
    }

    public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
        "EndPoint", typeof(Point?), typeof(Cable), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public Point? StartPoint
    {
        get => (Point?)GetValue(StartPointProperty);
        set => SetValue(StartPointProperty, value);
    }

    public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
        "StartPoint", typeof(Point?), typeof(Cable), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        var color = _color;
        var startPoint = StartPoint ?? new Point();
        var endPoint = EndPoint ?? new Point();

        int r = color.R;
        int g = color.G;
        int b = color.B;

        const int cableThickness = 5;

        for (var i = cableThickness; i > 0; --i)
        {
            var adjustedColor = Color.FromArgb(255, (byte)r, (byte)g, (byte)b);

            var path = new Path
            {
                Stroke = new SolidColorBrush(adjustedColor),
                StrokeThickness = i,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
            };

            var geometry = new PathGeometry();
            var figure = new PathFigure { StartPoint = startPoint };
            var bezierSegment = new BezierSegment
            {
                Point1 = new Point((startPoint.X + endPoint.X) / 2, Math.Max(startPoint.Y, endPoint.Y) + 40),
                Point2 = new Point((startPoint.X + endPoint.X) / 2, Math.Max(startPoint.Y, endPoint.Y) + 40),
                Point3 = endPoint,
            };

            figure.Segments.Add(bezierSegment);
            geometry.Figures.Add(figure);
            path.Data = geometry;


            var style = Color.FromArgb(255, (byte)r, (byte)g, (byte)b);
            var pen = new Pen(new SolidColorBrush(style), i);

            drawingContext.DrawGeometry(null, pen, geometry);

            if (r < 200)
                r += (200 - r) / 4;
            if (g < 200)
                g += (200 - g) / 4;
            if (b < 200)
                b += (200 - b) / 4;
        }

        switch (DraggableState)
        {
            case DraggableStateEnum.DraggingCanDrop:
                // Draw green O
                DrawDragSuccess(endPoint, drawingContext);

                //var pen = new Pen(Brushes.Green, 2);
                //drawingContext.DrawEllipse(null, pen, new Point(endPoint.X, endPoint.Y), 10, 10);
                break;
            case DraggableStateEnum.DraggingNoDrop:
                DrawDragNoSuccess(endPoint, drawingContext);
                //// Draw red X
                //var pen2 = new Pen(Brushes.Red, 2);
                //drawingContext.DrawLine(pen2, new Point(endPoint.X - 10, endPoint.Y - 10), new Point(endPoint.X + 10, endPoint.Y + 10));
                //drawingContext.DrawLine(pen2, new Point(endPoint.X + 10, endPoint.Y - 10), new Point(endPoint.X - 10, endPoint.Y + 10));
                break;
            case DraggableStateEnum.NoDrag:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DrawDragSuccess(Point endPoint, DrawingContext drawingContext)
    {
        var pen = new Pen(Brushes.Green, 2);
        drawingContext.DrawEllipse(null, pen, new Point(endPoint.X, endPoint.Y), 10, 10);
    }

    private void DrawDragNoSuccess(Point endPoint, DrawingContext drawingContext)
    {
        var pen = new Pen(Brushes.Red, 2);
        drawingContext.DrawEllipse(null, pen, new Point(endPoint.X, endPoint.Y), 10, 10);

        //var pen2 = new Pen(Brushes.Red, 2);
        drawingContext.DrawLine(pen, new Point(endPoint.X - 10, endPoint.Y - 10), new Point(endPoint.X + 10, endPoint.Y + 10));
        drawingContext.DrawLine(pen, new Point(endPoint.X + 10, endPoint.Y - 10), new Point(endPoint.X - 10, endPoint.Y + 10));

    }

}
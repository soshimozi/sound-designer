using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SoundDesigner.Controls
{
    public class Cable : System.Windows.Controls.Control
    {
        public enum DraggableStateEnum
        {
            NoDrag,
            DraggingCanDrop,
            DraggingNoDrop,
        }

        private Point? _start;
        private Color _color;

        public Cable(Point? start, Point? end, Color color)
        {
            _start = start;
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
            if (d is Cable cable)
            {
                cable.InvalidateVisual(); // Redraw the cable when the value changes
            }
        }

        public Point? EndPoint
        {
            get => (Point?)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }

        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint", typeof(Point?), typeof(Cable), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var color = _color;
            var startPoint = _start ?? new Point();
            var endPoint = EndPoint ?? new Point();

            int r = color.R;
            int g = color.G;
            int b = color.B;

            for (int i = 4; i > 0; --i)
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
                    var pen = new Pen(Brushes.Green, 2);
                    drawingContext.DrawEllipse(null, pen, new Point(endPoint.X, endPoint.Y), 10, 10);
                    break;
                case DraggableStateEnum.DraggingNoDrop:
                    // Draw red X
                    var pen2 = new Pen(Brushes.Red, 2);
                    drawingContext.DrawLine(pen2, new Point(endPoint.X - 10, endPoint.Y - 10), new Point(endPoint.X + 10, endPoint.Y + 10));
                    drawingContext.DrawLine(pen2, new Point(endPoint.X + 10, endPoint.Y - 10), new Point(endPoint.X - 10, endPoint.Y + 10));
                    break;
                default:
                    // NoDrag, do nothing
                    break;
            }
        }
    }
}

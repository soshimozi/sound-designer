using System.Windows;
using System.Windows.Media;

namespace SoundDesigner.Controls;

public class RectangleControl : System.Windows.Controls.Control
{
    static RectangleControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleControl), new FrameworkPropertyMetadata(typeof(RectangleControl)));
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        Rect rectangle = new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight));
        Pen pen = new Pen(Brushes.Black, 1);

        drawingContext.DrawRectangle(null, pen, rectangle);
    }
}
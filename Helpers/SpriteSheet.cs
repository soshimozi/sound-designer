using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SoundDesigner.Controls;

namespace SoundDesigner.Helpers;

public class SpriteSheet : DependencyObject
{
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        "Source", typeof(ImageSource), typeof(SpriteSheet), new PropertyMetadata(null));

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        "Orientation", typeof(Orientation), typeof(SpriteSheet), new PropertyMetadata(Orientation.Horizontal));

    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }


    public static readonly DependencyProperty RangeProperty =
        DependencyProperty.Register("Range", typeof(IntegerRange), typeof(SpriteSheet), new PropertyMetadata(null));

    public IntegerRange Range
    {
        get => (IntegerRange)GetValue(RangeProperty);
        set => SetValue(RangeProperty, value);
    }

    public static readonly DependencyProperty CellsProperty = DependencyProperty.Register(
        "Cells", typeof(int), typeof(SpriteSheet), new PropertyMetadata(1));

    public int Cells
    {
        get => (int)GetValue(CellsProperty);
        set => SetValue(CellsProperty, value);
    }

    private List<ImageSource> _croppedImages;
    public List<ImageSource> CroppedImages
    {
        get
        {
            if (_croppedImages == null)
            {
                GenerateCroppedImages();
            }
            return _croppedImages;
        }
    }


    private void GenerateCroppedImages()
    {
        _croppedImages = new List<ImageSource>();

        if (Source == null || Cells <= 0) return;

        var spriteWidth = Source.Width / (Orientation == Orientation.Horizontal ? Cells : 1);
        var spriteHeight = Source.Height / (Orientation == Orientation.Vertical ? Cells : 1);


        int start = Range?.Start ?? 0;
        int end = Range?.End ?? (Cells - 1);

        for (int i = start; i <= end; i++)
        {
            var x = Orientation == Orientation.Horizontal ? i * spriteWidth : 0;
            var y = Orientation == Orientation.Vertical ? i * spriteHeight : 0;

            var rect = new Int32Rect((int)x, (int)y, (int)spriteWidth, (int)spriteHeight);
            var croppedImage = new CroppedBitmap((BitmapSource)Source, rect);

            _croppedImages.Add(croppedImage);
        }
    }

}
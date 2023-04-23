using System.Globalization;
using System.Windows.Data;
using System;
using System.Windows;

namespace SoundDesigner.Converters;

public class NoteToCanvasLocationConverter : IMultiValueConverter
{
    public object Convert(object[]? values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 4 || values[0] is not string || values[1] is not double || values[2] is not double || values[3] is not HorizontalAlignment)
            return 0;

        var noteAndOctave = (string)values[0];
        var totalWidth = (double)values[1];
        var containerWidth = (double)values[2];
        var alignment = (HorizontalAlignment)values[3];

        // Calculate the original left position based on the noteAndOctave property
        var leftPosition = CalculateLeftPositionBasedOnNoteAndOctave(noteAndOctave);

        // Calculate the offset based on the alignment
        double offset = 0;
        switch (alignment)
        {
            case HorizontalAlignment.Center:
                offset = (containerWidth - totalWidth) / 2;
                break;
            case HorizontalAlignment.Right:
                offset = containerWidth - totalWidth;
                break;
            case HorizontalAlignment.Left:
            case HorizontalAlignment.Stretch:
            default:
                break;
        }

        // Add the offset to the original left position
        return leftPosition + offset;
    }

    private double CalculateLeftPositionBasedOnNoteAndOctave(string noteAndOctave)
    {
        // Your existing logic to calculate the left position
        var octave = int.Parse(noteAndOctave[^1].ToString());
        var note = noteAndOctave.Substring(0, noteAndOctave.Length - 1);

        double baseOffset = 40 * octave * 7;

        return note switch
        {
            "C" => 0 + baseOffset,
            "C#" => 25 + baseOffset,
            "D" => 40 + baseOffset,
            "D#" => 65 + baseOffset,
            "E" => 80 + baseOffset,
            "F" => 120 + baseOffset,
            "F#" => 145 + baseOffset,
            "G" => 160 + baseOffset,
            "G#" => 185 + baseOffset,
            "A" => 200 + baseOffset,
            "A#" => 225 + baseOffset,
            "B" => 240 + baseOffset,
            _ => 0
        };
    }


    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
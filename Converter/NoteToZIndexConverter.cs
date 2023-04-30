using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundDesigner.Converter;

public class NoteToZIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string noteAndOctave)
        {
            int octave = int.Parse(noteAndOctave[^1].ToString());
            string note = noteAndOctave.Substring(0, noteAndOctave.Length - 1);

            double baseOffset = 40 * octave * 7;

            switch (note)
            {
                case "C": return 1;
                case "C#": return 999;
                case "D": return 1;
                case "D#": return 999;
                case "E": return 1;
                case "F": return 1;
                case "F#": return 999;
                case "G": return 1;
                case "G#": return 999;
                case "A": return 1;
                case "A#": return 999;
                case "B": return 1;
            }
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
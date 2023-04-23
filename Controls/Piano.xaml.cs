using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SoundDesigner.Models;

namespace SoundDesigner.Controls;

/// <summary>
/// Interaction logic for Piano.xaml
/// </summary>
public partial class Piano : UserControl
{
    public ObservableCollection<PianoKeyModel> Keys { get; set; }

    public ICommand KeyPressCommand
    {
        get { return (ICommand)GetValue(KeyPressCommandProperty); }
        set { SetValue(KeyPressCommandProperty, value); }
    }

    public static readonly DependencyProperty KeyPressCommandProperty =
        DependencyProperty.Register("KeyPressCommand", typeof(ICommand), typeof(Piano), new PropertyMetadata(null));

    public Piano()
    {
        InitializeComponent();

        Keys = new ObservableCollection<PianoKeyModel>();
        for (int octave = 0; octave < 2; octave++)
        {
            Keys.Add(new PianoKeyModel { Note = "C", IsBlackKey = false, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "C#", IsBlackKey = true, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "D", IsBlackKey = false, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "D#", IsBlackKey = true, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "E", IsBlackKey = false, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "F", IsBlackKey = false, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "F#", IsBlackKey = true, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "G", IsBlackKey = false, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "G#", IsBlackKey = true, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "A", IsBlackKey = false, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "A#", IsBlackKey = true, Octave = octave });
            Keys.Add(new PianoKeyModel { Note = "B", IsBlackKey = false, Octave = octave });
        }

        DataContext = this;
    }
}
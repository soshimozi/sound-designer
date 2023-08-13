using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NAudio.Wave;
using SoundDesigner.Event;
using SoundDesigner.Helper;
using SoundDesigner.Lib;
using SoundDesigner.Lib.CommandRouting;
using SoundDesigner.Model;
using SynthesizerEngine.Core.Audio;
using SynthesizerEngine.Core.Audio.Interface;
using SynthesizerEngine.DSP;
using SynthesizerEngine.Scale;

namespace SoundDesigner.ViewModel;

public record WaveformSelection(string? Description, WaveShape Shape);

public class SoundGenerationViewModel : ViewModelBase
{
    private float _attack = 0;
    private float _sustain = 100f;
    private float _release = 0;
    private float _decay = 0;
    private double _detune = 0;
    private float _modFrequency = 0;
    private WaveformSelection? _selectedModShape;

    private WaveformSelection? _osc1Waveform;
    private string? _osc1Octave = "16'";

    private WaveformSelection? _osc2Waveform;
    private string? _osc2Octave = "8'";

    private float _osc1Tremolo = 0;
    private float _osc2Tremolo = 0;

    private float _osc1Detune = 0;
    private float _osc2Detune = 0;
    private float _osc1Mix = 0;
    private float _osc2Mix = 0;

    private float _filterCutoff = 20;
    private float _filterQ = 0;
    private float _filterModulation = 0;
    private float _filterEnvelopeMix = 0;

    private float _filterEnvelopeAttack = 20;
    private float _filterEnvelopeSustain = 90;
    private float _filterEnvelopeRelease = 10;
    private float _filterEnvelopeDecay = 10;

    private float _volumeEnvelopeAttack = 20;
    private float _volumeEnvelopeSustain = 90;
    private float _volumeEnvelopeRelease = 10;
    private float _volumeEnvelopeDecay = 10;

    private float _reverb = 0;
    private float _drive = 0;
    private float _masterVolume = 0;

    private bool _showModLabel = false;

    private bool _showMasterVolumeLabel = false;
    private bool _showReverbLabel = false;
    private bool _showDriveLabel = false;

    public DelegateCommand? ButtonPressedCommand { get; }
    public DelegateCommand? ButtonReleasedCommand { get; }

    public ObservableCollection<string> MidiDevices { get; }

    private string? _keyboardOctave = "0";
    private string? _selectedDevice = "";
    public ObservableCollection<PianoKeyModel> Keys { get; set; }

    private readonly RoundRobinObjectPool<SynthVoice> _voices;

    private readonly AudioProvider _audioProvider = new AudioProvider();

    private readonly List<SynthVoice> _playingVoices = new List<SynthVoice>();

    private readonly IEventAggregator _eventAggregator;

    public SoundGenerationViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        Shapes = new WaveformSelection[]
        {
            new WaveformSelection("Sine", WaveShape.Sine),
            new WaveformSelection("Sawtooth", WaveShape.Sawtooth),
            new WaveformSelection("Inverse Sawtooth", WaveShape.InvSawtooth),
            new WaveformSelection("Square", WaveShape.Square),
            new WaveformSelection("Triangle", WaveShape.Triangle),
            new WaveformSelection("Pulse", WaveShape.Pulse),
        };

        Osc1Waveform = Shapes[0];
        Osc2Waveform = Shapes[1];

        var voiceFactory = new Func<SynthVoice>(() =>
        {
            var voice = new Voice(_audioProvider, 0);
            voice.Connect(_audioProvider.Output);
            voice.NoteOff();

            return new SynthVoice(false, voice, 0);
        });

        _voices = new(8, voiceFactory);
        //for (var i = 0; i < 5; i++)
        //{
        //    _voices[i] = new SynthVoice(false, new Voice(_audioProvider, 0), 0);
        //    _voices[i].Voice.Connect(_audioProvider.Output);
        //    _voices[i].Voice.NoteOff();
        //}

        ButtonReleasedCommand = new DelegateCommand
        {
            CommandAction = (o) =>
            {
                if (o is not PianoKeyModel vm) return;

                var playingVoice = _playingVoices.FirstOrDefault(pv => pv.NoteIndex == vm.NoteIndex);
                if(playingVoice == null) return;

                playingVoice.Playing = false;
                playingVoice.Voice.NoteOff();
                playingVoice.NoteIndex = -1;

                _playingVoices.Remove(playingVoice);
            },
            CanExecuteFunc = (o) => true
        };

        ButtonPressedCommand = new DelegateCommand
        {
            CommandAction = (o) =>
            {
                if (o is not PianoKeyModel vm) return;

                // find an empty voice
                var voice = _voices.GetNext((v) => !v.Playing);

                if(voice == null) return;

                voice.Voice.NoteOff();

                voice.Playing = true;

                if (KeyboardOctave == null) return;

                var noteFrequency = GetFrequencyFromNoteAndOctave(vm.Note, vm.Octave + int.Parse(KeyboardOctave));
                voice.VolumeAttack = Map(_volumeEnvelopeAttack, 0, 100, 0.01, 2500);
                voice.VolumeDecay = Map(_volumeEnvelopeDecay, 0, 100, 0.01, 2500);
                voice.VolumeRelease = Map(_volumeEnvelopeRelease, 0, 100, 0.01, 2500);
                voice.VolumeSustain = Map(_volumeEnvelopeSustain, 0, 100, 0.01, 1);

                voice.Voice.NoteOn(noteFrequency);
                voice.NoteIndex = vm.NoteIndex;
                voice.SetOsc1WaveShape(_osc1Waveform?.Shape ?? WaveShape.Sine);
                voice.SetOsc2WaveShape(_osc2Waveform?.Shape ?? WaveShape.Sine);

                _playingVoices.Add(voice);

            },

            CanExecuteFunc = (o) => true
        };

        MidiDevices = new ObservableCollection<string>();

        Keys = new ObservableCollection<PianoKeyModel>();
        Keys.CollectionChanged += Keys_CollectionChanged;

        for (var octave = 0; octave < 2; octave++)
        {
            Keys.Add(new PianoKeyModel { Note = "C", IsBlackKey = false, Octave = octave, NoteIndex = octave * 12 });
            Keys.Add(new PianoKeyModel { Note = "C#", IsBlackKey = true, Octave = octave, NoteIndex = octave * 12 + 1 });
            Keys.Add(new PianoKeyModel { Note = "D", IsBlackKey = false, Octave = octave, NoteIndex = octave * 12 + 2 });
            Keys.Add(new PianoKeyModel { Note = "D#", IsBlackKey = true, Octave = octave, NoteIndex = octave * 12 + 3 });
            Keys.Add(new PianoKeyModel { Note = "E", IsBlackKey = false, Octave = octave, NoteIndex = octave * 12 + 4 });
            Keys.Add(new PianoKeyModel { Note = "F", IsBlackKey = false, Octave = octave, NoteIndex = octave * 12 + 5 });
            Keys.Add(new PianoKeyModel { Note = "F#", IsBlackKey = true, Octave = octave, NoteIndex = octave * 12 + 6 });
            Keys.Add(new PianoKeyModel { Note = "G", IsBlackKey = false, Octave = octave, NoteIndex = octave * 12 + 7 });
            Keys.Add(new PianoKeyModel { Note = "G#", IsBlackKey = true, Octave = octave, NoteIndex = octave * 12 + 8 });
            Keys.Add(new PianoKeyModel { Note = "A", IsBlackKey = false, Octave = octave, NoteIndex = octave * 12 + 9 });
            Keys.Add(new PianoKeyModel { Note = "A#", IsBlackKey = true, Octave = octave, NoteIndex = octave * 12 + 10 });
            Keys.Add(new PianoKeyModel { Note = "B", IsBlackKey = false, Octave = octave, NoteIndex = octave * 12 + 11 });
        }

        var audioOutput = new WasapiOut();
        audioOutput.Init(_audioProvider);
        audioOutput.Play();

        _eventAggregator.Subscribe<WindowClosingEvent>((o) =>
        {
            audioOutput.Stop();
        });
    }

    private static float Map(float value, double inputStart, double inputEnd, double outputStart, double outputEnd)
    {
        return (float)(((value - inputStart) / (inputEnd - inputStart)) * (outputEnd - outputStart) + outputStart);
    }


    private int GetNoteIndexFromString(string note)
    {
        switch (note)
        {
            case "C": return 0;
            case "C#": return 1;
            case "D": return 2;
            case "D#": return 3;
            case "E": return 4;
            case "F": return 5;
            case "F#": return 6;
            case "G": return 7;
            case "G#": return 8;
            case "A": return 9;
            case "A#": return 10;
            case "B": return 11;

            default: return 0;
        }
    }

    private double GetFrequencyFromNoteAndOctave(string note, int octave)
    {
        var index = GetNoteIndexFromString(note);
        const double A4TuningBase = 16.352d;
        var chromatic = new Scale(new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11});
        return chromatic.GetFrequency(index, A4TuningBase, octave + 3);
    }

    private void Keys_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateTotalKeysWidth();
    }


    private double _totalKeysWidth;

    public bool ShowModLabel
    {
        get => _showModLabel;
        set => SetProperty(ref _showModLabel, value);
    }

    public double TotalKeysWidth
    {
        get => _totalKeysWidth;
        set => SetProperty(ref _totalKeysWidth, value);
    }

    public string? SelectedDevice
    {
        get => _selectedDevice;
        set => SetProperty(ref _selectedDevice, value);
    }
    public string? KeyboardOctave
    {
        get => _keyboardOctave;
        set => SetProperty(ref _keyboardOctave, value);
    }

    public bool ShowDriveLabel
    {
        get => _showDriveLabel;
        set => SetProperty(ref _showDriveLabel, value);
    }

    public bool ShowReverbLabel
    {
        get => _showReverbLabel;
        set => SetProperty(ref _showReverbLabel, value);
    }
        
    public bool ShowMasterVolumeLabel
    {
        get => _showMasterVolumeLabel;
        set => SetProperty(ref _showMasterVolumeLabel, value);
    }

    public float Reverb
    {
        get => _reverb;
        set => SetProperty(ref _reverb, value);
    }

    public float Drive
    {
        get => _drive;
        set => SetProperty(ref _drive, value);
    }

    public float MasterVolume
    {
        get => _masterVolume;
        set => SetProperty(ref _masterVolume, value);
    }

    public float VolumeEnvelopeRelease
    {
        get => _volumeEnvelopeRelease;
        set => SetProperty(ref _volumeEnvelopeRelease, value);
    }

    public float VolumeEnvelopeDecay
    {
        get => _volumeEnvelopeDecay;
        set => SetProperty(ref _volumeEnvelopeDecay, value);
    }

    public float VolumeEnvelopeSustain
    {
        get => _volumeEnvelopeSustain;
        set => SetProperty(ref _volumeEnvelopeSustain, value);
    }

    public float VolumeEnvelopeAttack
    {
        get => _volumeEnvelopeAttack;
        set => SetProperty(ref _volumeEnvelopeAttack, value);
    }

    public float FilterEnvelopeRelease
    {
        get => _filterEnvelopeRelease;
        set => SetProperty(ref _filterEnvelopeRelease, value);
    }

    public float FilterEnvelopeDecay
    {
        get => _filterEnvelopeDecay;
        set => SetProperty(ref _filterEnvelopeDecay, value);
    }

    public float FilterEnvelopeSustain
    {
        get => _filterEnvelopeSustain;
        set => SetProperty(ref _filterEnvelopeSustain, value);
    }

    public float FilterEnvelopeAttack
    {
        get => _filterEnvelopeAttack;
        set => SetProperty(ref _filterEnvelopeAttack, value);
    }

    public float FilterEnvelopeMix
    {
        get => _filterEnvelopeMix;
        set => SetProperty(ref _filterEnvelopeMix, value);
    }


    public float FilterModulation
    {
        get => _filterModulation;
        set => SetProperty(ref _filterModulation, value);
    }

    public float FilterQ
    {
        get => _filterQ;
        set => SetProperty(ref _filterQ, value);
    }


    public float FilterCutoff
    {
        get => _filterCutoff;
        set => SetProperty(ref _filterCutoff, value);
    }

    public float Osc1Detune
    {
        get => _osc1Detune;
        set => SetProperty(ref _osc1Detune, value);
    }

    public float Osc2Detune
    {
        get => _osc2Detune;
        set => SetProperty(ref _osc2Detune, value);
    }

    public float Osc1Mix
    {
        get => (float)_osc1Mix;
        set => SetProperty(ref _osc1Mix, value);
    }
    public float Osc2Mix
    {
        get => (float)_osc2Mix;
        set => SetProperty(ref _osc2Mix, value);
    }

    public float Osc1Tremolo
    {
        get => _osc1Tremolo;
        set => SetProperty(ref _osc1Tremolo, value);
    }


    public float Osc2Tremolo
    {
        get => _osc2Tremolo;
        set => SetProperty(ref _osc2Tremolo, value);
    }

    public float ModFrequency
    {
        get => _modFrequency;
        set => SetProperty(ref _modFrequency, value);
    }

    public WaveformSelection[] Shapes { get; }

    public string[] KeyboardOctaves { get; } = new[] { "+3", "+2", "+1", "0", "-1", "-2", "-3" };

    public WaveformSelection? SelectedShape
    {
        get => _selectedModShape;
        set => SetProperty(ref _selectedModShape, value);
    }

    public WaveformSelection? Osc1Waveform
    {
        get => _osc1Waveform;
        set => SetProperty(ref _osc1Waveform, value);
    }


    public string [] Osc1Octaves { get; } = new[] { "32'", "16'", "8'" };


    public string? Osc1Octave
    {
        get => _osc1Octave;
        set => SetProperty(ref _osc1Octave, value);
    }
    public WaveformSelection? Osc2Waveform
    {
        get => _osc2Waveform;
        set => SetProperty(ref _osc2Waveform, value);
    }


    public string[] Osc2Octaves { get; } = new[] { "16'", "8'", "4'" };


    public string? Osc2Octave
    {
        get => _osc2Octave;
        set => SetProperty(ref _osc2Octave, value);
    }

    private void UpdateTotalKeysWidth()
    {
        var whiteKeysCount = Keys.Count(key => !key.IsBlackKey);
        TotalKeysWidth = whiteKeysCount * 40; // 40 is the width of the white keys
    }
}
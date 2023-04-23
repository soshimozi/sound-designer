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
using SoundDesigner.Lib;
using SoundDesigner.Lib.CommandRouting;
using SoundDesigner.Models;

namespace SoundDesigner.ViewModel
{
    internal class SoundGenerationViewModel : ViewModelBase
    {
        private float _attack = 0;
        private float _sustain = 100f;
        private float _release = 0;
        private float _decay = 0;
        private double _detune = 0;
        private float _modFrequency = 0;
        string[] _shapes = new[] { "sine", "square", "saw", "triangle" };
        private string _selectedModShape = "saw";

        private string _osc1Waveform = "sine";
        private readonly string[] _osc1Ocataves = new[] { "32'", "16'", "8'" };
        private string _osc1Octave = "16'";

        private string _osc2Waveform = "sine";
        private readonly string[] _osc2Ocataves = new[] { "16'", "8'", "4'" };
        private string _osc2Octave = "8'";

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

        private float _filterEnvelopeAttack = 0;
        private float _filterEnvelopeSustain = 0;
        private float _filterEnvelopeRelease = 0;
        private float _filterEnvelopeDecay = 0;

        private float _volumeEnvelopeAttack = 0;
        private float _volumeEnvelopeSustain = 0;
        private float _volumeEnvelopeRelease = 0;
        private float _volumeEnvelopeDecay = 0;

        private float _reverb = 0;
        private float _drive = 0;
        private float _masterVolume = 0;

        private bool _showModLabel = false;

        private bool _showMasterVolumeLabel = false;
        private bool _showReverbLabel = false;
        private bool _showDriveLabel = false;

        public DelegateCommand? KeyCommand { get; private set; }

        public ObservableCollection<string> MidiDevices { get; }

        private readonly string[] _keyboardOctaves = new[] { "+3", "+2", "+1", "0", "-1", "-2", "-3" };
        private string _keyboardOctave = "0";
        private string _selectedDevice = "";
        public ObservableCollection<PianoKeyModel> Keys { get; set; }

        public SoundGenerationViewModel()
        {

            KeyCommand = new DelegateCommand
            {
                CommandAction = (o) =>
                {
                    MessageBox.Show(o?.ToString());
                },
                CanExecuteFunc = (o) => true
            };

            MidiDevices = new ObservableCollection<string>();

            Keys = new ObservableCollection<PianoKeyModel>();
            Keys.CollectionChanged += Keys_CollectionChanged;

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


        }

        private void Keys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

        public string SelectedDevice
        {
            get => _selectedDevice;
            set => SetProperty(ref _selectedDevice, value);
        }
        public string KeyboardOctave
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

        public float Decay
        {
            get => _decay;
            set => SetProperty(ref _decay, value);
        }
        public float Release
        {
            get => _release;
            set => SetProperty(ref _release, value);
        }

        public float Sustain
        {
            get => _sustain;
            set => SetProperty(ref _sustain, value);
        }
        public float Attack
        {
            get => _attack;
            set => SetProperty(ref _attack, value);
        }

        public double Detune
        {
            get => _detune;
            set => SetProperty(ref _detune, value);
        }

        public string[] Shapes => _shapes;
        public string[] KeyboardOctaves => _keyboardOctaves;

        public string SelectedShape
        {
            get => _selectedModShape;
            set => SetProperty(ref _selectedModShape, value);
        }

        public string Osc1Waveform
        {
            get => _osc1Waveform;
            set => SetProperty(ref _osc1Waveform, value);
        }


        public string [] Osc1Octaves
        {
            get => _osc1Ocataves;
        }


        public string Osc1Octave
        {
            get => _osc1Octave;
            set => SetProperty(ref _osc1Octave, value);
        }
        public string Osc2Waveform
        {
            get => _osc2Waveform;
            set => SetProperty(ref _osc2Waveform, value);
        }


        public string[] Osc2Octaves
        {
            get => _osc2Ocataves;
        }


        public string Osc2Octave
        {
            get => _osc2Octave;
            set => SetProperty(ref _osc2Octave, value);
        }

        private void UpdateTotalKeysWidth()
        {
            int whiteKeysCount = Keys.Count(key => !key.IsBlackKey);
            TotalKeysWidth = whiteKeysCount * 40; // 40 is the width of the white keys
        }


        private double CalculateLeftPositionBasedOnNoteAndOctave(string keyNoteAndOctave)
        {
            int octave = int.Parse(keyNoteAndOctave[^1].ToString());
            string note = keyNoteAndOctave.Substring(0, keyNoteAndOctave.Length - 1);

            double baseOffset = 40 * octave * 7;

            switch (note)
            {
                case "C": return 0 + baseOffset;
                case "C#": return 25 + baseOffset;
                case "D": return 40 + baseOffset;
                case "D#": return 65 + baseOffset;
                case "E": return 80 + baseOffset;
                case "F": return 120 + baseOffset;
                case "F#": return 145 + baseOffset;
                case "G": return 160 + baseOffset;
                case "G#": return 185 + baseOffset;
                case "A": return 200 + baseOffset;
                case "A#": return 225 + baseOffset;
                case "B": return 240 + baseOffset;
            }

            return 0;
        }
    }
}

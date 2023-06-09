﻿using System.ComponentModel;

namespace SoundDesigner.Models;

public class PianoKeyModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string NoteAndOctave => $"{Note}{Octave}";
    private string _note;
    public string Note
    {
        get => _note;
        set
        {
            _note = value;
            OnPropertyChanged("Note");
        }
    }

    private bool _isBlackKey;
    public bool IsBlackKey
    {
        get => _isBlackKey;
        set
        {
            _isBlackKey = value;
            OnPropertyChanged("IsBlackKey");
        }
    }

    private int _octave;

    public int Octave
    {
        get => _octave;
        set
        {
            _octave = value;
            OnPropertyChanged("Octave");
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
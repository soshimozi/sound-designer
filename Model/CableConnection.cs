using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SoundDesigner.Control;

namespace SoundDesigner.Model;

public class CableConnection : INotifyPropertyChanged
{
    private AudioJack? _from;
    private AudioJack? _to;

    public AudioJack? From
    {
        get => _from;
        set => SetField(ref _from, value);
    }

    public AudioJack? To
    {
        get => _to;
        set => SetField(ref _to, value);
    }
        
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
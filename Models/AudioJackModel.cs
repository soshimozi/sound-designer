using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoundDesigner.Models;

public class AudioJackModel : INotifyPropertyChanged
{

    private int _x;
    private int _y;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int X
    {
        get => _x;
        set
        {
            var oldX = _x;
            _x = value;

            if (oldX != value)
            {
                OnPropertyChanged();
            }

        }
    }
    public int Y
    {
        get => _y;
        set
        {
            var oldY = _y;
            _y = value;

            if (oldY != value)
            {
                OnPropertyChanged();
            }
        }
    }

    protected void SetProperty<T>(ref T var, T value)
    {
        var = value;
        OnPropertyChanged();
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
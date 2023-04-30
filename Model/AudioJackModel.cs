using SoundDesigner.Lib;

namespace SoundDesigner.Model;

public class AudioJackModel : NotificationObject
{

    private int _x;
    private int _y;
    private string? _jackName;

    public string? JackName
    {
        get => _jackName;
        set => SetProperty(ref _jackName, value);
    }
    public int X
    {
        get => _x;
        set => SetProperty(ref _x, value);
    }
    public int Y
    {
        get => _y;
        set => SetProperty(ref _y, value);
    }

}
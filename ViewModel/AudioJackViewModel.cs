using SoundDesigner.Lib;

namespace SoundDesigner.ViewModel;

public class AudioJackViewModel : ViewModelBase
{
    private int _x;
    private int _y;

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
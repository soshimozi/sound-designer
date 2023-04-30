using SoundDesigner.Lib;

namespace SoundDesigner.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private string _title;

    public string Title
    {
        get => _title; set => SetProperty(ref _title, value);
    }

    public MainWindowViewModel()
    {
        Title = "Sound Designer Pro";
    }
}
using System.Windows.Input;
using SoundDesigner.Event;
using SoundDesigner.Lib;
using SoundDesigner.Lib.CommandRouting;

namespace SoundDesigner.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private string? _title;

    private readonly IEventAggregator  _eventAggregator;
    public string? Title
    {
        get => _title; set => SetProperty(ref _title, value);
    }

    public MainWindowViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        Title = "Sound Designer Pro";

        WindowClosing = new DelegateCommand
        {
            CommandAction = (o) => { _eventAggregator.Publish(new WindowClosingEvent()); },
            CanExecuteFunc = (o) => true
        };
    }

    public ICommand WindowClosing { get; }
}
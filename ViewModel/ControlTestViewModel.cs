using SoundDesigner.Lib;
using System.Collections.ObjectModel;
using SoundDesigner.Models;

namespace SoundDesigner.ViewModel;

public class ControlTestViewModel : ViewModelBase
{
    public ObservableCollection<CableConnection> Connections { get; set; } = new ObservableCollection<CableConnection>();

    public ControlTestViewModel() : base()
    {
        var model = new AudioJackModel
        {
            X = 100,
            Y = 100
        };

        var model2 = new AudioJackModel
        {
            X = 50,
            Y = 50
        };

        Jacks = new ObservableCollection<AudioJackModel> { model, model2 };

        Jacks.Add(new AudioJackModel { X = 120, Y = 60 });

    }

    public ObservableCollection<AudioJackModel> Jacks
    {
        get;
    }

}
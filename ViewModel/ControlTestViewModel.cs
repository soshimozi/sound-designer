using SoundDesigner.Lib;
using System.Collections.ObjectModel;
using SoundDesigner.Models;

namespace SoundDesigner.ViewModel;

public class ControlTestViewModel : ViewModelBase
{
    public ObservableCollection<CableConnection> Connections { get; set; } = new ObservableCollection<CableConnection>();
}
using CommunityToolkit.Mvvm.DependencyInjection;
using SoundDesigner.ViewModel;

namespace SoundDesigner.Helper;

public class ViewModelLocator
{
    public MainWindowViewModel? MainWindowViewModel => Ioc.Default.GetService<MainWindowViewModel>();
    public SoundGenerationViewModel? SoundGenerationViewModel => Ioc.Default.GetService<SoundGenerationViewModel>();
    public NavigationViewModel? NavigationViewModel => Ioc.Default.GetService<NavigationViewModel>();
}
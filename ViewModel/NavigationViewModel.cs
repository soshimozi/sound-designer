using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using SoundDesigner.Lib;
using SoundDesigner.Lib.CommandRouting;
using SoundDesigner.View;

namespace SoundDesigner.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    private object? _currentView;
    public object? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    //public ICommand HomeCommand { get; set; }
    public ICommand StencilCommand { get; set; }
    public ICommand SoundGenerationCommand { get; set; }
    public ICommand SettingsCommand { get; set; }
    public ICommand ControlTestCommand { get; set; }

    //private void HomePage(object obj) => CurrentView = new HomePageViewModel();
    private void SoundGenerationPage(object obj) => CurrentView = Ioc.Default.GetService<SoundGenerationViewModel>();
    private void SettingsPage(object obj) => CurrentView = Ioc.Default.GetService<SettingsViewModel>();
    private void ControlTestPage(object obj) => CurrentView = Ioc.Default.GetService<ControlTestViewModel>();
    private void StencilPage(object obj) => CurrentView = Ioc.Default.GetService<AudioGraphViewModel>();

    public NavigationViewModel()
    {
        //HomeCommand = new RoutableCommand(HomePage, (o) => true);
        SoundGenerationCommand = new RoutableCommand(SoundGenerationPage, (o) => true);
        SettingsCommand = new RoutableCommand(SettingsPage, (o) => true);
        ControlTestCommand = new RoutableCommand(ControlTestPage, (o) => true);
        StencilCommand = new RoutableCommand(StencilPage, (o) => true);

        // Startup Page
        CurrentView = Ioc.Default.GetService<SoundGenerationViewModel>();
    }
}
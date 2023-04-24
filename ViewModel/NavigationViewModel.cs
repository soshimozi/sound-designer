using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using SoundDesigner.Lib;
using SoundDesigner.Lib.CommandRouting;

namespace SoundDesigner.ViewModel
{
    internal class NavigationViewModel : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand HomeCommand { get; set; }
        public ICommand SoundGenerationCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand ControlTestCommand { get; set; }

        private void HomePage(object obj) => CurrentView = new HomePageViewModel();
        private void SoundGenerationPage(object obj) => CurrentView = new SoundGenerationViewModel();
        private void SettingsPage(object obj) => CurrentView = new SettingsViewModel();
        private void ControlTestPage(object obj) => CurrentView = new MainWindowControlTest();

        public NavigationViewModel()
        {
            HomeCommand = new RoutableCommand(HomePage, (o) => true);
            SoundGenerationCommand = new RoutableCommand(SoundGenerationPage, (o) => true);
            SettingsCommand = new RoutableCommand(SettingsPage, (o) => true);
            ControlTestCommand = new RoutableCommand(ControlTestPage, (o) => true);

            // Startup Page
            CurrentView = new HomePageViewModel();
        }
    }
}

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
        //public ICommand CustomersCommand { get; set; }
        //public ICommand ProductsCommand { get; set; }
        //public ICommand OrdersCommand { get; set; }
        //public ICommand TransactionsCommand { get; set; }
        //public ICommand ShipmentsCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void HomePage(object obj) => CurrentView = new HomePageViewModel();
        private void SoundGenerationPage(object obj) => CurrentView = new SoundGenerationViewModel();
        private void SettingsPage(object obj) => CurrentView = new SettingsViewModel();
        
        //private void Customer(object obj) => CurrentView = new CustomerVM();
        //private void Product(object obj) => CurrentView = new ProductVM();
        //private void Order(object obj) => CurrentView = new OrderVM();
        //private void Transaction(object obj) => CurrentView = new TransactionVM();
        //private void Shipment(object obj) => CurrentView = new ShipmentVM();
        //private void Setting(object obj) => CurrentView = new SettingVM();

        public NavigationViewModel()
        {
            HomeCommand = new RoutableCommand(HomePage, (o) => true);
            SoundGenerationCommand = new RoutableCommand(SoundGenerationPage, (o) => true);
            SettingsCommand = new RoutableCommand(SettingsPage, (o) => true);

            //CustomersCommand = new RelayCommand(Customer);
            //ProductsCommand = new RelayCommand(Product);
            //OrdersCommand = new RelayCommand(Order);
            //TransactionsCommand = new RelayCommand(Transaction);
            //ShipmentsCommand = new RelayCommand(Shipment);
            //SettingsCommand = new RelayCommand(Setting);

            // Startup Page
            CurrentView = new HomePageViewModel();
        }
    }
}

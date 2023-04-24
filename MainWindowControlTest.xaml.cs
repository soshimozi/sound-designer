using System.Collections.ObjectModel;
using System.Windows;
using SoundDesigner.Models;
using SoundDesigner.ViewModel;

namespace SoundDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowControlTest : Window
    {
        public MainWindowControlTest()
        {
            InitializeComponent();

            DataContext = this;


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

        }

        public ObservableCollection<AudioJackModel> Jacks
        {
            get;
        }

    }
}

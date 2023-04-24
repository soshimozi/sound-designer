using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SoundDesigner.Models;
using SoundDesigner.ViewModel;

namespace SoundDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowControlTest : UserControl
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

            /*
             *
        AddJack("myjack", 50, 50, 1);
        AddJack("myjack2", 100, 100, 0);
        AddJack("myjack3", 130, 100, 0);
        AddJack("myjack4", 160, 100, 0);
        AddJack("myjack5", 200, 50, 1);

        DrawText("PWM", 50, 35, Colors.White, HorizontalAlignment.Center, VerticalAlignment.Bottom);
        DrawDownArrow(50, 35, 5, Colors.White, Colors.White);
             */

        }

        public ObservableCollection<AudioJackModel> Jacks
        {
            get;
        }

    }
}

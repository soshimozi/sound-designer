using System.Windows;
using System.Windows.Media;
using JackTest.Controls;

namespace SoundDesigner.Controls;

public class MainPanel : ControlPanel
{
    static MainPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MainPanel), new FrameworkPropertyMetadata(typeof(MainPanel)));
            
    }


    public MainPanel()
    {
    }
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

            

        if (FindResource("BackgroundBrush") is ImageBrush brush)
        {
            DrawRoundedRectangle(10, 10, 760, 450, 10, brush, new SolidColorBrush(Colors.White));
        }

        AddJack("myjack", 50, 50, 1);
        AddJack("myjack2", 100, 100, 0);
        AddJack("myjack3", 130, 100, 0);
        AddJack("myjack4", 160, 100, 0);
        AddJack("myjack5", 200, 50, 1);

        DrawText("PWM", 50, 35, Colors.White, HorizontalAlignment.Center, VerticalAlignment.Bottom);
        DrawDownArrow(50, 35, 5, Colors.White, Colors.White);

    }




}
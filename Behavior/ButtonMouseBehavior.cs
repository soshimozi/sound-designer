using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace SoundDesigner.Behavior;

public static class ButtonMouseBehavior
{
    public static readonly DependencyProperty PressedCommandProperty =
        DependencyProperty.RegisterAttached("PressedCommand", typeof(ICommand), typeof(ButtonMouseBehavior), new FrameworkPropertyMetadata(PressedCommandChanged));

    public static readonly DependencyProperty ReleasedCommandProperty =
        DependencyProperty.RegisterAttached("ReleasedCommand", typeof(ICommand), typeof(ButtonMouseBehavior), new FrameworkPropertyMetadata(ReleasedCommandChanged));

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ButtonMouseBehavior), new FrameworkPropertyMetadata(null));

    public static void SetPressedCommand(DependencyObject target, ICommand value) => target.SetValue(PressedCommandProperty, value);

    public static ICommand GetPressedCommand(DependencyObject target) => (ICommand)target.GetValue(PressedCommandProperty);

    public static void SetReleasedCommand(DependencyObject target, ICommand value) => target.SetValue(ReleasedCommandProperty, value);

    public static ICommand GetReleasedCommand(DependencyObject target) => (ICommand)target.GetValue(ReleasedCommandProperty);

    public static void SetCommandParameter(DependencyObject target, object value) => target.SetValue(CommandParameterProperty, value);

    public static object GetCommandParameter(DependencyObject target) => target.GetValue(CommandParameterProperty);

    private static void PressedCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
        if (target is Button button)
        {
            button.PreviewMouseDown -= ButtonOnPreviewMouseDown;
            button.PreviewMouseDown += ButtonOnPreviewMouseDown;
        }
    }

    private static void ButtonOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Button button)
        {
            ICommand command = GetPressedCommand(button);
            object commandParameter = GetCommandParameter(button);
            command?.Execute(commandParameter);
        }
    }

    private static void ReleasedCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
        if (target is Button button)
        {
            button.PreviewMouseUp -= ButtonOnPreviewMouseUp;
            button.PreviewMouseUp += ButtonOnPreviewMouseUp;
        }
    }

    private static void ButtonOnPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is Button button)
        {
            ICommand command = GetReleasedCommand(button);
            object commandParameter = GetCommandParameter(button);
            command?.Execute(commandParameter);
        }
    }

    private static void Button_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Button button)
        {
            ICommand command = GetPressedCommand(button);
            command?.Execute(null);
        }
    }

    private static void Button_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is Button button)
        {
            ICommand command = GetReleasedCommand(button);
            command?.Execute(null);
        }
    }
}
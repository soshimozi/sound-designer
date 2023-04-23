namespace SoundDesigner.Lib.CommandRouting.EventArgs;

/// <summary>
/// CommandEventArgs - simply holds the command parameter.
/// </summary>
public class CommandEventArgs : System.EventArgs
{
    /// <summary>
    /// Gets or sets the parameter.
    /// </summary>
    /// <value>The parameter.</value>
    public object? Parameter { get; set; }
}

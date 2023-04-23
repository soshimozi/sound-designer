namespace SoundDesigner.Lib.CommandRouting.EventArgs;


/// <summary>
/// CancelCommandEventArgs - just like above but allows the event to 
/// be cancelled.
/// </summary>
public class CancelCommandEventArgs : CommandEventArgs
{
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CancelCommandEventArgs"/> command should be cancelled.
    /// </summary>
    /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
    public bool Cancel { get; set; }
}

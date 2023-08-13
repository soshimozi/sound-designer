namespace SoundDesigner.Event;

public class WindowClosingEvent : IEventBase
{
    public string? Message { get; set; }
}
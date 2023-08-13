using System;

namespace SoundDesigner.Event;

public interface IEventAggregator
{
    void Publish<TEvent>(TEvent eventToPublish) where TEvent : IEventBase;
    void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase;
    void Unsubscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase;
}
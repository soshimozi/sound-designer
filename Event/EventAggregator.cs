using System.Collections.Generic;
using System;
using System.Linq;

namespace SoundDesigner.Event;

public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, List<object>> _subscriptions;

    public EventAggregator()
    {
        _subscriptions = new Dictionary<Type, List<object>>();
    }

    public void Publish<TEvent>(TEvent eventToPublish) where TEvent : IEventBase
    {
        var eventType = typeof(TEvent);
        if (_subscriptions.ContainsKey(eventType))
        {
            var subscribers = _subscriptions[eventType];
            foreach (var subscriber in subscribers.ToList())
            {
                var callback = subscriber as Action<TEvent>;
                callback?.Invoke(eventToPublish);
            }
        }
    }

    public void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase
    {
        var eventType = typeof(TEvent);
        if (!_subscriptions.ContainsKey(eventType))
        {
            _subscriptions[eventType] = new List<object>();
        }
        _subscriptions[eventType].Add(action);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase
    {
        var eventType = typeof(TEvent);
        if (_subscriptions.ContainsKey(eventType))
        {
            _subscriptions[eventType].Remove(action);
        }
    }
}

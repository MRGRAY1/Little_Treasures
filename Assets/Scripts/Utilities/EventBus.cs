using System.Collections.Generic;

public static class EventBus
{
    private static readonly List<GameEventListener> listeners = new();

    public static void Register(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public static void Unregister(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public static void Publish(EventIndex evt)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEnumEventRaised(evt);
    }

    public static void Publish<T>(EventIndex evt, T value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEnumEventRaised(evt, value);
    }
}
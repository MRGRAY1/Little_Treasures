using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<EventIndex, Delegate> events = new Dictionary<EventIndex, Delegate>();

    public static void Subscribe<T>(EventIndex EventIndex, Action<T> listener)
    {
        if (!events.ContainsKey(EventIndex))
        {
            events[EventIndex] = null;
        }
        events[EventIndex] = (Action<T>)events[EventIndex] + listener;
    }

    public static void Unsubscribe<T>(EventIndex EventIndex, Action<T> listener)
    {
        if (events.ContainsKey(EventIndex))
        {
            events[EventIndex] = (Action<T>)events[EventIndex] - listener;

            if (events[EventIndex] == null)
            {
                events.Remove(EventIndex);
            }
        }
    }

    public static void Publish<T>(EventIndex EventIndex, T data)
    {
        if (events.ContainsKey(EventIndex))
        {
            ((Action<T>)events[EventIndex])?.Invoke(data);
        }
    }
}

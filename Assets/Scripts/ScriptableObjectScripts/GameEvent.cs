using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject representing a game event. 
/// Can raise events with optional payloads of any type.
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    // List of listeners that respond to this event
    private List<GameEventListener> listeners = new List<GameEventListener>();

    /// <summary>
    /// Raise the event without payload
    /// </summary>
    public void Raise()
    {
        Logger.Log($"GameEvent: '{name}' raised (no payload)");
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(this);
        }
    }

    /// <summary>
    /// Raise the event with any payload type
    /// </summary>
    public void Raise<T>(T value)
    {
        Logger.Log($"GameEvent: '{name}' raised with payload: {value}");
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(this, value);
        }
    }

    /// <summary>
    /// Register a listener to this event
    /// </summary>
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    /// <summary>
    /// Unregister a listener from this event
    /// </summary>
    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
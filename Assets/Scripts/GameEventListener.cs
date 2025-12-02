using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Generic container to link a GameEvent to a UnityEvent of type T
/// </summary>
[Serializable]
public class EventResponsePair<T>
{
    public GameEvent Event;
    public UnityEvent<T> Response;
}

/// <summary>
/// Listener component that handles any type of GameEvent with optional payloads.
/// </summary>
public class GameEventListener : MonoBehaviour
{
    // Lists of events by type
    public List<EventResponsePair<int>> IntEvents = new();
    public List<EventResponsePair<float>> FloatEvents = new();
    public List<EventResponsePair<bool>> BoolEvents = new();
    public List<EventResponsePair<string>> StringEvents = new();
    public List<EventResponsePair<AudioClip>> AudioEvents = new();
    public List<EventResponsePair<Vector3>> Vector3Events = new();
    public List<EventResponsePair<object>> NoPayloadEvents = new(); // fallback for no payload

    /// <summary>
    /// Called when object is enabled - register all events
    /// </summary>
    private void OnEnable()
    {
        RegisterAllEvents();
    }

    /// <summary>
    /// Called when object is disabled - unregister all events
    /// </summary>
    private void OnDisable()
    {
        UnregisterAllEvents();
    }

    /// <summary>
    /// Generic function to register a list of events
    /// </summary>
    private void RegisterAllEvents()
    {
        RegisterEvents(NoPayloadEvents);
        RegisterEvents(IntEvents);
        RegisterEvents(FloatEvents);
        RegisterEvents(BoolEvents);
        RegisterEvents(StringEvents);
        RegisterEvents(AudioEvents);
        RegisterEvents(Vector3Events);
    }

    /// <summary>
    /// Generic function to unregister a list of events
    /// </summary>
    private void UnregisterAllEvents()
    {
        UnregisterEvents(NoPayloadEvents);
        UnregisterEvents(IntEvents);
        UnregisterEvents(FloatEvents);
        UnregisterEvents(BoolEvents);
        UnregisterEvents(StringEvents);
        UnregisterEvents(AudioEvents);
        UnregisterEvents(Vector3Events);
    }

    /// <summary>
    /// Register each event in a list
    /// </summary>
    private void RegisterEvents<T>(List<EventResponsePair<T>> events)
    {
        foreach (var pair in events)
        {
            if (pair?.Event != null)
                pair.Event.RegisterListener(this);
        }
    }

    /// <summary>
    /// Unregister each event in a list
    /// </summary>
    private void UnregisterEvents<T>(List<EventResponsePair<T>> events)
    {
        foreach (var pair in events)
        {
            if (pair?.Event != null)
                pair.Event.UnregisterListener(this);
        }
    }

    /// <summary>
    /// Called by GameEvent when raised with no payload
    /// </summary>
    public void OnEventRaised(GameEvent passedEvent)
    {
        foreach (var pair in NoPayloadEvents)
        {
            if (pair.Event == passedEvent)
            {
                pair.Response?.Invoke(null); // invoke as object for no-payload
                break;
            }
        }
    }

    /// <summary>
    /// Generic handler for any payload type
    /// </summary>
    public void OnEventRaised<T>(GameEvent passedEvent, T value)
    {
        List<EventResponsePair<T>> eventList = value switch
        {
            int _ => IntEvents as List<EventResponsePair<T>>,
            float _ => FloatEvents as List<EventResponsePair<T>>,
            bool _ => BoolEvents as List<EventResponsePair<T>>,
            string _ => StringEvents as List<EventResponsePair<T>>,
            AudioClip _ => AudioEvents as List<EventResponsePair<T>>,
            Vector3 _ => Vector3Events as List<EventResponsePair<T>>,
            _ => null
        };

        if (eventList == null) return;

        foreach (var pair in eventList)
        {
            if (pair.Event == passedEvent)
            {
                pair.Response?.Invoke(value);
                break;
            }
        }
    }
}

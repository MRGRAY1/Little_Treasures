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
    public GameEvent ScriptableEvent;
    public EventIndex EnumEvent;
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
    public List<EventResponsePair<object>> NoPayloadEvents = new();

    /// <summary>
    /// Called when object is enabled - register all events
    /// </summary>
    private void OnEnable()
    {
        RegisterSOEvents();
        EventBus.Register(this);
    }

    private void OnDisable()
    {
        UnregisterSOEvents();
        EventBus.Unregister(this);
    }

    private void RegisterSOEvents()
    {
        RegisterList(NoPayloadEvents);
        RegisterList(IntEvents);
        RegisterList(FloatEvents);
        RegisterList(BoolEvents);
        RegisterList(StringEvents);
        RegisterList(AudioEvents);
        RegisterList(Vector3Events);
    }

    private void UnregisterSOEvents()
    {
        UnregisterList(NoPayloadEvents);
        UnregisterList(IntEvents);
        UnregisterList(FloatEvents);
        UnregisterList(BoolEvents);
        UnregisterList(StringEvents);
        UnregisterList(AudioEvents);
        UnregisterList(Vector3Events);
    }

    private void RegisterList<T>(List<EventResponsePair<T>> list)
    {
        foreach (var pair in list)
            pair?.ScriptableEvent?.RegisterListener(this);
    }

    private void UnregisterList<T>(List<EventResponsePair<T>> list)
    {
        foreach (var pair in list)
            pair?.ScriptableEvent?.UnregisterListener(this);
    }


    /* ===== ScriptableObject callbacks ===== */

    public void OnEventRaised(GameEvent evt)
    {
        Invoke(NoPayloadEvents, evt, null);
    }

    public void OnEventRaised<T>(GameEvent evt, T value)
    {
        Invoke(GetList<T>(), evt, value);
    }

    /* ===== Enum callbacks from EventBus ===== */

    public void OnEnumEventRaised<T>(EventIndex evt, T value)
    {
        Invoke(GetList<T>(), evt, value);
    }

    public void OnEnumEventRaised(EventIndex evt)
    {
        Invoke(NoPayloadEvents, evt, null);
    }

    /* ===== Shared invoke logic ===== */

    private void Invoke<T>(List<EventResponsePair<T>> list, GameEvent evt, T value)
    {
        foreach (var pair in list)
        {
            if (pair.ScriptableEvent == evt)
            {
                pair.Response?.Invoke(value);
                return;
            }
        }
    }

    private void Invoke<T>(List<EventResponsePair<T>> list, EventIndex evt, T value)
    {
        foreach (var pair in list)
        {
            if (pair.EnumEvent == evt)
            {
                pair.Response?.Invoke(value);
                return;
            }
        }
    }

    private List<EventResponsePair<T>> GetList<T>()
    {
        if (typeof(T) == typeof(int)) return IntEvents as List<EventResponsePair<T>>;
        if (typeof(T) == typeof(float)) return FloatEvents as List<EventResponsePair<T>>;
        if (typeof(T) == typeof(bool)) return BoolEvents as List<EventResponsePair<T>>;
        if (typeof(T) == typeof(string)) return StringEvents as List<EventResponsePair<T>>;
        if (typeof(T) == typeof(AudioClip)) return AudioEvents as List<EventResponsePair<T>>;
        if (typeof(T) == typeof(Vector3)) return Vector3Events as List<EventResponsePair<T>>;
        return null;
    }
}
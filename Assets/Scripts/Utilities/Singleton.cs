using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()

    {
        if (Instance != null && Instance != this)
        {
            Logger.LogWarning($"[Singleton] Duplicate instance of {typeof(T).Name} detected. " +
                             $"The object '{gameObject.name}' will be destroyed.");
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
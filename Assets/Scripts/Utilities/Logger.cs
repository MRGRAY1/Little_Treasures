using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public static void Log(string message, Object context = null)
    {
        Debug.Log(message, context);
    }

    public static void LogWarning(string message, Object context = null)
    {
        Debug.LogWarning(message, context);
    }

    public static void LogError(string message, Object context = null)
    {
        Debug.LogError(message, context);
    }
}

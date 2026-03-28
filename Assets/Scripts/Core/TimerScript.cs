using System;
using System.Collections;
using UnityEngine;


public class TimerScript : MonoBehaviour
{
    public float Timer;

    private void OnEnable()
    {
        GameEvents.StartTimer += DoWait;
    }

    private void OnDisable()
    {
        GameEvents.StartTimer -= DoWait;
    }

    private void DoWait(object sender, TimerType type, float timeToWait)
    {
        Logger.LogWarning("Starting Timer");
        StartCoroutine(WaitCoroutine(type, timeToWait));
    }

    private IEnumerator WaitCoroutine(TimerType type, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        Logger.LogWarning($"{timeToWait} seconds passed!");
        GameEvents.TimerElapsed?.Invoke(this, type);
    }
}

public enum TimerType
{
    GenerationsComplete,
    GenerationsStart,
}
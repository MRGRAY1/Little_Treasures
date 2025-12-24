using System.Collections;
using UnityEngine;


public class TimerScript : MonoBehaviour
{
    public EventIndex GameEvent;
    public float Timer;

    public void DoWait()
    {
        Logger.LogWarning("Starting Timer");
        Invoke(nameof(WaitForTimer), Timer); // runs once after 10 seconds
    }

    void WaitForTimer()
    {
        Logger.LogWarning($"{Timer} seconds passed!");
        EventBus.Publish(GameEvent);
    }
}

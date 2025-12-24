using UnityEngine;

public class InitializeItem : MonoBehaviour
{
    public EventIndex CompleteEvent;
    public BoolVariable CompleteCheck;


    public virtual void Initialize()
    {
        Logger.Log($"Initilizing: {gameObject.name}");
    }

    public virtual void CompleteInit()
    {
        Logger.Log($"Initilizing Complete: {gameObject.name}");
        CompleteCheck.setValue(true);
        EventBus.Publish(CompleteEvent);


    }
}
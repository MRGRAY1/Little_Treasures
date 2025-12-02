using System;
using UnityEngine;

public class MiscManager : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    private GameEvent ReloadSceneEvent;

    /// <summary>
    /// On Enable
    /// </summary>
    private void OnEnable()
    {
        this.inputManager = FindFirstObjectByType<InputManager>();
        this.inputManager.OnNreleased += ReloadScene;
    }

    private void ReloadScene()
    {
        Logger.Log("Reload Scene Pressed");
        ReloadSceneEvent?.Raise();
    }

    /// <summary>
    /// On Disable
    /// </summary>
    private void OnDisable()
    {
        this.inputManager = FindFirstObjectByType<InputManager>();
        if (this.inputManager == null) return;
        this.inputManager.OnNreleased -= ReloadScene;


    }
}

using System;
using UnityEngine;

/// <summary>
/// DO NOT CHANGE ORDER, ONLY ADD TO THE END!
/// </summary>
public static class GameEvents
{
    public static Action<object> DungeonGenerationComplete;
    public static Action<object, bool> GenerationsComplete;
    public static Action<object> NextSceneConfigurations;
    public static Action<object> PlayerSpawnComplete;
    public static Action<object> ReloadScene;
    public static Action<object> RoomGenerationStart;
    public static Action<object> SceneReady;
    public static Action<object> SceneTransitionEnd;
    public static Action<object> SceneTransitionStart;
    public static Action<object> StartFadeIn;
    public static Action<object> StartFadeOut;
    public static Action<object, GameScenes> StartSceneChange;
    public static Action<object> LoadScene;
    public static Action<object> ExitGame;
    public static Action<object, int> PickUpCoin;
    public static Action<object> PickUpItem;
    public static Action<object> UseCoin;
    public static Action<object> ItemsSpawnStart;
    public static Action<object> ItemsSpawnComplete;
    public static Action<object, TimerType> TimerElapsed;
    public static Action<object, TimerType, float> StartTimer;

    public static Action OnNreleased;
}
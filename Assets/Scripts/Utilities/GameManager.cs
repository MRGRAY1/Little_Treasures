using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.GenerationsComplete += GenerationsComplete;
        GameEvents.OnNreleased += OnNreleased;
    }

    private void OnNreleased()
    {
        GameEvents.ReloadScene?.Invoke(this);
    }

    private void OnDisable()
    {
        GameEvents.GenerationsComplete -= GenerationsComplete;
    }

    private void GenerationsComplete(object obj, bool state)
    {
        GameEvents.StartTimer?.Invoke(this, TimerType.GenerationsComplete, 10f);
    }
}

public enum GameScenes
{
    MainMenu,
    TestScene,
}

public enum GemTypes
{
    RedGem,
    BlueGem,
    GreenGem,
}

public enum CoinTypes
{
    CopperCoin,
    SilverCoin,
    GoldCoin,
}
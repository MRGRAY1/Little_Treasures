using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private static EventIndex Event;
    public static GameState CurrentGameState;

    [Header("Base Variables")]
    [Tooltip("World Gravity")]
    public float World_Gravity = -9.81f;
    [SerializeField]
    private float timer = 0.0f;
    [SerializeField]
    private int counter = 0;

    static void OnEnable()
    {
        EventBus.Subscribe<GameState>(Event, GameStateChange);
    }

    static void OnDisable()
    {
        EventBus.Unsubscribe<GameState>(Event, GameStateChange);
    }

    static void GameStateChange(GameState state)
    {
        CurrentGameState = state;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadGameInstance()
    {

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5)
        {
            Logger.Log($"Countdown: {timer}");
            counter++;
            timer = 0.0f;
        }
    }

}

public enum GameState
{
    Menu,
    Running,
    Paused,
    GameOver,
    Other,
}
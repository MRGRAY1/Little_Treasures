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

    public static IEnumerator WaitForSeconds(float duration)
    {
        Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
        yield return new WaitForSeconds(duration);
        Debug.Log($"Ended at {Time.time}");
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
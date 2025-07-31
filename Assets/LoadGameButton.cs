using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameButton : MonoBehaviour
{

    [SerializeField]
    private GameObject BlackLoadScreen;
    [SerializeField]
    private float fadeDuration = 0.5f;
    [SerializeField]
    private float LoadCompleteionDuration = 0.9f;
    [SerializeField]
    private float delayDuration = 0.5f;
    [SerializeField]
    private string sceneToLoad = "TestScene";

    [SerializeField]
    private Color blackColor = Color.black;
    [SerializeField]
    private Image blackImage;



    private void Start()
    {
        BlackLoadScreen.SetActive(false);
    }

    public void LoadGame()
    {
        Logger.Log("Test");
        StartCoroutine(LoadSequence());
    }

    IEnumerator LoadSequence()
    {
        BlackLoadScreen?.SetActive(true);
        yield return StartCoroutine(FadeBlackIn());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < LoadCompleteionDuration)
        {
            yield return null;
        }

        yield return new WaitForSeconds(delayDuration);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        yield return WaitForDungeon();
        yield return WaitForPlayerLoad();

        yield return StartCoroutine(FadeBlackOut());
        BlackLoadScreen.SetActive(false);

    }

    IEnumerator WaitForDungeon()
    {
        if (DungeonCreator.Instance != null)
        {
            DungeonCreator.Instance.StartGenerateDungeon();
            yield return new WaitUntil(() => DungeonCreator.Instance.IsGenerationComplete);
        }
        else
        {
            Debug.LogWarning("DungeonCreator.Instance not found.");
        }
    }
    private IEnumerator WaitForPlayerLoad()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SpawnPlayer();
            yield return new WaitUntil(() => PlayerManager.Instance.PlayerReady);
        }
        else
        {
            Debug.LogWarning("PlayerManager.Instance not found.");
        }
    }
    IEnumerator FadeBlackIn()
    {
        float timer = 0f;
        Color color = blackImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            blackImage.color = color;
            yield return null;
        }

    }
    IEnumerator FadeBlackOut()
    {
        float timer = 0f;
        Color color = blackImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            blackImage.color = color;
            yield return null;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private GameObject blackScreen;
    [SerializeField] private Image blackImage;
    [SerializeField] private float fadeDuration = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        Logger.Log("LoadSceneWithTransition");
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        Logger.Log("Transition");
        blackScreen.SetActive(true);
        yield return Fade(1); // Fade to black

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) yield return null;

        // Wait for scene to signal ready
        yield return new WaitUntil(() => SceneLoadController.SceneReady);

        Logger.Log("Fade");
        yield return Fade(0); // Fade out
        blackScreen.SetActive(false);
    }

    public IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = blackImage.color.a;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            Color c = blackImage.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            blackImage.color = c;
            yield return null;
        }
    }
}

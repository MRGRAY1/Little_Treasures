using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManagerEvents : MonoBehaviour
{
    public static SceneTransitionManagerEvents Instance;

    [SerializeField] private GameObject blackScreen;
    [SerializeField] private Image blackImage;
    [SerializeField] private float fadeDuration = 1f;

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
        Logger.Log($"Transition: {blackScreen.activeSelf}");
        blackScreen.SetActive(true);

        // Fade to black
        yield return StartCoroutine(Fade(1f));

        // Load new scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade back to transparent
        yield return StartCoroutine(Fade(0f));

        blackScreen.SetActive(false);
    }

    public IEnumerator Fade(float targetAlpha)
    {
        Logger.Log($"Fade: {targetAlpha}");
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

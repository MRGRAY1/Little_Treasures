using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneLoadController : MonoBehaviour
{
    public static bool SceneReady { get; private set; }

    [Header("Loading Steps (Executed in order)")]
    public List<UnityEvent> loadingSteps = new List<UnityEvent>();

    [Header("Delay between steps (optional)")]
    public float stepDelay = 0.2f;

    private void Awake()
    {
        SceneReady = false;
    }

    private IEnumerator Start()
    {
        // Fade in from black
        if (SceneTransitionManager.Instance != null)
        {
            yield return SceneTransitionManager.Instance.Fade(1); // Ensure screen stays black
        }

        // Execute all steps in order
        foreach (var step in loadingSteps)
        {
            step?.Invoke();
            yield return new WaitForSeconds(stepDelay);
        }

        // Example: Wait until DungeonCreator finishes (if present)
        if (DungeonCreator.Instance != null)
            yield return new WaitUntil(() => DungeonCreator.Instance.IsGenerationComplete);

        // Example: Wait until PlayerManager finishes (if present)
        if (PlayerManager.Instance != null)
            yield return new WaitUntil(() => PlayerManager.Instance.PlayerReady);

        // Mark scene ready, fade out
        SceneReady = true;

        if (SceneTransitionManager.Instance != null)
            yield return SceneTransitionManager.Instance.Fade(0);
    }
}

using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameButton : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject BlackLoadScreen;
    [SerializeField] private Image blackImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "TestScene";
    [SerializeField] private float loadCompletionThreshold = 0.9f;
    [SerializeField] private float delayDuration = 0.5f;

    public void LoadGame()
    {
        if (SceneTransitionManagerEvents.Instance != null)
        {
            SceneTransitionManagerEvents.Instance.LoadSceneWithTransition(sceneToLoad);
        }
        else
        {
            Debug.LogError("TransitionManager instance not found.");
        }
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private GameObject CanavasController;
    [SerializeField]
    private GameEvent sceneTransitionStart;

    public void SceneLoad(StringVariable scene)
    {
        Logger.Log($"Scene to change to: {scene}");
        SceneManager.LoadScene(scene.value);
    }

    public void LoadNextScene()
    {
        Logger.Log("Load Next Scene");
        SetCanvasState(true);
        sceneTransitionStart?.Raise();
    }

    public void SetCanvasState(bool state)
    {
        CanavasController.SetActive(state);
    }
}

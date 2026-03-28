using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private GameObject CanavasController;

    private void OnEnable()
    {
        GameEvents.StartSceneChange += SceneLoad;
        GameEvents.LoadScene += LoadNextScene;
        GameEvents.GenerationsComplete += SetCanvasState;
        GameEvents.ReloadScene += LoadNextScene;
    }

    private void OnDisable()
    {
        GameEvents.StartSceneChange -= SceneLoad;
        GameEvents.LoadScene -= LoadNextScene;
        GameEvents.GenerationsComplete -= SetCanvasState;
        GameEvents.ReloadScene -= LoadNextScene;
    }

    public void SceneLoad(object sender, GameScenes scene)
    {
        Logger.Log($"Scene to change to: {scene}");
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadNextScene(object sender)
    {
        Logger.Log("Load Next Scene");
        GameEvents.SceneTransitionStart?.Invoke(this);
        //SetCanvasState(true);
    }

    public void SetCanvasState(object sender, bool state)
    {
        CanavasController.SetActive(state);
    }
}
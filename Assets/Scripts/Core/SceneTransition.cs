using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private GameObject CanavasController;
    [SerializeField]
    private EventIndex sceneTransitionStart;

    public void SceneLoad(StringVariable scene)
    {
        Logger.Log($"Scene to change to: {scene}");
        SceneManager.LoadScene(scene.value);
    }

    public void LoadNextScene()
    {
        Logger.Log("Load Next Scene");
        //SetCanvasState(true);
        EventBus.Publish(sceneTransitionStart);
    }

    public void SetCanvasState(bool state)
    {
        CanavasController.SetActive(state);
    }
}
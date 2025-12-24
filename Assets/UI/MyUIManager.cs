using UnityEngine;
using UnityEngine.UIElements;

public class MyUIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;

    [SerializeField]
    private EventIndex LoadSceneEvent;

    [SerializeField]
    private EventIndex ExitGameEvent;

    void OnEnable()
    {
        // Get the UIDocument component attached to this GameObject
        uiDocument = GetComponent<UIDocument>();

        // Query the root visual element for a button named "my-button"
        var PlayGameBtn = uiDocument.rootVisualElement.Q<Button>("PlayGame_Btn");
        var ExitBtn = uiDocument.rootVisualElement.Q<Button>("Exit_Btn");

        // Register a callback for when the button is clicked
        if (PlayGameBtn != null)
        {
            PlayGameBtn.clicked += PlayGame;
        }
        if (ExitBtn != null)
        {
            ExitBtn.clicked += ExitGame;
        }
    }

    private void ExitGame()
    {
        Logger.Log("Exit Game");
        EventBus.Publish(ExitGameEvent);
    }

    private void PlayGame()
    {
        Logger.Log("Play Game");
        EventBus.Publish(LoadSceneEvent);
    }
}
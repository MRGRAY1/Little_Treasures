

using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistLoader : MonoBehaviour
{
    [SerializeField] private string firstScene = "MainMenu";

    private void Start()
    {
        // Load main menu after managers are initialized
        SceneManager.LoadScene(firstScene);
    }
}

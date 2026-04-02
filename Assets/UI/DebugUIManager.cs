using UnityEngine;
using UnityEngine.UIElements;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;

    [SerializeField]
    private Label CoinsLbl;
    
    void OnEnable()
    {
        // Get the UIDocument component attached to this GameObject
        uiDocument = GetComponent<UIDocument>();

        // Query the root visual element for a button named "my-button"
        CoinsLbl = uiDocument.rootVisualElement.Q<Label>("Coins_lbl");

    }

    public void UpdateUI()
    {
    }

}
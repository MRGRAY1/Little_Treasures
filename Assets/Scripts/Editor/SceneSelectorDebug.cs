using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSelectorDebug : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset treeAsset;
    private string sceneName = "";

    [MenuItem("Tools/Scene Selector")]
    public static void ShowMyEditor()
    {
        EditorWindow window = GetWindow<SceneSelectorDebug>();
        window.titleContent = new GUIContent("Scene Selector");

    }
    private void OnEnable()
    {
        CreateGUI();
    }

    private void CreateGUI()
    {
        treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/DebugWindows/SceneSelectorWindow.uxml");
        if (treeAsset == null)
        {
            Debug.LogError("SceneSelectorDebug: VisualTreeAsset is not assigned.");
            return;
        }

        rootVisualElement.Clear();

        VisualElement ui = treeAsset.CloneTree();
        rootVisualElement.Add(ui);

        BindButtons(ui);
    }

    private void BindButtons(VisualElement root)
    {
        root.Q<Button>("MainMenu_btn").clicked += LoadMainMenuScene;
        root.Q<Button>("TestScene_btn").clicked += LoadTestScene;
    }

    private void LoadTestScene()
    {
        sceneName = "TestScene";
        SaveScene();
        LoadScene(sceneName);
    }

    private void LoadMainMenuScene()
    {
        sceneName = "MainMenu";
        SaveScene();
        LoadScene(sceneName);
    }

    private void LoadScene(string lclName)
    {
        EditorSceneManager.OpenScene($"Assets/Scenes/{lclName}.unity", OpenSceneMode.Single);
        Logger.Log($"{lclName} Loaded");
    }
    private void SaveScene()
    {
        Logger.Log($"{name} Saved");
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
}
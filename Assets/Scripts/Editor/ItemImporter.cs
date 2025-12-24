using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemImporter : EditorWindow
{
    private TextAsset jsonFile;

    [MenuItem("Tools/Import Items from JSON")]
    public static void ShowWindow()
    {
        GetWindow<ItemImporter>("Item Importer");
    }

    void OnGUI()
    {
        GUILayout.Label("Import Item JSON", EditorStyles.boldLabel);
        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);

        if (GUILayout.Button("Import") && jsonFile != null)
        {
            ImportItems(jsonFile);
        }
    }

    private void ImportItems(TextAsset file)
    {
        string path = EditorUtility.OpenFolderPanel("Choose Output Folder", "Assets", "");
        if (string.IsNullOrEmpty(path)) return;

        // Ensure Unity-relative path
        string relativePath = "Assets" + path.Substring(Application.dataPath.Length);

        ItemJsonList wrapper = JsonUtility.FromJson<ItemJsonList>(file.text);

        foreach (var jsonItem in wrapper.items)
        {
            // Create ScriptableObject
            ItemDataSO item = ScriptableObject.CreateInstance<ItemDataSO>();
            item.Name = jsonItem.Name;
            item.ID = jsonItem.ID;
            item.Description = jsonItem.Description;
            item.Value = jsonItem.Value;

            string assetPath = Path.Combine(relativePath, jsonItem.Name + ".asset");

            AssetDatabase.CreateAsset(item, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Imported " + wrapper.items.Length + " items.");
    }
}
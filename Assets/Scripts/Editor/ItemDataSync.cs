using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class ItemDataSOSync : EditorWindow
{
    private TextAsset jsonFile;
    private string exportPath = "Assets/Scripts/JSONS/Items.json";
    private string importFolder = "Assets/ScriptableObjects/Items"; // where SO assets are created

    [MenuItem("Tools/Item DataSO Sync")]
    public static void ShowWindow()
    {
        GetWindow<ItemDataSOSync>("Item DataSO Sync");
    }

    void OnGUI()
    {
        GUILayout.Label("Item DataSO Sync Tool", EditorStyles.boldLabel);

        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);
        exportPath = EditorGUILayout.TextField("Export Path", exportPath);
        importFolder = EditorGUILayout.TextField("Import Folder", importFolder);

        EditorGUILayout.Space();

        if (GUILayout.Button("Import JSON → ScriptableObjects") && jsonFile != null)
        {
            ImportItems(jsonFile);
        }

        if (GUILayout.Button("Export ScriptableObjects → JSON"))
        {
            ExportItems();
        }
    }

    private void ImportItems(TextAsset file)
    {
        if (!Directory.Exists(importFolder))
        {
            Directory.CreateDirectory(importFolder);
        }

        ItemJsonList wrapper = JsonUtility.FromJson<ItemJsonList>(file.text);
        HashSet<int> seenIds = new HashSet<int>();
        HashSet<string> seenNames = new HashSet<string>();

        foreach (var jsonItem in wrapper.items)
        {
            // Check for duplicates in JSON itself
            if (!seenIds.Add(jsonItem.ID))
                Debug.LogWarning($"Duplicate ID {jsonItem.ID} found in JSON");
            if (!seenNames.Add(jsonItem.Name))
                Debug.LogWarning($"Duplicate Name '{jsonItem.Name}' found in JSON");

            // Try to find by ID first
            ItemDataSO item = FindItemByID(jsonItem.ID);

            if (item == null)
            {
                // Try by Name
                item = FindItemByName(jsonItem.Name);
            }

            if (item == null)
            {
                // Create new if nothing found
                string assetPath = Path.Combine(importFolder, $"{jsonItem.ID}_{jsonItem.Name}.asset");
                item = ScriptableObject.CreateInstance<ItemDataSO>();
                AssetDatabase.CreateAsset(item, assetPath);
            }

            // Update values
            item.Name = jsonItem.Name;
            item.ID = jsonItem.ID;
            item.Description = jsonItem.Description;
            item.Value = jsonItem.Value;

            EditorUtility.SetDirty(item);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Imported/Updated " + wrapper.items.Length + " items.");
    }

    private void ExportItems()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");
        List<ItemJson> exportItems = new List<ItemJson>();

        HashSet<int> seenIds = new HashSet<int>();
        HashSet<string> seenNames = new HashSet<string>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemDataSO data = AssetDatabase.LoadAssetAtPath<ItemDataSO>(path);

            if (!seenIds.Add(data.ID))
                Debug.LogWarning($"Duplicate ID {data.ID} found among ScriptableObjects ({data.Name})");
            if (!seenNames.Add(data.Name))
                Debug.LogWarning($"Duplicate Name '{data.Name}' found among ScriptableObjects (ID {data.ID})");

            exportItems.Add(new ItemJson
            {
                Name = data.Name,
                ID = data.ID,
                Description = data.Description,
                Value = data.Value
            });
        }

        ItemJsonList wrapper = new ItemJsonList { items = exportItems.ToArray() };

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(exportPath, json);

        AssetDatabase.Refresh();
        Debug.Log("Exported " + exportItems.Count + " items to " + exportPath);
    }

    private ItemDataSO FindItemByID(int id)
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemDataSO data = AssetDatabase.LoadAssetAtPath<ItemDataSO>(path);
            if (data.ID == id) return data;
        }
        return null;
    }

    private ItemDataSO FindItemByName(string name)
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemDataSO data = AssetDatabase.LoadAssetAtPath<ItemDataSO>(path);
            if (data.Name == name) return data;
        }
        return null;
    }
}

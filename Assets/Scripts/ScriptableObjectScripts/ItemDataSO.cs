using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Items/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public string Name;
    [TextArea(3, 10)] public string Description;
    public MeshRenderer Renderer;
    public int Value;
    public Material Material;
}
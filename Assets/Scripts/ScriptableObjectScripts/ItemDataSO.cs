using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public string Name;
    public int ID;
    [TextArea(3, 10)]
    public string Description;
    public MeshRenderer Renderer;
    public int Value;
}

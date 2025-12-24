using UnityEngine;

[CreateAssetMenu(fileName = "TransformVariable", menuName = "Scriptable Objects/TransformVariable")]
public class TransformVariable : ScriptableObject
{
    public Transform value;

    public void setValue(Transform val)
    {
        value = val;
    }
    public Transform getValue()
    {
        return value;
    }
    public void ChangeValue(Transform val)
    {
        value = val;
    }
}
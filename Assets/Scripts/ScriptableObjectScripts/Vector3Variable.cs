using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Variable", menuName = "Scriptable Objects/Vector3Variable")]
public class Vector3Variable : ScriptableObject
{
    public Vector3 value;

    public void setValue(Vector3 val)
    {
        value = val;
    }
    public Vector3 getValue()
    {
        return value;
    }
    public void ChangeValue(Vector3 val)
    {
        value = val;
    }
}
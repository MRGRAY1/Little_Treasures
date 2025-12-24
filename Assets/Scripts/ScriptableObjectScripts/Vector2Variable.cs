using UnityEngine;

[CreateAssetMenu(fileName = "Vector2Variable", menuName = "Scriptable Objects/Vector2Variable")]
public class Vector2Variable : ScriptableObject
{
    public Vector2 value;

    public void setValue(Vector2 val)
    {
        value = val;
    }
    public Vector2 getValue()
    {
        return value;
    }
    public void ChangeValue(Vector2 val)
    {
        value = val;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Scriptable Objects/BoolVariable")]
public class BoolVariable : ScriptableObject
{
    public bool value;

    public void setValue(bool val)
    {
        value = val;
    }
    public bool getValue()
    {
        return value;
    }
    public void ChangeValue(bool val)
    {
        value = val;
    }
}

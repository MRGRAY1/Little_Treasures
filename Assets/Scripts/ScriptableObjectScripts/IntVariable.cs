using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Scriptable Objects/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int value;

    public void setValue(int val)
    {
        value = val;
    }
    public int getValue()
    {
        return value;
    }
    public void ChangeValue(int val)
    {
        value += val;
    }
}
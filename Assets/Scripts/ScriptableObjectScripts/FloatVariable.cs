using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "Scriptable Objects/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    public float value;

    public void setValue(float val)
    {
        value = val;
    }
    public float getValue()
    {
        return value;
    }
    public void ChangeValue(float val)
    {
        value += val;
    }
}
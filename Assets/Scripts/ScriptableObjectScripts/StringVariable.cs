using UnityEngine;

[CreateAssetMenu(fileName = "StringVariable", menuName = "Scriptable Objects/StringVariable")]
public class StringVariable : ScriptableObject
{
    public string value;

    public void setValue(string val)
    {
        value = val;
    }
    public string getValue()
    {
        return value;
    }
    public void ChangeValue(string val)
    {
        value += val;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "StringReference", menuName = "Scriptable Objects/StringReference")]
public class StringReference : ScriptableObject
{
    public bool UseConstant = true;
    public string ConstantValue;
    public StringVariable Variable;

    public string value
    {
        get
        {
            return UseConstant ? ConstantValue : Variable.value;
        }
    }
}

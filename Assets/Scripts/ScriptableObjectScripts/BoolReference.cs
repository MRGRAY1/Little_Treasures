using UnityEngine;

[CreateAssetMenu(fileName = "BoolReference", menuName = "Scriptable Objects/BoolReference")]
public class BoolReference : ScriptableObject
{
    public bool UseConstant = true;
    public bool ConstantValue;
    public BoolVariable Variable;

    public bool value
    {
        get
        {
            return UseConstant ? ConstantValue : Variable.value;
        }
    }
}
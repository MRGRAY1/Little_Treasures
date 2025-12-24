using UnityEngine;

[CreateAssetMenu(fileName = "IntReference", menuName = "Scriptable Objects/IntReference")]
public class IntReference : ScriptableObject
{
    public bool UseConstant = true;
    public int ConstantValue;
    public IntVariable Variable;

    public int value
    {
        get
        {
            return UseConstant ? ConstantValue : Variable.value;
        }
    }
}
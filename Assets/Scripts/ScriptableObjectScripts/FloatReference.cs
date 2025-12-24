using UnityEngine;

[CreateAssetMenu(fileName = "FloatReference", menuName = "Scriptable Objects/FloatReference")]
public class FloatReference : ScriptableObject
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public float value
    {
        get
        {
            return UseConstant ? ConstantValue : Variable.value;
        }
    }
}
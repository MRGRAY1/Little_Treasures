using UnityEngine;

[CreateAssetMenu(fileName = "ConfigureRoomSO", menuName = "Scriptable Objects/ConfigureRoomSO")]
public class ConfigureRoomSO : ScriptableObject
{


    public BoolVariable IsDugeonGenerated;
    public BoolVariable IsPlayerSpawned;
}
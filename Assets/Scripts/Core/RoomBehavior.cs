using UnityEngine;

public enum RoomType { Start, Normal, Boss }

public class RoomBehavior : MonoBehaviour
{

    [SerializeField]
    private GameObject[] doors; // 0=N, 1=E, 2=S, 3=W
    [SerializeField]
    private GameObject[] walls; // 0=N, 1=E, 2=S, 3=W
    [SerializeField]
    private GameObject spawnPoint;
    public GameObject publicSpawnPoint { get; private set; }

    public RoomType room_Type;

    private void Awake()
    {
        if (spawnPoint != null)
        {
            //Logger.Log($"SpawnPoint RB {spawnPoint.transform.position}");

            publicSpawnPoint = spawnPoint;
        }
    }
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    public void SetType(RoomType type)
    {
        room_Type = type;
    }
}
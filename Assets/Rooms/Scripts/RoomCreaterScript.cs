using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RoomCreaterScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> roomTypes;
    [SerializeField]
    private List<GameObject> possibleStartingRooms;

    [SerializeField]
    private Grid gridSystem;

    [SerializeField]
    private int randomStartingRoom;

    private void Start()
    {
        this.Initialize();
    }
    private void Initialize()
    {
        this.randomStartingRoom = Random.Range(0, this.possibleStartingRooms.Count);
        Instantiate(this.possibleStartingRooms[this.randomStartingRoom], this.gridSystem.transform.position, Quaternion.identity);

    }
}

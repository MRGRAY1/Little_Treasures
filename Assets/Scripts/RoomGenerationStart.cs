using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This is to only start the generation of the room.
/// Nothing Else
/// </summary>
public class RoomGenerationStart : MonoBehaviour
{
    [SerializeField]
    private GameEvent roomGenStart;

    private void Start()
    {
        roomGenStart?.Raise();
    }
}

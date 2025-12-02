using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [Header("Generation Steps")]
    [SerializeField]
    private List<BoolVariable> GenSteps = new List<BoolVariable>();

    [Header("Generation Complete Event")]
    [SerializeField]
    private GameEvent FinishedGeneration;


    public void CheckStates()
    {
        foreach (var step in GenSteps)
        {
            if (!step.getValue())
                return;
        }
        FinishedGeneration?.Raise();

        ResetValues();
    }

    private void ResetValues()
    {
        foreach (var step in GenSteps)
            step.setValue(false);
    }
}

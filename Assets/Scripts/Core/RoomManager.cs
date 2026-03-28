using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Generation Steps")] [SerializeField]
    private List<BoolVariable> GenSteps = new List<BoolVariable>();

    private void OnEnable()
    {
        GameEvents.DungeonGenerationComplete += CheckStates;
        GameEvents.PlayerSpawnComplete += CheckStates;
        GameEvents.ItemsSpawnComplete += CheckStates;
    }

    private void OnDisable()
    {
        GameEvents.DungeonGenerationComplete -= CheckStates;
        GameEvents.PlayerSpawnComplete -= CheckStates;
        GameEvents.ItemsSpawnComplete -= CheckStates;
    }

    public void CheckStates(object sender)
    {
        foreach (var step in GenSteps)
        {
            if (!step.getValue())
                return;
        }

        GameEvents.GenerationsComplete?.Invoke(this, false);
        ResetValues();
    }


    private void ResetValues()
    {
        foreach (var step in GenSteps)
            step.setValue(false);
    }
}
using System;
using UnityEngine;

/// <summary>
/// Summary Of File
/// </summary>

// ToDo:
// - Add specific logic or initialization here
public class InventoryManager : MonoBehaviour
{
    #region Variables

    // Declare fields, constants, and serialized variables here.
    [Header("Items")] [SerializeField] private int currCoinAmount;
    [SerializeField] private IntVariable globalCoinAmount;

    #endregion

    #region Functions

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        GameEvents.PickUpCoin += PickupCoin;
    }

    private void OnDisable()
    {
        GameEvents.PickUpCoin -= PickupCoin;
    }

    private void Initialize()
    {
        currCoinAmount = globalCoinAmount.getValue();
    }

    public void PickupCoin(object sender, int amount)
    {
        Logger.Log($"Pickup Coin Amount: {amount}");
        currCoinAmount += amount;
        globalCoinAmount.setValue(currCoinAmount);
    }

    #endregion
}
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
    [SerializeField] private int redGemCount;
    [SerializeField] private int blueGemCount;
    [SerializeField] private int greenGemCount;

    #endregion

    #region Functions

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Initialize()
    {
        currCoinAmount = 0;
    }

    public void PickupCoin(int amount)
    {
        Logger.Log($"Pickup Coin Amount: {amount}");
        AddCoin(amount);
    }

    public void PickUpGem(GemTypes type, int amount)
    {
        switch (type)
        {
            case GemTypes.RedGem:
                AddRedGem(amount);
                break;
            case GemTypes.BlueGem:
                AddBlueGem(amount);
                break;
            case GemTypes.GreenGem:
                AddGreenGem(amount);
                break;
        }
    }

    public void AddCoin(int amount)
    {
        currCoinAmount += amount;
    }

    public void RemoveCoin(int amount)
    {
        currCoinAmount -= amount;
        if (currCoinAmount <= 0)
        {
            currCoinAmount = 0;
        }
    }

    public void AddRedGem(int amount)
    {
        redGemCount += amount;
    }

    public void AddBlueGem(int amount)
    {
        blueGemCount += amount;
    }

    public void AddGreenGem(int amount)
    {
        greenGemCount += amount;
    }

    public void RemoveRedGem(int amount)
    {
        redGemCount -= amount;
        if (redGemCount <= 0)
        {
            redGemCount = 0;
        }
    }

    public void RemoveBlueGem(int amount)
    {
        blueGemCount -= amount;
        if (blueGemCount <= 0)
        {
            blueGemCount = 0;
        }
    }

    public void RemoveGreenGem(int amount)
    {
        greenGemCount -= amount;
        if (greenGemCount <= 0)
        {
            greenGemCount = 0;
        }
    }

    #endregion
}
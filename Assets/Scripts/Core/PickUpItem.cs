using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Summary Of File
/// </summary>

// ToDo:
// - Add specific logic or initialization here
public abstract class PickUpItem : MonoBehaviour
{
    #region Variables
    // Declare fields, constants, and serialized variables here.
    [SerializeField]
    public ItemDataSO itemData;
    #endregion

    #region Functions
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePickup(other.gameObject);
            Logger.Log($"Player Pickup: {this.name}");
            Destroy(this);
        }
    }

    protected abstract void HandlePickup(GameObject player);

    #endregion
}
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
    public MeshRenderer _meshRenderer;

    #region Functions

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePickup(other.gameObject);
            Logger.Log($"Player Pickup: {this.name}");
            Destroy(gameObject);
        }
    }

    protected virtual void Awake()
    {
        Initialize();
    }

    protected abstract void HandlePickup(GameObject player);
    protected abstract void Initialize();

    #endregion
}
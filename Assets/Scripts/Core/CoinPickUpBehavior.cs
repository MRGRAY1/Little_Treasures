using UnityEngine;

public class CoinPickUpBehavior : PickUpItem
{
    [SerializeField] private CoinDataSO _coinData;

    protected override void HandlePickup(GameObject player)
    {
        player.GetComponent<InventoryManager>().PickupCoin(_coinData.Value);
    }

    protected override void Initialize()
    {
        _meshRenderer.material = _coinData.Material;
    }
}
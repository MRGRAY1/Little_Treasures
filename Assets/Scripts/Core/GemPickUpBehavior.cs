using UnityEngine;

public class GemPickUpBehavior : PickUpItem
{
    [SerializeField] private GemDataSO _gemData;

    protected override void HandlePickup(GameObject player)
    {
        player.GetComponent<InventoryManager>().PickUpGem(_gemData.GemType, _gemData.Value);
    }

    protected override void Initialize()
    {
        _meshRenderer.material = _gemData.Material;
    }
}
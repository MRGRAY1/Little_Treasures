using UnityEngine;

public class CoinPickUpBehavior : PickUpItem
{
    protected override void HandlePickup(GameObject player)
    {
        GameEvents.PickUpCoin?.Invoke(this, itemData.Value);
    }
}
using UnityEngine;

public class CoinPickUpBehavior : PickUpItem
{
    private EventIndex pickUpEvent = EventIndex.PickUpCoin;
    protected override void HandlePickup(GameObject player)
    {
        EventBus.Publish(pickUpEvent, itemData.Value);
    }
}
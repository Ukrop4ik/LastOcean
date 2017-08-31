using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDecorator : MonoBehaviour
{
    public int slotId;
    public string itemId;

    [SerializeField]
    ShipDecorator decorator;

    public void SetToSlot(string id)
    {
        itemId = id;
        decorator.AddItemToSlot(slotId, id);
    }
    public void RemoveFromSlot()
    {
        itemId = "";
        decorator.RemoveFromSlot(slotId);
    }


}

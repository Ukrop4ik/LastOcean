using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDecorator : MonoBehaviour
{
    public int slotId;
    public string itemId;
    public SlotType Type;
    public SpecialType SpecialType;
    public int SlotMass;


    [SerializeField]
    ShipDecorator decorator;

    public void SetToSlot(string id, int mass)
    {
        itemId = id;
        decorator.AddItemToSlot(slotId, id, mass);
    }
    public void RemoveFromSlot(int mass)
    {
        itemId = "";
        decorator.RemoveFromSlot(slotId, mass);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDecorator : MonoBehaviour {


    [SerializeField]
    private string _shipId;
    [SerializeField]
    private List<ShipSlot> slots = new List<ShipSlot>();


    private void Start()
    {
        Player.Instance().SetShipDecorator(this);    
    }

    public string GetShipId()
    {
        return _shipId;
    }
    public void AddItemToSlot(int slotId, string itemId)
    {
        foreach(ShipSlot slot in slots)
        {
            if(slotId == slot.SlotId)
            {
                slot.IteminslotId = itemId;
            }
        }
    }
    public void RemoveFromSlot(int slotId)
    {
        foreach (ShipSlot slot in slots)
        {
            if (slotId == slot.SlotId)
            {
                slot.IteminslotId = "";
            }
        }
    }

    public Dictionary<int,string> GetWeaponData()
    {
        Dictionary<int, string> dict = new Dictionary<int, string>();

        foreach(ShipSlot slot in slots)
        {
            dict.Add(slot.SlotId, slot.IteminslotId);
        }

        return dict;
    }

    [System.Serializable]
    private class ShipSlot
    {
        public int SlotId;
        public string IteminslotId;

        public ShipSlot(int id, string itemid)
        {
            SlotId = id;
            IteminslotId = itemid;
        }
    }
}

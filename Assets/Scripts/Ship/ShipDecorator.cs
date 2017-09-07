using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDecorator : MonoBehaviour {


    [SerializeField]
    private string _shipId;
    [SerializeField]
    private List<ShipSlot> slots = new List<ShipSlot>();
    [SerializeField]
    private List<Transform> slots_transform = new List<Transform>();

    private void Start()
    {
        Player.Instance().SetShipDecorator(this);    
    }
    public void DestroyDecorator()
    {
        Destroy(this.gameObject);
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

    public Transform GetSlotFromId(int id)
    {
        return slots_transform[id-1];
    }

    public void CreateItemInSlot(Transform slot, string itemId)
    {
        GameObject itemObj = Instantiate(Resources.Load("Items/" + itemId) as GameObject, slot);
        itemObj.name = itemId;
        itemObj.transform.localScale = Vector3.one;
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
    public class ShipSlot
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

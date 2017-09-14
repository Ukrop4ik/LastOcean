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
    [SerializeField]
    private float _maximumShipMass;
    [SerializeField]
    private float _currentShipMass;

    [SerializeField]
    private ShipStat _stats;

    public bool isReady = false;

    private void Start()
    {
        Player.Instance().SetShipDecorator(this);    
    }

    private void Update()
    {
        if(_maximumShipMass >= _currentShipMass)
        {
            isReady = true;
        }
        else
        {
            isReady = false;
        }
    }

    public void DestroyDecorator()
    {
        Destroy(this.gameObject);
    }

    public string GetShipId()
    {
        return _shipId;
    }
    public void AddItemToSlot(int slotId, string itemId, int mass)
    {
        foreach(ShipSlot slot in slots)
        {
            if(slotId == slot.SlotId)
            {
                foreach(PlayerDB.ShipData data in PlayerDB.Instance().GetShips())
                {
                    if(data.ID == _shipId)
                    {
                        bool isFindSlot = false;

                        foreach(ShipDecorator.ShipSlot _slot in data.Slots)
                        {
                            if(_slot.SlotId == slot.SlotId)
                            {
                                _slot.IteminslotId = itemId;
                                isFindSlot = true;
                                break;
                            }
                        }

                        if(!isFindSlot)
                            data.Slots.Add(new ShipSlot(slotId, itemId));
                    }
                }
                slot.IteminslotId = itemId;
                _currentShipMass += mass;
                return;
            }
        }
    }

    public Transform GetSlotFromId(int id)
    {
        return slots_transform[id-1];
    }

    public Item CreateItemInSlot(Transform slot, string itemId)
    {
        if (itemId == "") return null;
        GameObject itemObj = Instantiate(Resources.Load("Items/" + itemId) as GameObject, slot);
        itemObj.name = itemId;
        itemObj.transform.localScale = Vector3.one;
        return itemObj.GetComponent<Item>();
    }
    public void RemoveFromSlot(int slotId, int mass)
    {
        foreach (ShipSlot slot in slots)
        {
            if (slotId == slot.SlotId)
            {
                foreach (PlayerDB.ShipData data in PlayerDB.Instance().GetShips())
                {
                    if (data.ID == _shipId)
                    {
                        foreach (ShipDecorator.ShipSlot _slot in data.Slots)
                        {
                            if (_slot.SlotId == slot.SlotId)
                            {
                                _slot.IteminslotId = "";
                            }
                        }
                    }
                }

                slot.IteminslotId = "";
                _currentShipMass -= mass;
                return;
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

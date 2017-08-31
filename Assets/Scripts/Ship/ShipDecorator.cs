using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDecorator : MonoBehaviour {


    [SerializeField]
    private string _shipId;
    [SerializeField]
    private List<ShipSlot> slots = new List<ShipSlot>();


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

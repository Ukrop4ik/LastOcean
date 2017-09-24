using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Image massSlider;
    [SerializeField]
    private Image speedSlider;
    [SerializeField]
    private Text speedText;
    [SerializeField]
    private Text massText;
    [SerializeField]
    private Text hullText;
    [SerializeField]
    private Text armorText;

    [SerializeField]
    public ShipStat _stats;

    public bool isReady = false;

    [SerializeField]
    private float SpeedStart = 0f;
    [SerializeField]
    private float _massCoef;

    private void OnEnable()
    {
        Player.Instance().SetShipDecorator(this);
        StartCoroutine(UpdateUI());
    }
    public void OnDisable()
    {
        StopCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.2f);

        speedText.text = _stats.GetSpeed().ToString() + " M";
        massText.text = _currentShipMass + " / " + _maximumShipMass;
        hullText.text = "HULL: " + _stats.GetHullValue(true).ToString();
        armorText.text = "ARMOR: " + _stats.GetArmorValue(true).ToString();

        StartCoroutine(UpdateUI());
    }

    private void Update()
    {
        if(_maximumShipMass >= _currentShipMass)
        {
            isReady = true;
            massSlider.color = Color.white;
        }
        else
        {
            isReady = false;
            massSlider.color = Color.red;
            massSlider.fillAmount = 1;
        }
        if(_currentShipMass > 0)
        {
            massSlider.fillAmount = _currentShipMass / _maximumShipMass;
        }
        else
        {
            massSlider.fillAmount = 0;
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
                _stats.SetSpeed(SpeedStart);
                _stats.SetSpeed(_stats.GetSpeed() - (_currentShipMass /_maximumShipMass) * _massCoef);
                speedSlider.fillAmount = (_stats.GetSpeed() / SpeedStart);
               // CreateItemInSlot(GetSlotFromId(slot.SlotId), itemId);
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
                _stats.SetSpeed(SpeedStart);
                _stats.SetSpeed(_stats.GetSpeed() - (_currentShipMass / _maximumShipMass) * _massCoef);
                speedSlider.fillAmount = (_stats.GetSpeed() / SpeedStart);
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

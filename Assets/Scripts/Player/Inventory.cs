﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [SerializeField]
    private List<ItemStack> _items = new List<ItemStack>();

    private void Start()
    {

    }

    private void OnEnable()
    {
       Invoke("LoadFromPlayerData", 0.3f);
       StartCoroutine(InvUpdateRoutine());
    }
    private void OnDesable()
    {
        StopCoroutine(InvUpdateRoutine());
    }
    public bool IsInventoryContainsItem(string id)
    {
        bool test = false;
        foreach (ItemStack stack in _items)
        {
            if (stack.ID == id)
            {
                test = true;
                break;
            }
        }

        return test;
    }

    public void AddNewItem(GameObject item)
    {
        item.transform.SetParent(this.transform);
        item.transform.localScale = Vector3.one;
    }

    public void SetItemToStack(string id, int value, GameObject obj)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Item item;
            item = transform.GetChild(i).gameObject.GetComponent<Item>();

            if(item.GetId() == id)
            {
                item.SetCount(item.GetCount() + value);
            }

        }
        Destroy(obj);
    }

    private IEnumerator InvUpdateRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        _items.Clear();

        Debug.Log("Update inventory!");

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                bool isAdd = true;
                Item item;
                item = transform.GetChild(i).gameObject.GetComponent<Item>();

                if (_items.Count > 0)
                {
                    foreach (ItemStack stack in _items)
                    {
                        if (stack.ID == item.GetId())
                        {
                            isAdd = false;
                            stack.Count += item.GetCount();
                        }
                    }
                }

                if (isAdd)
                    _items.Add(new ItemStack(item.GetId(), item.GetCount()));
            }
        }

        SaveToPlayerData();

        StartCoroutine(InvUpdateRoutine());
    }

    public void SaveToPlayerData()
    {
        PlayerDB.Instance().ClearItemData();

        if (_items.Count > 0)
        {

            foreach (ItemStack item in _items)
            {
                PlayerDB.Instance().AddItem(new PlayerDB.ItemData(item.ID, item.Count));

            }
        }
    }

    [ContextMenu("LoadFromPlayerData")]
    public void LoadFromPlayerData()
    {
        List<PlayerDB.ItemData> items = Player.Instance().Data.LoadInventoryItems();

        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        foreach(PlayerDB.ItemData item in items)
        {
            GameObject itemObj = Instantiate(Resources.Load("Items/" + item.ItemId)) as GameObject;
            itemObj.name = item.ItemId;
            itemObj.transform.SetParent(transform);
            itemObj.transform.localScale = Vector3.one;

            Item i = itemObj.GetComponent<Item>();
            i.SetCount(item.ItemCount);
        }
    }

    [System.Serializable]
    private class ItemStack
    {
        public string ID;
        public int Count;
        public ItemStack(string id, int count)
        {
            ID = id;
            Count = count;

        }
    }
}

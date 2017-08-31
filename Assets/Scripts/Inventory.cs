using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [SerializeField]
    private List<ItemStack> _items = new List<ItemStack>();

    private void Start()
    {
        UpdateInventory();
    }

    public bool IsInventoryContainsItem(string id)
    {
        UpdateInventory();
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

    public void SetItemToStack(string id, int value)
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

        UpdateInventory();
    }

    public void UpdateInventory()
    {
        _items.Clear();

        if (transform.childCount > 0)
        {
            for(int i = 0; i<transform.childCount; i++)
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

                if(isAdd)
                    _items.Add(new ItemStack(item.GetId(), item.GetCount()));
            }
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

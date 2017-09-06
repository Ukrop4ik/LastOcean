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

    private void OnEnable()
    {
       Invoke("LoadFromPlayerData", 0.3f);
    }
    private void OnDesable()
    {

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

    public void AddNewItem(GameObject item)
    {
        item.transform.SetParent(this.transform);
        item.transform.localScale = Vector3.one;
        UpdateInventory();
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
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        StartCoroutine(InvUpdateRoutine());
    }

    private IEnumerator InvUpdateRoutine()
    {
        yield return new WaitForSeconds(0.075f);

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
    }

    [ContextMenu("SaveToPlayer")]
    public void SaveToPlayerData()
    {
        if(_items.Count > 0)
        {
            List<PlayerDB.ItemData> items = new List<PlayerDB.ItemData>();
            
            foreach(ItemStack item in _items)
            {
                Debug.Log("item: " + item.ID);
                items.Add(new PlayerDB.ItemData(item.ID, item.Count));
            }

            Player.Instance().Data.SaveData(items, Player.Instance().NickName);
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

        UpdateInventory();
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

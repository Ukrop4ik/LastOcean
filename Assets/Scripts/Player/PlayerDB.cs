using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Diagnostics;
using System.IO;

public class PlayerDB : MonoBehaviour {

    private JsonData _data;
    public string NickName;
    [SerializeField]
    private List<ItemData> _inventoryitemdata = new List<ItemData>();

    private static PlayerDB instance;
    public static PlayerDB Instance() { return instance; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _data = LoadData();
    }

    public List<ItemData> LoadInventoryItems()
    {
        List<ItemData> id = new List<ItemData>();

        if (_data["Inventory"].Count > 0)
        {
            for (int i = 0; i < _data["Inventory"].Count; i++)
            {
                id.Add(new ItemData((string)_data["Inventory"][i]["ItemId"], (int)_data["Inventory"][i]["ItemCount"]));
            }
        }

        if(id.Count == 0)
            UnityEngine.Debug.Log("No items to load!");

        return id;
    }

    [ContextMenu("SaveData")]
    public void SaveData(List<ItemData> items, string NickName)
    {
        PlayerData PD = new PlayerData(NickName, items);
        File.WriteAllText(Application.persistentDataPath + "/" + Player.Instance().NickName + ".prf", JsonMapper.ToJson(PD));
        OpenProfileFolder();
    }

    public JsonData LoadData()
    {

        if (File.Exists(Application.persistentDataPath + "/" + Player.Instance().NickName + ".prf"))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/" + Player.Instance().NickName + ".prf");
            return JsonMapper.ToObject(data);
        }
        else
            return null;
    }

    public class PlayerData
    {
        public string PlayerName;
        public List<ItemData> Inventory;

        public PlayerData(string Name, List<ItemData> Items)
        {
            PlayerName = Name;
            Inventory = Items;
        }
    }

    [System.Serializable]
    public class ItemData
    {
        public string ItemId;
        public int ItemCount;

        public ItemData(string Id, int count)
        {
            ItemId = Id;
            ItemCount = count;
        }
    }


    [ContextMenu("OpenProfileFolder")]
    public void OpenProfileFolder()
    {
        Process.Start(Application.persistentDataPath);
    }
}


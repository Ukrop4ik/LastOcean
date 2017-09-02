using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Diagnostics;
using System.IO;

public class PlayerDB : MonoBehaviour {

    private JsonData _data;

    [SerializeField]
    private List<ItemData> _inventoryitemdata = new List<ItemData>();

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
    public void SaveData(List<ItemData> items)
    {
        PlayerData PD = new PlayerData("Nick", items);
        File.WriteAllText(Application.persistentDataPath + "/" + PD.PlayerName + ".json", JsonMapper.ToJson(PD));
        OpenProfileFolder();
    }

    public JsonData LoadData()
    {

        if(File.Exists(Application.persistentDataPath + "/" + "Nick" + ".json"))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/" + "Nick" + ".json");
            return JsonMapper.ToObject(data);
        }
        else
        {
            List<ItemData> startItems = new List<ItemData>();
            startItems.Add(new ItemData("small_gun_tutorial", 500));
            startItems.Add(new ItemData("small_gun_art", 500));

            PlayerData _PD = new PlayerData("Nick", startItems);
            File.WriteAllText(Application.persistentDataPath + "/" + _PD.PlayerName + ".json", JsonMapper.ToJson(_PD));

            string data = File.ReadAllText(Application.persistentDataPath + "/" + "Nick" + ".json");
            return JsonMapper.ToObject(data);
        }


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


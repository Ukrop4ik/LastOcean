using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Diagnostics;
using System.IO;

public class PlayerDB : MonoBehaviour {

    private int gold;
    private JsonData _data;
    public string NickName;
    [SerializeField]
    private List<ItemData> _inventoryitemdata = new List<ItemData>();
    [SerializeField]
    private List<Tasks> _tasks = new List<Tasks>();
    [SerializeField]
    private List<Resources> _resources = new List<Resources>();
    [SerializeField]
    private List<ShipData> _ships = new List<ShipData>();

    private static PlayerDB instance;
    public static PlayerDB Instance() { return instance; }

    private void Awake()
    {
        instance = this;
    }

    public List<ShipData> GetShips()
    {
        return _ships;
    }

    public void AddShip( ShipData data)
    {
        _ships.Add(data);
    }

    public int GetPlayerGold()
    {
        int value = 0;
        foreach(Resources res in _resources)
        {
            if(res.Id == "Gold")
            {
                value = res.Count;
            }
        }
        return value;
    }

    public void SetPlayerGold(int value)
    {
        foreach (Resources res in _resources)
        {
            if (res.Id == "Gold")
            {
                res.Count += value;
                return;
            }
        }
    }

    private void Start()
    {
        _data = LoadData();
        LoadTasks();
        LoadName();
        LoadResources();
        LoadShips();
    }

    public List<Tasks> GetTasks()
    {
        return _tasks;
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

    public List<ItemData> GetItems()
    {
        return _inventoryitemdata;
    }
    public void AddItem(ItemData item)
    {
        _inventoryitemdata.Add(item);
    }
    public void ClearItemData()
    {
        _inventoryitemdata.Clear();
    }

    public void Save()
    {
        PlayerData PD = new PlayerData(Player.Instance().NickName, _inventoryitemdata, _tasks, _resources, _ships);
        File.WriteAllText(Application.persistentDataPath + "/" + Player.Instance().NickName + ".prf", JsonMapper.ToJson(PD));
    }

    [ContextMenu("SaveData")]
    public void SaveData(List<ItemData> items, string NickName, List<Tasks> tasks)
    {
        PlayerData PD = new PlayerData(NickName, items, tasks, _resources, _ships);
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

    private void LoadResources()
    {
        for (int i = 0; i < _data["Resources"].Count; i++)
        {
            _resources.Add(new Resources((string)_data["Resources"][i]["Id"], (int)_data["Resources"][i]["Count"]));
        }
    }

    private void LoadShips()
    {

        for (int i = 0; i < _data["Ships"].Count; i++)
        {
            List<ShipDecorator.ShipSlot> slots = new List<ShipDecorator.ShipSlot>();

            for(int s = 0; s < _data["Ships"][i]["Slots"].Count; s++)
            {
                slots.Add(new ShipDecorator.ShipSlot((int)_data["Ships"][i]["Slots"][s]["SlotId"], (string)_data["Ships"][i]["Slots"][s]["IteminslotId"]));
            }

            ShipData data = new ShipData((string)_data["Ships"][i]["ID"], slots);
            _ships.Add(data);
        }
    }

    private void LoadTasks()
    {
        for(int i = 0; i < _data["Tasks"].Count; i++)
        {
            _tasks.Add(new Tasks((string)_data["Tasks"][i]["ID"], (int)_data["Tasks"][i]["Time"]));
        }
    }
    private void LoadName()
    {
        NickName = (string)_data["PlayerName"];
    }

    private void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("SaveData");
        Save();
    }


    public class PlayerData
    {
        public string PlayerName;
        public List<ItemData> Inventory;
        public List<Tasks> Tasks;
        public List<Resources> Resources;
        public List<ShipData> Ships;

        public PlayerData(string Name, List<ItemData> Items, List<Tasks> tasks, List<Resources> resources, List<ShipData> ships)
        {
            Resources = resources;
            PlayerName = Name;
            Inventory = Items;
            Tasks = tasks;
            Ships = ships;
        }
    }
    [System.Serializable]
    public class Resources
    {
        public string Id;
        public int Count;

        public Resources(string id, int count)
        {
            Id = id;
            Count = count;
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
    [System.Serializable]
    public class Tasks
    {
        public string ID;
        public int Time;

        public Tasks(string Id, int time)
        {
            ID = Id;
            Time = time;
        }
    }
    [System.Serializable]
    public class ShipData
    {
        public string ID;
        public List<ShipDecorator.ShipSlot> Slots;
        public ShipData(string Id, List<ShipDecorator.ShipSlot> slots)
        {
            ID = Id;
            Slots = slots;
        }
    }


    [ContextMenu("OpenProfileFolder")]
    public void OpenProfileFolder()
    {
        Process.Start(Application.persistentDataPath);
    }
}


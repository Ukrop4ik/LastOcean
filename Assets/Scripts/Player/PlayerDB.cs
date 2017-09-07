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
    [SerializeField]
    private List<Tasks> _tasks = new List<Tasks>();

    private static PlayerDB instance;
    public static PlayerDB Instance() { return instance; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _data = LoadData();
        LoadTasks();
        LoadName();
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
        PlayerData PD = new PlayerData(Player.Instance().NickName, _inventoryitemdata, _tasks);
        File.WriteAllText(Application.persistentDataPath + "/" + Player.Instance().NickName + ".prf", JsonMapper.ToJson(PD));
    }

    [ContextMenu("SaveData")]
    public void SaveData(List<ItemData> items, string NickName, List<Tasks> tasks)
    {
        PlayerData PD = new PlayerData(NickName, items, tasks);
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

        public PlayerData(string Name, List<ItemData> Items, List<Tasks> tasks)
        {
            PlayerName = Name;
            Inventory = Items;
            Tasks = tasks;
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


    [ContextMenu("OpenProfileFolder")]
    public void OpenProfileFolder()
    {
        Process.Start(Application.persistentDataPath);
    }
}


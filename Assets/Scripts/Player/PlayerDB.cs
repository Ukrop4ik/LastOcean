using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Diagnostics;
using System.IO;
using Combu;

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

    public ShipDecorator _currentShipDecorator;

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

    #region("Resources")
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
    public int GetPlayerMetal()
    {
        int value = 0;
        foreach (Resources res in _resources)
        {
            if (res.Id == "Metal")
            {
                value = res.Count;
            }
        }
        return value;
    }

    public void SetPlayerMetal(int value)
    {
        foreach (Resources res in _resources)
        {
            if (res.Id == "Metal")
            {
                res.Count += value;
                return;
            }
        }
    }
    public int GetPlayerFuel()
    {
        int value = 0;
        foreach (Resources res in _resources)
        {
            if (res.Id == "Fuel")
            {
                value = res.Count;
            }
        }
        return value;
    }

    public void SetPlayerFuel(int value)
    {
        foreach (Resources res in _resources)
        {
            if (res.Id == "Fuel")
            {
                res.Count += value;
                return;
            }
        }
    }
    public int GetPlayerGems()
    {
        int value = 0;
        foreach (Resources res in _resources)
        {
            if (res.Id == "Gems")
            {
                value = res.Count;
            }
        }
        return value;
    }

    public void SetPlayerGems(int value)
    {
        foreach (Resources res in _resources)
        {
            if (res.Id == "Gems")
            {
                res.Count += value;
                return;
            }
        }
    }
#endregion

    private void Start()
    {
        //_data = LoadData();
        _inventoryitemdata = LoadInventoryItems();
        LoadName();
        LoadResources();
        LoadShips();
        LoadTasks();

    }



    public List<Tasks> GetTasks()
    {
        return _tasks;
    }
    public void AddTask(TimelineEvent task)
    {
        _tasks.Add(new Tasks(task.EventId, task.Endtime, task.Starttime, task.UnikalId, task.IsMomental ? 1 : 0));
        GameObject obj = Instantiate(UnityEngine.Resources.Load("Events/" + task.EventId) as GameObject, this.transform);
        obj.name = task.UnikalId.ToString();
    }
    public void RemoveTask(TimelineEvent task)
    {
        Tasks tascktoremove = null;
        foreach(Tasks t in _tasks)
        {
            if(t.UnikalId == task.UnikalId)
            {
                tascktoremove = t;
            }
            
        }
        if(tascktoremove != null)
            _tasks.Remove(tascktoremove);
    }
    public List<ItemData> LoadInventoryItems()
    {
        List<ItemData> id = new List<ItemData>();
        JsonData d = JsonMapper.ToObject(CombuManager.localUser.customData["_inventoryitemdata"].ToString());

        if (d.Count > 0)
        {
            for (int i = 0; i < d.Count; i++)
            {
                id.Add(new ItemData((string)d[i]["ItemId"], (int)d[i]["ItemCount"]));
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
    public void AddNewItem(ItemData item)
    {
        _inventoryitemdata.Add(item);
    }
    public void AddItem(string itemId, int value)
    {
        foreach(ItemData i in _inventoryitemdata)
        {
            if (i.ItemId == itemId)
                i.ItemCount = value;
        }
    }
    public void ClearItemData()
    {
        _inventoryitemdata.Clear();
    }

    public void SaveShips()
    {
        string d = JsonMapper.ToJson(_ships);
        CombuManager.localUser.customData["_ships"] = d;
        CombuManager.localUser.Update((bool success, string error) =>
        {
            UnityEngine.Debug.Log(d);
        });
    }

    public void SaveItems()
    {
        string d = JsonMapper.ToJson(_inventoryitemdata);
        CombuManager.localUser.customData["_inventoryitemdata"] = d;
        CombuManager.localUser.Update((bool success, string error) =>
        {
            UnityEngine.Debug.Log(d);
        });
    }

    public void SaveTasks()
    {
        string d = JsonMapper.ToJson(_tasks);
        CombuManager.localUser.customData["_tasks"] = d;
        CombuManager.localUser.Update((bool success, string error) =>
        {
            UnityEngine.Debug.Log(d);
        });
    }

    public void SaveResources()
    {
        Combu.Inventory.Load(CombuManager.localUser.id, (Combu.Inventory[] items, string error) =>
        {
            foreach (Resources rs in _resources)
            {
                foreach (Combu.Inventory item in items)
                {
                    if(rs.Id == "Gold" && item.name == "gold")
                    {
                        item.quantity = rs.Count;
                        item.Update((bool __success, string __error) => {});
                    }
                    if (rs.Id == "Metal" && item.name == "metal")
                    {
                        item.quantity = rs.Count;
                        item.Update((bool __success, string __error) => { });
                    }
                    if (rs.Id == "Fuel" && item.name == "fuel")
                    {
                        item.quantity = rs.Count;
                        item.Update((bool __success, string __error) => { });
                    }
                    if (rs.Id == "Gems" && item.name == "gems")
                    {
                        item.quantity = rs.Count;
                        item.Update((bool __success, string __error) => { });
                    }
                }
            }
        });
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

    [ContextMenu("Save")]
    public void Save()
    {
        SaveItems();
        SaveResources();
        SaveShips();
        SaveTasks();
    }

    private void LoadResources()
    {
        Combu.Inventory.Load(CombuManager.localUser.id, (Combu.Inventory[] items, string error) =>
        {

            foreach(Combu.Inventory item in items)
            {
                if(item.name == "gold")
                {
                    _resources.Add(new Resources("Gold", item.quantity));
                }
                if (item.name == "metal")
                {
                    _resources.Add(new Resources("Metal", item.quantity));
                }
                if (item.name == "fuel")
                {
                    _resources.Add(new Resources("Fuel", item.quantity));
                }
                if (item.name == "gems")
                {
                    _resources.Add(new Resources("Gems", item.quantity));
                }
            }


        });


    }

    private void LoadShips()
    {
        UnityEngine.Debug.Log("Loaded Ships");

        JsonData d = JsonMapper.ToObject(CombuManager.localUser.customData["_ships"].ToString());

        if (d.Count == 0) return;


        for (int i = 0; i < d.Count; i++)
        {
            List<ShipDecorator.ShipSlot> slots = new List<ShipDecorator.ShipSlot>();

            for(int s = 0; s < d[i]["Slots"].Count; s++)
            {
                slots.Add(new ShipDecorator.ShipSlot((int)d[i]["Slots"][s]["SlotId"], (string)d[i]["Slots"][s]["IteminslotId"]));
            }

            ShipData data = new ShipData((string)d[i]["ID"], slots);
            _ships.Add(data);
        }

    }

    private void LoadTasks()
    {
        JsonData d = JsonMapper.ToObject(CombuManager.localUser.customData["_tasks"].ToString());

        UnityEngine.Debug.Log("Loaded Events");
        if (d.Count == 0) return;
        for (int i = 0; i < d.Count; i++)
        {
            Tasks t = new Tasks((string)d[i]["ID"], (int)d[i]["TimeEnd"], (int)d[i]["TimeStart"], (int)d[i]["UnikalId"], (int)d[i]["IsMomental"]);
            _tasks.Add(t);
            NetworkData.Instance().AddEventToTimeline(new TimelineEvent(t.TimeStart, t.TimeEnd, t.ID, t.UnikalId, t.IsMomental == 1));
            GameObject obj = Instantiate(UnityEngine.Resources.Load("Events/" + t.ID) as GameObject, this.transform);
            obj.name = t.UnikalId.ToString();
        }
    }
    private void LoadName()
    {
        UnityEngine.Debug.Log("Loaded Name");
        NickName = CombuManager.localUser.userName;
    }

    private void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("SaveData");
        Save();
    }


    private IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(10f);
        Save();
        StartCoroutine(AutoSave());
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
        public int TimeStart;
        public int TimeEnd;
        public int UnikalId;
        public int IsMomental;
        public Tasks(string Id, int timeend, int timestart, int Unikal, int isMomental)
        {
            ID = Id;
            TimeEnd = timeend;
            TimeStart = timestart;
            UnikalId = Unikal;
            IsMomental = isMomental;
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


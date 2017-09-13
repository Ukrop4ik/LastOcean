using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : Combu.User
{

    string _inventoryitemdata = "";
    string _tasks = "";
    string _ships = "";

    public string InventoryItems
    {
        get { return _inventoryitemdata; }
        set { _inventoryitemdata = value; customData["_inventoryitemdata"] = _inventoryitemdata; }
    }

    public string Tasks
    {
        get { return _tasks; }
        set { _tasks = value; customData["_tasks"] = _tasks; }
    }

    public string Ships
    {
        get { return _ships; }
        set { _ships = value; customData["_ships"] = _ships; }
    }

    public UserData()
    {
        _inventoryitemdata = "";
        _tasks = "";
        _ships = "";
    }
    public override void FromHashtable(Hashtable hash)
    {
        // Set User class properties
        base.FromHashtable(hash);


        if (customData.ContainsKey("_inventoryitemdata"))
            _inventoryitemdata = customData["_inventoryitemdata"].ToString();
        if (customData.ContainsKey("_tasks"))
            _tasks = customData["_tasks"].ToString();
        if (customData.ContainsKey("_ships"))
            _ships = customData["_ships"].ToString();



    }


}

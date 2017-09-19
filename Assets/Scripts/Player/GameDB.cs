using Combu;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDB : MonoBehaviour {


    private static GameDB instance;
    public static GameDB Instance() { return instance; }
    public string weaponsJSON;
    public DataObj Data;
    public List<string> weaponsidlist = new List<string>();

    public NAmeGenerator name_generator;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    [ContextMenu("Load")]
    public void Dounload()
    {
        StartCoroutine(LoadGameDataFromWWW());
    }

    IEnumerator LoadGameDataFromWWW()
    {
        
        WWW w = new WWW(CombuManager.instance.serverInfo.settings["GameData"].ToString() + "?p=" + Random.Range(1, 100000000).ToString() + "33" + Random.Range(1, 100000000).ToString() + Time.frameCount);
        yield return w;
        string gamedata = w.text;
        Debug.Log(gamedata);

        List<WeaponDate> weapons = new List<WeaponDate>();


        Data = JsonUtility.FromJson<DataObj>(gamedata);

        
    }

    public WeaponDate GetWeaponDate(string weaponid)
    {
        WeaponDate w = null;

        foreach (WeaponDate weapon in Data.Weapons)
        {
            if (weaponid == weapon.ID)
            {
                w = weapon;
            }
        }

        return w;
    }

    public void LoadData()
    {
        LoadGameDataFromWWW();
    }
    [ContextMenu("SaveData")]
    public void SaveData()
    {
        List<WeaponDate> we = new List<WeaponDate>();
        DataObj o = new DataObj(we);
        we.Add(new WeaponDate("1", 156, 18419, 20, 14896, 148, 7789, 1145, 789));
        Debug.Log(JsonUtility.ToJson(o));
    }
    private void LoadWeapons()
    {
        //string mySetting = CombuManager.instance.serverInfo.settings["small_gun_tutorial"].ToString();
        //WeaponDate data = JsonUtility.FromJson<WeaponDate>(mySetting);


        //string ID = data.ID;
        //var DIST = data.DIST;
        //var BULLETSPEED = data.BULLETSPEED;
        //var MINDAMAGE = data.MINDAMAGE;
        //var MAXDAMAGE = data.MAXDAMAGE;
        //var RELOADSPEED = data.RELOADSPEED;
        //var AMMO = data.AMMO;
        //var RPM = data.RPM;
        //var ANGULARSPEED = data.ANGULARSPEED;

        //WeaponDate weapon = new WeaponDate(ID, (float)DIST, (float)BULLETSPEED, (float)MINDAMAGE, (float)MAXDAMAGE, (float)RELOADSPEED, (float)AMMO, (float)RPM, (float)ANGULARSPEED);

        //Data = new DataObj(new List<WeaponDate>() { weapon });

    

    }

    [System.Serializable]
    public class DataObj
    {
        public List<WeaponDate> Weapons;

        public DataObj(List<WeaponDate> weapons)
        {
            Weapons = weapons;
        }
    }

    [System.Serializable]
    public class WeaponDate
    {
        public string ID;
        public float DIST;
        public float BULLETSPEED;
        public float MINDAMAGE;
        public float MAXDAMAGE;
        public float RELOADSPEED;
        public float AMMO;
        public float RPM;
        public float ANGULARSPEED;

        public WeaponDate(string iD, float dIST, float bULLETSPEED, float mINDAMAGE, float mAXDAMAGE, float rELOADSPEED, float aMMO, float rPM, float aNGULARSPEED)
        {
            ID = iD;
            DIST = dIST;
            BULLETSPEED = bULLETSPEED;
            MINDAMAGE = mINDAMAGE;
            MAXDAMAGE = mAXDAMAGE;
            RELOADSPEED = rELOADSPEED;
            AMMO = aMMO;
            RPM = rPM;
            ANGULARSPEED = aNGULARSPEED;
        }
    }
}



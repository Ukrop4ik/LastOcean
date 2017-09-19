﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour {


    public GameObject playership;
    [SerializeField]
    private List<GameObject> spawnpoints = new List<GameObject>();
    // Use this for initialization


    private static NetworkManager instance;
    public static NetworkManager Instance() { return instance; }

    private void Awake()
    {
        instance = this;
    }

    void Start () {

        //AddSpaunPoints();

        if(PhotonNetwork.offlineMode)
        {
            Transform pos = spawnpoints[Random.Range(0, spawnpoints.Count)].transform;
            GameObject ship = Instantiate(Resources.Load(Player.Instance().GetShipDecorator().GetShipId()) as GameObject, pos.position, pos.rotation);
            ship.name = PhotonNetwork.player.NickName + "_PlayerShip";
            ship.GetComponent<ShipMain>().CreateFromClient(Player.Instance().GetPlayerShipProp());
        }

    }

    public void AddSpaunPoints()
    {
        spawnpoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnPlayer"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public virtual void OnJoinedRoom()
    {

        Transform pos = spawnpoints[Random.Range(0, spawnpoints.Count)].transform;
        GameObject ship = PhotonNetwork.Instantiate(Player.Instance().GetShipDecorator().GetShipId(), pos.position , pos.rotation, 0);
        ship.name = PhotonNetwork.player.NickName + "_PlayerShip";
        ship.GetComponent<ShipMain>().CreateFromClient(Player.Instance().GetPlayerShipProp());

        Player.Instance().GetShipDecorator().DestroyDecorator();
    }

    public List<GameObject> GetPoints()
    {
        return spawnpoints;
    }


    public class ItemList
    {
        public int slot;
        public string id;

        public ItemList(int _slot, string _id)
        {
            slot = _slot;
            id = _id;
        }
    }

    [ContextMenu("OpenProfileFolder")]
    public void OpenProfileFolder()
    {
        Process.Start(Application.persistentDataPath);
    }


}

﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour {

    private Text _netStatusText;
    public GameObject playership;
    [SerializeField] List<GameObject> spawnpoints = new List<GameObject>();

	// Use this for initialization
	void Start () {

        _netStatusText = GameObject.Find("NetworkStatusText").GetComponent<Text>();

        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = Random.Range(0, 555665665).ToString();

        AddSpaunPoints();

    }

    public void AddSpaunPoints()
    {
        spawnpoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnPlayer"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        _netStatusText.text = PhotonNetwork.connectionStateDetailed.ToString();	
	}

    public virtual void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Test", new RoomOptions() { MaxPlayers = 10 }, null);
    }

    public virtual void OnJoinedRoom()
    {
        Transform pos = spawnpoints[Random.Range(0, spawnpoints.Count)].transform;
        GameObject ship = PhotonNetwork.Instantiate(Player.Instance().GetShipDecorator().GetShipId(), pos.position , pos.rotation, 0);


        ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();

        foreach (KeyValuePair<int, string> KVP in Player.Instance().GetShipDecorator().GetWeaponData())
        {  
            prop.Add(("slot_" + KVP.Key), KVP.Value);     
        }

        ship.GetComponent<ShipMain>().CreateFromServer(PhotonNetwork.player.NickName, prop);
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

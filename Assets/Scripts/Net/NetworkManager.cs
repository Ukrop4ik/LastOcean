﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour {

    private Text _netStatusText;
    public GameObject playership;
    [SerializeField] List<GameObject> spawnpoints = new List<GameObject>();

	// Use this for initialization
	void Start () {

        spawnpoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnPlayer"));

        _netStatusText = GameObject.Find("NetworkStatusText").GetComponent<Text>();

        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;

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
        PhotonNetwork.Instantiate(Player.Instance().GetShip().name, pos.position , pos.rotation, 0);
    }
}

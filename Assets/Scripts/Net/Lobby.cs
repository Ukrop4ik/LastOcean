﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Photon.MonoBehaviour {


    // Use this for initialization
    void Start () {

        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings("0.1");
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.automaticallySyncScene = true;
        }
    }

    public virtual void OnJoinedLobby()
    {
        PhotonNetwork.playerName = Player.Instance().NickName;

        Debug.Log("JoinLobby");
    }
}

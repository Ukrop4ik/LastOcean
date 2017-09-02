using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Photon.MonoBehaviour {


    // Use this for initialization
    void Start () {      

        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;
    }

    public virtual void OnJoinedLobby()
    {
        

        Debug.Log("JoinLobby");
    }
}

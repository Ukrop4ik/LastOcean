using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour {

    private Text _netStatusText;
    public GameObject playership;
    [SerializeField] Transform spawn;

	// Use this for initialization
	void Start () {

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
        PhotonNetwork.JoinOrCreateRoom("Test", new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public virtual void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playership.name, spawn.position, Quaternion.identity, 0);
    }
}

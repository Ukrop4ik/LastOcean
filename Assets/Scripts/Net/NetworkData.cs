using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkData : Photon.MonoBehaviour {

    public int sessionTime = 0;
    public int lastsessionTime = 0;
    public int sessionTimeDelta = 0;

    public bool AllReady = false;

    private static NetworkData instance;
    public static NetworkData Instance() { return instance; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lastsessionTime = PlayerPrefs.GetInt("STime");
    }


    private IEnumerator GetServerTime()
    {
        if(sessionTime != 0)
        {
            AllReady = true;
        }

        yield return new WaitForSeconds(1f);

        if(sessionTimeDelta == 0 && sessionTime != 0 && lastsessionTime != 0)
        {
            sessionTimeDelta = sessionTime - lastsessionTime;
        }

        sessionTime = (int)PhotonNetwork.time;
        StartCoroutine(GetServerTime());

    }

    [ContextMenu("DropTime")]
    public void DropTime()
    {
        PlayerPrefs.SetInt("STime", 0);
    }

    public virtual void OnConnectedToPhoton()
    {
        StartCoroutine(GetServerTime());
    }

    public virtual void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnect");
    }
    private void OnApplicationQuit()
    {
        Debug.Log("Quit");

        PlayerPrefs.SetInt("STime", (int)PhotonNetwork.time);
        PlayerPrefs.Save();
    }

}

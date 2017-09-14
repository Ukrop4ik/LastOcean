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

    public Timeline EventTimeline;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lastsessionTime = PlayerPrefs.GetInt("STime");
        EventTimeline = Timeline.CreateInstance<Timeline>();
    }

    private void TimelineRoutine()
    {

        EventTimeline.UpdateTimeLine(PhotonNetwork.time);

    }

    public int GetServerTimeInt()
    {
        return (int)PhotonNetwork.time;
    }

    [ContextMenu("AddTestEventToTimeline")]
    public void AddTestEventToTimeline()
    {
        TimelineEvent en = new TimelineEvent((int)PhotonNetwork.time, 150000, "event_get_fuel_1", true);
        PlayerDB.Instance().AddTask(en);
        EventTimeline.AddEventToTimeline(en);
    }
    [ContextMenu("AddNoMomentalEvenbt")]
    public void AddTestNoMomentalEventToTimeline()
    {
        TimelineEvent en = new TimelineEvent((int)PhotonNetwork.time, 10, "event_get_fuel_1", false);
        en.IsMomental = false;
        PlayerDB.Instance().AddTask(en);
        EventTimeline.AddEventToTimeline(en);
    }
    public void AddEventToTimeline(TimelineEvent even)
    {
        EventTimeline.AddEventToTimeline(even);
    }


    private IEnumerator GetServerTime()
    {
        if(sessionTime != 0 && !AllReady)
        {
            AllReady = true;
        }

        yield return new WaitForSeconds(1f);

        if(sessionTimeDelta == 0 && sessionTime != 0 && lastsessionTime != 0)
        {
            sessionTimeDelta = sessionTime - lastsessionTime;
        }

        TimelineRoutine();

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

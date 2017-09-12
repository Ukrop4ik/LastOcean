using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : ScriptableObject
{

    public List<TimelineEvent> TimelineEvents = new List<TimelineEvent>();


    public void UpdateTimeLine(double time)
    {

        if (time <= 0) return;

        if(TimelineEvents.Count > 0)
        {
            foreach(TimelineEvent Tevent in TimelineEvents)
            {
                Tevent.Check(time);
            }
        }
        if (TimelineEvents.Count > 0)
        {
            List<TimelineEvent> restoreevents = new List<TimelineEvent>();

            foreach (TimelineEvent Tevent in TimelineEvents)
            {
                if(!Tevent.ToRemove)
                {
                    restoreevents.Add(Tevent);               
                }
                else
                {
                    PlayerDB.Instance().RemoveTask(Tevent);
                }
            }

            TimelineEvents.Clear();
            TimelineEvents.AddRange(restoreevents);
        }

    }

    public void AddEventToTimeline(TimelineEvent timlineevent)
    {
        TimelineEvents.Add(timlineevent);
    }
}

[System.Serializable]
public class TimelineEvent
{
    public int Starttime;
    public int Endtime;
    public string EventId;
    public bool ToRemove;
    public int UnikalId;
    public bool IsMomental;

    public TimelineEvent(int start, int end, string id, int unikalId, bool isMomental)
    {
        Starttime = start;
        Endtime = end;
        EventId = id;
        ToRemove = false;
        UnikalId = unikalId;
        IsMomental = isMomental;
    }
    public TimelineEvent(int start, int end, string id, bool ismomental)
    {
        Starttime = start;
        Endtime = (end + Starttime);
        EventId = id;
        ToRemove = false;
        UnikalId = (Random.Range(0, 999999) + Random.Range(0, 999999) + Random.Range(Random.Range(0, 999999), 999999) + Endtime) * Random.Range(2, 5) * Random.Range(2, 5);
        IsMomental = ismomental;
    }

    public void Check(double time)
    {
        if(time >= Endtime)
        {
            if (IsMomental)
            {
                GameObject obj = GameObject.Find(UnikalId.ToString());
                obj.GetComponent<TimelineEventTrigger>().UseTrigger();
                Debug.Log("Event: " + EventId + " End!");
                ToRemove = true;
            }
        }
    }

}

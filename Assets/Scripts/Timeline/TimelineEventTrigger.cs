using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineEventTrigger : MonoBehaviour {

    public string Id;

    public virtual void Use() { }

    [ContextMenu("Use")]
    public void UseTrigger()
    {
        Use();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineEventEffect : TimelineEventTrigger {


    public override void Use()
    {
        PlayerDB.Instance().SetPlayerFuel(10);
    }
}

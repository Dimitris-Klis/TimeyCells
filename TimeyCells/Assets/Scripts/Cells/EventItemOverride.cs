using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EventItemOverride : EventItem
{
    public bool OverrideFavourite;

    public EventItemOverride()
    {
        ItemID = 0;
        EventName = "";
        Info1 = "";
        Info2 = "";
        EventType = -1;
        Favourite = false;
        OverrideFavourite = false;
    }
}

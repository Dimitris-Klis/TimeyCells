using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class EventItem
{
    public int ItemID;
    [Space]
    public string EventName;
    public string Info1;
    public string Info2;
    [Space]
    public int EventType;
    public bool Favourite;

    public EventItem(EventItem e)
    {
        ItemID = e.ItemID;
        EventName = e.EventName;
        Info1 = e.Info1;
        Info2 = e.Info2;
        EventType = e.EventType;
        Favourite = e.Favourite;
    }
    public EventItem()
    {
        ItemID = 0;
        EventName = "";
        Info1 = "";
        Info2 = "";
        EventType = 0;
        Favourite = false;
    }
}
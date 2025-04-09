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
}
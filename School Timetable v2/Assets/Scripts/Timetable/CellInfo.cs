using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class CellInfo : MonoBehaviour
{
    public TimetableCell cellUI;

    public int SelectedEvent;
    public EventItem Override;
    [Space]
    public int TemporaryEvent;
    public EventItem TemporaryOverride;
    //[Space]
    //public DateTime ExpirationDate;

    [ContextMenu("Test DateTime")]
    public void Test()
    {
        DateTime now = DateTime.Now;
        string nowstring = now.ToString();

        Debug.Log(now);
        Debug.Log(nowstring);
        Debug.Log(DateTime.Parse(nowstring));
    }
}
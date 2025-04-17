using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class CellInfo : MonoBehaviour
{
    public TimetableCell cellUI;

    public int SelectedEvent;
    public bool ShouldOverride;
    public EventItem Override;
    [Space]
    public int TemporaryEvent;
    public EventItem TemporaryOverride;
    public int weeks;
    //[Space]
    public DateTime StartDate;

    public void SetSelfToSelectedEvent()
    {
        SelectedEvent = TimetableEditor.instance.SelectedID;
        UpdateUI();
    }
    public void UpdateUI()
    {
        EventItem e = EventManager.Instance.GetEvent(SelectedEvent);
        if (ShouldOverride)
        {
            if(Override.EventName != "") e.EventName = Override.EventName;
            if (Override.Info1 != "") e.Info1 = Override.Info1;
            if (Override.Info2 != "") e.Info2 = Override.Info2;

            // Add an extra dropdown option to the cellinfo editor, called "Don't Override".
            if (Override.EventType >= 0) e.EventType = Override.EventType;
        }

        // Actual UI Changes here
        cellUI.EventNameText.text = e.EventName;
        cellUI.Info1Text.text = e.Info1;
        cellUI.Info2Text.text = e.Info2;

        cellUI.FavouriteImage.gameObject.SetActive(e.Favourite);

        EventTypeItem et = EventManager.Instance.GetEventType(e.EventType);
        cellUI.EventNameText.color = cellUI.Info1Text.color = cellUI.Info2Text.color = et.TextColor;
        cellUI.BackgroundImage.color = et.BackgroundColor;
    }

    [ContextMenu("Test DateTime")]
    public void Test()
    {
        DateTime now = DateTime.Now;
        string nowstring = now.ToString();

        int currDay = (int)now.DayOfWeek;
        Debug.Log(currDay);
        //Debug.Log(now);
        //Debug.Log(nowstring);
        //Debug.Log(DateTime.Parse(nowstring));
    }
}
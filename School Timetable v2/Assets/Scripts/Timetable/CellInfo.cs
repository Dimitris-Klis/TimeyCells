using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class CellInfo : MonoBehaviour
{
    public TimetableCell cellUI;

    public int SelectedEventBase = 0;
    public EventItemOverride Override = new();
    [Space]
    public int TemporaryBase;
    public EventItemOverride TemporaryOverride = new();
    public DateTime OverrideDate = DateTime.Now;
    public int ExpirationLength = 0;
    public int WeeksDelay = 0;
    public int WeeksLifetime = -1;
    //[Space]
    public bool OverrideCommonLength;
    public bool TempOverrideCommonLength;
    public TimeSpan Length;
    public TimeSpan TempLength;

    public void SetSelfToSelectedEvent()
    {
        SelectedEventBase = TimetableEditor.instance.SelectedID;
        UpdateUI();
    }
    public void UpdateUI()
    {
        EventItem e = new(EventManager.Instance.GetEvent(SelectedEventBase));
        if (Override.EventName != "") e.EventName = Override.EventName;
        if (Override.Info1 != "") e.Info1 = Override.Info1;
        if (Override.Info2 != "") e.Info2 = Override.Info2;

        // Add an extra dropdown option to the cellinfo editor, called "Don't Override".
        if (Override.EventType >= 0) e.EventType = Override.EventType;
        if (Override.OverrideFavourite) e.Favourite = Override.Favourite;

        // Actual UI Changes here
        cellUI.EventNameText.text = e.EventName;
        cellUI.Info1Text.text = e.Info1;
        cellUI.Info2Text.text = e.Info2;

        cellUI.FavouriteImage.gameObject.SetActive(e.Favourite);

        EventTypeItem et = EventManager.Instance.GetEventType(e.EventType);
        cellUI.EventNameText.color = cellUI.Info1Text.color = cellUI.Info2Text.color = et.TextColor;
        cellUI.BackgroundImage.color = et.BackgroundColor;

        CheckIfTempExpired();

        if (WeeksLifetime >= 0)
        {
            if (DateTime.Now >= OverrideDate.AddDays(WeeksDelay * 7))
            {
                EventItem temp_e = new(EventManager.Instance.GetEvent(TemporaryBase));
                if (TemporaryOverride.EventName != "") temp_e.EventName = TemporaryOverride.EventName;
                else if (Override.EventName != "") temp_e.EventName = Override.EventName;

                if (TemporaryOverride.Info1 != "") temp_e.Info1 = TemporaryOverride.Info1;
                else if (Override.Info1 != "") temp_e.Info1 = Override.Info1;

                if (TemporaryOverride.Info2 != "") temp_e.Info2 = TemporaryOverride.Info2;
                else if (Override.Info2 != "") temp_e.Info2 = Override.Info2;

                if (TemporaryOverride.EventType >= 0) temp_e.EventType = TemporaryOverride.EventType;
                if (TemporaryOverride.OverrideFavourite) temp_e.Favourite = TemporaryOverride.Favourite;

                cellUI.EventNameText.text = temp_e.EventName;
                cellUI.Info1Text.text = temp_e.Info1;
                cellUI.Info2Text.text = temp_e.Info2;
                
                EventTypeItem temp_et = EventManager.Instance.GetEventType(temp_e.EventType);
                if(temp_et.ItemID >= 0)
                {
                    cellUI.EventNameText.color = cellUI.Info1Text.color = cellUI.Info2Text.color = temp_et.TextColor;
                    cellUI.BackgroundImage.color = temp_et.BackgroundColor;
                }
                
                cellUI.FavouriteImage.gameObject.SetActive(temp_e.Favourite);
            }
        }
    }
    public void CheckIfTempExpired()
    {
        if(WeeksLifetime >= 0 && DateTime.Now > OverrideDate.AddDays(WeeksDelay * 7 + WeeksLifetime * 7 + ExpirationLength).AddHours(23).AddMinutes(59))
        {
            WeeksLifetime = -1;
            UpdateUI();
        }
    }
}
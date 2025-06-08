using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class CellInfo : MonoBehaviour
{
    public TimetableCell CellUI;

    public int SelectedEventBase = 0;
    public EventItemOverride Override = new();
    [Space]
    public int TemporaryBase;
    public EventItemOverride TemporaryOverride = new();
    public DateTime OverrideDate = DateTime.Now;
    public int OverrideLength = 0;
    public int OverrideDelayWeeks = 0;
    public int OverrideExtraLengthWeeks = -1;
    //[Space]
    public bool OverrideCommonLength;
    public bool TempOverrideCommonLength;
    public TimeSpan NewLength;
    public TimeSpan TempNewLength;

    public void SetupSelf(TimetableData.CellInfoData data)
    {
        SelectedEventBase = data.SelectedEvent;

        Override = new();
        TemporaryOverride = new();

        Override.EventName = data.EventNameOverride;
        Override.Info1 = data.Info1Override;
        Override.Info2 = data.Info2Override;
        Override.EventType = data.EventTypeOverride;
        Override.OverrideFavourite = data.OverrideFavourite > 0;
        Override.Favourite = data.OverrideFavourite > 1;

        OverrideCommonLength = data.OverrideCommonLength;
        NewLength = new(data.NewLength[0], data.NewLength[1], 0);

        if (data.ExtraOverrideLengthWeeks >= 0)
        {
            // Expiration Date Stuff
            OverrideDate = new(data.OverrideDate[0], data.OverrideDate[1], data.OverrideDate[2]);

            OverrideLength = data.OverrideLength;
            OverrideDelayWeeks = data.OverrideDelayWeeks;

            // Temporary Overrides
            TemporaryBase = data.TempSelectedEvent;

            TemporaryOverride.EventName = data.TempEventNameOverride;
            TemporaryOverride.Info1 = data.TempInfo1Override;
            TemporaryOverride.Info2 = data.TempInfo2Override;
            TemporaryOverride.EventType = data.TempEventTypeOverride;

            TemporaryOverride.OverrideFavourite = data.TempOverrideFavourite > 0;
            TemporaryOverride.Favourite = data.TempOverrideFavourite > 1;

            TempOverrideCommonLength = data.TempOverrideCommonLength;
            TempNewLength = new(data.TempNewLength[0], data.TempNewLength[1], 0);
        }
    }

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
        CellUI.EventNameText.text = e.EventName;
        CellUI.Info1Text.text = e.Info1;
        CellUI.Info2Text.text = e.Info2;

        CellUI.FavouriteImage.gameObject.SetActive(e.Favourite);

        EventTypeItem et = EventManager.Instance.GetEventType(e.EventType);
        CellUI.EventNameText.color = CellUI.Info1Text.color = CellUI.Info2Text.color = et.TextColor;
        CellUI.BackgroundImage.color = et.BackgroundColor;

        CheckIfTempExpired();

        if (OverrideExtraLengthWeeks >= 0)
        {
            if (DateTime.Now >= OverrideDate.AddDays(OverrideDelayWeeks * 7))
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

                CellUI.EventNameText.text = temp_e.EventName;
                CellUI.Info1Text.text = temp_e.Info1;
                CellUI.Info2Text.text = temp_e.Info2;
                
                EventTypeItem temp_et = EventManager.Instance.GetEventType(temp_e.EventType);
                if(temp_et.ItemID >= 0)
                {
                    CellUI.EventNameText.color = CellUI.Info1Text.color = CellUI.Info2Text.color = temp_et.TextColor;
                    CellUI.BackgroundImage.color = temp_et.BackgroundColor;
                }
                
                CellUI.FavouriteImage.gameObject.SetActive(temp_e.Favourite);
            }
        }
    }
    public void CheckIfTempExpired()
    {
        if(OverrideExtraLengthWeeks >= 0 && DateTime.Now > OverrideDate.AddDays(OverrideDelayWeeks * 7 + OverrideExtraLengthWeeks * 7 + OverrideLength).AddHours(23).AddMinutes(59))
        {
            OverrideExtraLengthWeeks = -1;
            UpdateUI();
        }
    }
}
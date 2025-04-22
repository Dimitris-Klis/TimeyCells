using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System;

public class CellInfoEditor : MonoBehaviour
{
    int SelectedCellColumn=-1;
    int SelectedCellRow=-1;
    int originalEvent;
    public TimetableCell MainPreview;
    public TimetableCell BasePreview;
    [Space]
    public TMP_InputField EventNameOverride;
    public TMP_InputField Info1Override;
    public TMP_InputField Info2Override;
    public TMP_Dropdown TypeOverride;
    public TMP_Dropdown FavouriteOverride;
    [Space]
    public Toggle OverrideTimeToggle;
    public TMP_InputField StartTimeInput;
    public TMP_InputField LengthInput;

    public void SelectCell(int column, int row)
    {
        SelectedCellColumn = column;
        SelectedCellRow = row;

        CellInfo selectedInfo = TimetableEditor.instance.Grid.ColumnsList[column].Children[row].Info;

        originalEvent = selectedInfo.SelectedEvent;
        //EventManager.Instance.ZoomHandler.enabled = false;

        //Setting up the dropdown.
        TypeOverride.ClearOptions();
        TMP_Dropdown.OptionData[] dropdownOptions = new TMP_Dropdown.OptionData[EventManager.Instance.EventTypes.Count+1];
        dropdownOptions[0] = new("Don't Override");
        for (int i = 1; i < dropdownOptions.Length; i++)
        {
            dropdownOptions[i] = new(EventManager.Instance.EventTypes[i-1].TypeName);
        }
        TypeOverride.AddOptions(dropdownOptions.ToList());

        TypeOverride.value = selectedInfo.Override.EventType + 1;
        EventNameOverride.text = selectedInfo.Override.EventName;
        Info1Override.text = selectedInfo.Override.Info1;
        Info2Override.text = selectedInfo.Override.Info2;

        FavouriteOverride.value = (selectedInfo.Override.OverrideFavourite ? 1 : 0) * (1 + (selectedInfo.Override.Favourite ? 1 : 0));

        if (selectedInfo.OverrideCommonLength)
        {

            LengthInput.text = DayTimeManager.instance.FormatLength(selectedInfo.Length);
        }
        else
        {
            TimeSpan commonLen = DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);
            LengthInput.text = DayTimeManager.instance.FormatLength(commonLen);
        }
        

        TimeSpan t = DayTimeManager.instance.GetCellStartTime(SelectedCellColumn, SelectedCellRow);
        StartTimeInput.text = DayTimeManager.instance.FormatTime(t);

        StartTimeInput.interactable = !selectedInfo.cellUI.isbreak;
        LengthInput.interactable = OverrideTimeToggle.isOn = selectedInfo.OverrideCommonLength;

        UpdatePreviews();
    }
    public CellInfo GetSelectedInfo()
    {
        return TimetableEditor.instance.Grid.ColumnsList[SelectedCellColumn].Children[SelectedCellRow].Info; 
    }
    public void ToggleOverrideTime(bool overridetime)
    {
        LengthInput.interactable = overridetime;
    }
    public void ParseLength(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if(!DayTimeManager.TryParseLength(text, out DateTime length))
        {
            TimeSpan commonLen = DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);
            LengthInput.text = DayTimeManager.instance.FormatLength(commonLen);
        }
    }
    public void ParseStartTime(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (!DayTimeManager.TryParseTime(text, out DateTime length) ||
            SelectedCellColumn > 0 && DayTimeManager.instance.TimeDiff(length.TimeOfDay, SelectedCellColumn - 1, SelectedCellRow) < TimeSpan.Zero)
        {
            TimeSpan t = DayTimeManager.instance.GetCellStartTime(SelectedCellColumn, SelectedCellRow);
            StartTimeInput.text = DayTimeManager.instance.FormatTime(t);
        }
    }
    
    // This should be used by the 'Event Selector' overlay.
    public void ChangeInfoBase(int EventID)
    {
        CellInfo c = GetSelectedInfo();
        c.SelectedEvent = EventID;
        c.UpdateUI();
        TimetableEditor.instance.EventSelectorOverlay.SetActive(false);

        UpdatePreviews();
    }
    public void UpdatePreviews()
    {
        CellInfo c = GetSelectedInfo();
        EventItem e = EventManager.Instance.GetEvent(c.SelectedEvent);
        EventTypeItem et = EventManager.Instance.GetEventType(e.EventType);

        BasePreview.EventNameText.text = MainPreview.EventNameText.text = e.EventName;
        BasePreview.Info1Text.text = MainPreview.Info1Text.text = e.Info1;
        BasePreview.Info2Text.text = MainPreview.Info2Text.text = e.Info2;

        if (e.ItemID == 0) BasePreview.EventNameText.text = "None";

        BasePreview.BackgroundImage.color = MainPreview.BackgroundImage.color = et.BackgroundColor;

        BasePreview.EventNameText.color = MainPreview.EventNameText.color = et.TextColor;
        BasePreview.Info1Text.color = MainPreview.Info1Text.color = et.TextColor;
        BasePreview.Info2Text.color = MainPreview.Info2Text.color = et.TextColor;

        BasePreview.FavouriteImage.gameObject.SetActive(e.Favourite);
        MainPreview.FavouriteImage.gameObject.SetActive(e.Favourite);

        // Override

        if (!isNothing(EventNameOverride.text))
            MainPreview.EventNameText.text = EventNameOverride.text;

        if (!isNothing(Info1Override.text))
            MainPreview.Info1Text.text = Info1Override.text;

        if (!isNothing(Info2Override.text))
            MainPreview.Info2Text.text = Info2Override.text;

        if(TypeOverride.value-1 >= 0)
        {
            EventTypeItem etOverride = EventManager.Instance.EventTypes[TypeOverride.value-1];
            MainPreview.BackgroundImage.color = etOverride.BackgroundColor;

            MainPreview.EventNameText.color = etOverride.TextColor;
            MainPreview.Info1Text.color = etOverride.TextColor;
            MainPreview.Info2Text.color = etOverride.TextColor;
        }

        if(FavouriteOverride.value > 0)
        {
            MainPreview.FavouriteImage.gameObject.SetActive(FavouriteOverride.value > 1);
        }
    }

    bool isNothing(string text)
    {
        return text.Replace(TMP_Specials.clear, "") == "";
    }

    public void Cancel()
    {
        ChangeInfoBase(originalEvent);
        //EventManager.Instance.ZoomHandler.enabled = true;
        gameObject.SetActive(false);
    }
    public void Save()
    {
        //EventManager.Instance.ZoomHandler.enabled = true;
        CellInfo c = GetSelectedInfo();
        //TO DO: Make special override type of EventItem
        c.Override.EventName = EventNameOverride.text.Replace(TMP_Specials.clear, "");
        c.Override.Info1 = Info1Override.text.Replace(TMP_Specials.clear, "");
        c.Override.Info2 = Info2Override.text.Replace(TMP_Specials.clear, "");
        c.Override.EventType = TypeOverride.value - 1;
        c.Override.OverrideFavourite = FavouriteOverride.value > 0;
        c.Override.Favourite = FavouriteOverride.value > 1;

        c.OverrideCommonLength = OverrideTimeToggle.isOn;
        
        if (DayTimeManager.TryParseLength(LengthInput.text.Replace(TMP_Specials.clear, ""), out DateTime len))
        {
            c.Length = len.TimeOfDay;
        }

        if (DayTimeManager.TryParseTime(StartTimeInput.text.Replace(TMP_Specials.clear, ""), out DateTime start))
        {
            if (SelectedCellColumn == 0)
            {
                DayTimeManager.instance.WeekDays[SelectedCellRow].StartTime = start.TimeOfDay;
            }
            else if (!c.cellUI.isbreak)
            {
                CellInfo PrevInfo = TimetableEditor.instance.Grid.ColumnsList[SelectedCellColumn - 1].Children[SelectedCellRow].Info;

                long ticks = DayTimeManager.instance.TimeDiff(start.TimeOfDay, SelectedCellColumn - 1, SelectedCellRow).Ticks;

                // Instead of using Mathf.Abs, we manually make the value absolute to avoid losing any bytes, since this is a long.
                if (ticks < 0) ticks = -ticks;

                PrevInfo.Length = new TimeSpan(ticks);

                PrevInfo.OverrideCommonLength = PrevInfo.Length != DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);
            }
        }

        DayTimeManager.instance.UpdateTimeIndexes();
        c.UpdateUI();

        SelectedCellColumn = -1;
        SelectedCellRow = -1;

        gameObject.SetActive(false);
    }
}
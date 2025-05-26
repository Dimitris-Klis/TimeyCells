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
    public TMP_InputField LengthInputHours;
    public TMP_InputField LengthInputMinutes;

    [Space(30)]
    [Header("Temporary Overriding")]
    public TabHandler tabs;
    public GameObject CreateButton;
    public GameObject DeleteButton;
    public GameObject ErrorText;
    [Space]
    public GameObject TempPropertiesLayout;
    public GameObject TempPromptLayout;
    public TimetableCell TempBasePreview;
    [Space(20)]
    // MaxDelay and length should be 52.
    public Slider DelaySlider;
    public Slider LengthSlider;
    public TMP_InputField DelayInput;
    public TMP_InputField LengthInput;
    [Space]
    public TMP_InputField TempEventNameOverride;
    public TMP_InputField TempInfo1Override;
    public TMP_InputField TempInfo2Override;
    public TMP_Dropdown TempTypeOverride;
    public TMP_Dropdown TempFavouriteOverride;
    [Space]
    public Toggle TempOverrideTimeToggle;
    public TMP_InputField TempStartTimeInput;
    public TMP_InputField TempLengthInputHours;
    public TMP_InputField TempLengthInputMinutes;

    DateTime OverrideDate;

    public void SelectCell(int column, int row)
    {
        
        SelectedCellColumn = column;
        SelectedCellRow = row;

        CellInfo selectedInfo = TimetableEditor.instance.Grid.ColumnsList[column].Children[row].Info;
        if (selectedInfo.WeeksLifetime < 0)
        {
            tabs.SelectTab(0);
            TempPropertiesLayout.SetActive(false);
            TempPromptLayout.SetActive(true);
        }
        else
        {
            tabs.SelectTab(1);
            TempPropertiesLayout.SetActive(true);
            TempPromptLayout.SetActive(false);
        }
        originalEvent = selectedInfo.SelectedEventBase;
        //EventManager.Instance.ZoomHandler.enabled = false;

        //Setting up the dropdown.
        TypeOverride.ClearOptions();
        TempTypeOverride.ClearOptions();
        TMP_Dropdown.OptionData[] dropdownOptions = new TMP_Dropdown.OptionData[EventManager.Instance.EventTypes.Count + 1];
        dropdownOptions[0] = new("Don't Override");
        for (int i = 1; i < dropdownOptions.Length; i++)
        {
            dropdownOptions[i] = new(EventManager.Instance.EventTypes[i - 1].TypeName);
        }
        TypeOverride.AddOptions(dropdownOptions.ToList());
        TempTypeOverride.AddOptions(dropdownOptions.ToList());

        TypeOverride.value = selectedInfo.Override.EventType + 1;
        EventNameOverride.text = selectedInfo.Override.EventName;
        Info1Override.text = selectedInfo.Override.Info1;
        Info2Override.text = selectedInfo.Override.Info2;

        FavouriteOverride.value = (selectedInfo.Override.OverrideFavourite ? 1 : 0) * (1 + (selectedInfo.Override.Favourite ? 1 : 0));

        if (selectedInfo.OverrideCommonLength)
        {
            LengthInputHours.text = selectedInfo.Length.Hours.ToString();
            LengthInputMinutes.text = selectedInfo.Length.Minutes.ToString();
        }
        else
        {
            TimeSpan commonLen = DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);

            LengthInputHours.text = commonLen.Hours.ToString();
            LengthInputMinutes.text = commonLen.Minutes.ToString();
        }

        if (selectedInfo.WeeksLifetime > -1)
        {
            TempTypeOverride.SetValueWithoutNotify(selectedInfo.TemporaryOverride.EventType + 1);
            TempEventNameOverride.SetTextWithoutNotify(selectedInfo.TemporaryOverride.EventName);
            TempInfo1Override.SetTextWithoutNotify(selectedInfo.TemporaryOverride.Info1);
            TempInfo2Override.SetTextWithoutNotify(selectedInfo.TemporaryOverride.Info2);

            TempFavouriteOverride.value = (selectedInfo.TemporaryOverride.OverrideFavourite ? 1 : 0) * (1 + (selectedInfo.TemporaryOverride.Favourite ? 1 : 0));


            DelaySlider.SetValueWithoutNotify(selectedInfo.WeeksDelay);
            LengthSlider.SetValueWithoutNotify(selectedInfo.WeeksLifetime);

            DelayInput.SetTextWithoutNotify(selectedInfo.WeeksDelay.ToString() + TMP_Specials.clear);
            LengthInput.SetTextWithoutNotify(selectedInfo.WeeksLifetime.ToString() + TMP_Specials.clear);

            if (selectedInfo.TempOverrideCommonLength)
            {
                TempLengthInputHours.text = selectedInfo.TempLength.Hours.ToString();
                TempLengthInputMinutes.text = selectedInfo.TempLength.Minutes.ToString();
            }
            else
            {
                if (selectedInfo.OverrideCommonLength)
                {
                    TempLengthInputHours.text = selectedInfo.Length.Hours.ToString();
                    TempLengthInputMinutes.text = selectedInfo.Length.Minutes.ToString();
                }
                else
                {
                    TimeSpan commonLen = DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);

                    TempLengthInputHours.text = commonLen.Hours.ToString();
                    TempLengthInputMinutes.text = commonLen.Minutes.ToString();
                }
            }
        }

        TimeSpan t = DayTimeManager.instance.GetCellStartTime(SelectedCellColumn, SelectedCellRow);
        StartTimeInput.text = DayTimeManager.instance.FormatTime(t);

        StartTimeInput.interactable = !selectedInfo.cellUI.isbreak;

        LengthInputHours.interactable = OverrideTimeToggle.isOn = selectedInfo.OverrideCommonLength;
        LengthInputMinutes.interactable = OverrideTimeToggle.isOn = selectedInfo.OverrideCommonLength;

        TempLengthInputHours.interactable = OverrideTimeToggle.isOn = selectedInfo.OverrideCommonLength;
        TempLengthInputMinutes.interactable = OverrideTimeToggle.isOn = selectedInfo.OverrideCommonLength;

        CreateButton.SetActive(DayTimeManager.instance.WeekDays[SelectedCellColumn].Days != 0);
        ErrorText.SetActive(DayTimeManager.instance.WeekDays[SelectedCellColumn].Days == 0);

        UpdatePreviews();
    }
    public void CreateTempOverride()
    {
        // TO DO: WHEN WE CHANGE A WEEKDAY'S DAYS, WE SHOULD DELETE THE TEMP OVERRIDE TO PREVENT ANY WEIRD BEHAVIOUR.
        TempPromptLayout.SetActive(false);
        TempPropertiesLayout.SetActive(true);
        DeleteButton.SetActive(true);

        OverrideDate = DateTime.Today;
        DelaySlider.SetValueWithoutNotify(0);
        LengthSlider.SetValueWithoutNotify(0);
        DelayInput.text = DelaySlider.value.ToString();
        LengthInput.text = LengthSlider.value.ToString();

        TempLengthInputHours.text = "";
        TempLengthInputMinutes.text = "";
        TempTypeOverride.SetValueWithoutNotify(0);
        TempEventNameOverride.SetTextWithoutNotify("");
        TempInfo1Override.SetTextWithoutNotify("");
        TempInfo2Override.SetTextWithoutNotify("");
        TempFavouriteOverride.value = 0;

        TempOverrideTimeToggle.isOn = false;
    }
    public void DeleteTempOverride()
    {
        TempPromptLayout.SetActive(true);
        TempPropertiesLayout.SetActive(false);
        DeleteButton.SetActive(false);
    }
    public CellInfo GetSelectedInfo()
    {
        return TimetableEditor.instance.Grid.ColumnsList[SelectedCellColumn].Children[SelectedCellRow].Info; 
    }
    public void ToggleOverrideTime(bool overridetime)
    {
        if (!TempPropertiesLayout.activeInHierarchy)
        {
            LengthInputHours.interactable = OverrideTimeToggle.isOn = overridetime;
            LengthInputMinutes.interactable = OverrideTimeToggle.isOn = overridetime;
        }
        else
        {
            TempLengthInputHours.interactable = TempOverrideTimeToggle.isOn = overridetime;
            TempLengthInputMinutes.interactable = TempOverrideTimeToggle.isOn = overridetime;
        }
    }
    public void ParseMinutes(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (int.TryParse(text, out int length))
        {
            if (length > 59) length = 59;
            if (!TempPropertiesLayout.activeInHierarchy)
                LengthInputMinutes.text = length.ToString();
            else
                TempLengthInputMinutes.text = length.ToString();
        }
        else
        {
            TimeSpan commonLen = DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);

            if (!TempPropertiesLayout.activeInHierarchy)
                LengthInputMinutes.text = commonLen.Minutes.ToString();
            else
                TempLengthInputMinutes.text = commonLen.Minutes.ToString();
        }
    }
    public void ParseHours(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (int.TryParse(text, out int length))
        {
            if (length > 23) length = 23; // No event will last 24 hours!
            if (!TempPropertiesLayout.activeInHierarchy)
                LengthInputHours.text = length.ToString();
            else
                TempLengthInputHours.text = length.ToString();
        }
        else
        {
            TimeSpan commonLen = DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);

            if (!TempPropertiesLayout.activeInHierarchy)
                LengthInputHours.text = commonLen.Hours.ToString();
            else
                TempLengthInputHours.text = commonLen.Hours.ToString();
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
    
    // This is called by the 'Event Selector' overlay.
    public void ChangeInfoBase(int EventID)
    {
        CellInfo c = GetSelectedInfo();
        if (!TempPropertiesLayout.activeInHierarchy)
            c.SelectedEventBase = EventID;
        else
            c.TemporaryBase = EventID;
        c.UpdateUI();
        TimetableEditor.instance.EventSelectorOverlay.SetActive(false);

        UpdatePreviews();
    }
    public void UpdatePreviews()
    {
        CellInfo c = GetSelectedInfo();
        EventItem e = EventManager.Instance.GetEvent(c.SelectedEventBase);
        EventTypeItem et = EventManager.Instance.GetEventType(e.EventType);

        EventItem temp_e = EventManager.Instance.GetEvent(c.TemporaryBase);
        EventTypeItem temp_et = EventManager.Instance.GetEventType(temp_e.EventType);

        BasePreview.EventNameText.text =  e.EventName;
        BasePreview.Info1Text.text = e.Info1;
        BasePreview.Info2Text.text = e.Info2;

        if (e.ItemID == 0) BasePreview.EventNameText.text = "None";

        BasePreview.BackgroundImage.color = et.BackgroundColor;

        BasePreview.EventNameText.color = et.TextColor;
        BasePreview.Info1Text.color = et.TextColor;
        BasePreview.Info2Text.color = et.TextColor;

        BasePreview.FavouriteImage.gameObject.SetActive(e.Favourite);

        // Temp Override
        TempBasePreview.EventNameText.text = MainPreview.EventNameText.text = e.EventName;
        TempBasePreview.Info1Text.text = MainPreview.Info1Text.text = e.Info1;
        TempBasePreview.Info2Text.text = MainPreview.Info2Text.text = e.Info2;

        if (temp_e.ItemID == 0) TempBasePreview.EventNameText.text = "None";

        TempBasePreview.BackgroundImage.color = et.BackgroundColor;

        TempBasePreview.EventNameText.color = et.TextColor;
        TempBasePreview.Info1Text.color = et.TextColor;
        TempBasePreview.Info2Text.color = et.TextColor;

        TempBasePreview.FavouriteImage.gameObject.SetActive(e.Favourite);


        // Main Preview
        if (!TempPropertiesLayout.activeInHierarchy)
        {
            MainPreview.EventNameText.text = e.EventName;
            MainPreview.Info1Text.text = e.Info1;
            MainPreview.Info2Text.text = e.Info2;

            MainPreview.BackgroundImage.color = et.BackgroundColor;
            MainPreview.EventNameText.color = et.TextColor;
            MainPreview.Info1Text.color = et.TextColor;
            MainPreview.Info2Text.color = et.TextColor;

            MainPreview.FavouriteImage.gameObject.SetActive(e.Favourite);

            if (!isNothing(EventNameOverride.text))
                MainPreview.EventNameText.text = EventNameOverride.text;

            if (!isNothing(Info1Override.text))
                MainPreview.Info1Text.text = Info1Override.text;

            if (!isNothing(Info2Override.text))
                MainPreview.Info2Text.text = Info2Override.text;

            if (TypeOverride.value - 1 >= 0)
            {
                EventTypeItem etOverride = EventManager.Instance.EventTypes[TypeOverride.value - 1];
                MainPreview.BackgroundImage.color = etOverride.BackgroundColor;

                MainPreview.EventNameText.color = etOverride.TextColor;
                MainPreview.Info1Text.color = etOverride.TextColor;
                MainPreview.Info2Text.color = etOverride.TextColor;
            }

            if (FavouriteOverride.value > 0)
            {
                MainPreview.FavouriteImage.gameObject.SetActive(FavouriteOverride.value > 1);
            }
        }
        else
        {
            MainPreview.EventNameText.text = temp_e.EventName;
            MainPreview.Info1Text.text = temp_e.Info1;
            MainPreview.Info2Text.text = temp_e.Info2;

            MainPreview.BackgroundImage.color = temp_et.BackgroundColor;
            MainPreview.EventNameText.color = temp_et.TextColor;
            MainPreview.Info1Text.color = temp_et.TextColor;
            MainPreview.Info2Text.color = temp_et.TextColor;
            if(temp_e.ItemID != 0)
                MainPreview.FavouriteImage.gameObject.SetActive(temp_e.Favourite);
            else
                MainPreview.FavouriteImage.gameObject.SetActive(e.Favourite);

            if (!isNothing(TempEventNameOverride.text))
            {
                MainPreview.EventNameText.text = TempEventNameOverride.text;
            }
            else
            {
                if (!isNothing(EventNameOverride.text))
                    MainPreview.EventNameText.text = EventNameOverride.text;
            }

            if (!isNothing(TempInfo1Override.text))
            {
                    MainPreview.Info1Text.text = TempInfo1Override.text;
            }
            else
            {
                if (!isNothing(Info1Override.text))
                    MainPreview.Info1Text.text = Info1Override.text;
            }   

            if (!isNothing(TempInfo2Override.text))
            {
                MainPreview.Info2Text.text = TempInfo2Override.text;
            }
            else
            {
                if (!isNothing(Info2Override.text))
                    MainPreview.Info2Text.text = Info2Override.text;
            }

            if (TempTypeOverride.value - 1 >= 0)
            {
                EventTypeItem etOverride = EventManager.Instance.EventTypes[TypeOverride.value - 1];
                MainPreview.BackgroundImage.color = etOverride.BackgroundColor;

                MainPreview.EventNameText.color = etOverride.TextColor;
                MainPreview.Info1Text.color = etOverride.TextColor;
                MainPreview.Info2Text.color = etOverride.TextColor;
            }
            else
            {
                if (TypeOverride.value - 1 >= 0)
                {
                    EventTypeItem etOverride = EventManager.Instance.EventTypes[TypeOverride.value - 1];
                    MainPreview.BackgroundImage.color = etOverride.BackgroundColor;

                    MainPreview.EventNameText.color = etOverride.TextColor;
                    MainPreview.Info1Text.color = etOverride.TextColor;
                    MainPreview.Info2Text.color = etOverride.TextColor;
                }
            }

            if (TempFavouriteOverride.value > 0)
            {
                MainPreview.FavouriteImage.gameObject.SetActive(TempFavouriteOverride.value > 1);
            }
            else
            {
                if (FavouriteOverride.value > 0)
                {
                    MainPreview.FavouriteImage.gameObject.SetActive(FavouriteOverride.value > 1);
                }
            }
        }
    }

    bool isNothing(string text)
    {
        return text.Replace(TMP_Specials.clear, "") == "";
    }

    public void SetDelay(float Delay)
    {
        DelayInput.text = Delay.ToString();
    }
    public void SetLength(float Length)
    {
        LengthInput.text = Length.ToString();
    }

    public void Cancel()
    {
        ChangeInfoBase(originalEvent);
        //EventManager.Instance.ZoomHandler.enabled = true;
        gameObject.SetActive(false);
    }
    public void Confirm()
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

        string lentext = LengthInputHours.text.Replace(TMP_Specials.clear, "") + ":" + LengthInputMinutes.text.Replace(TMP_Specials.clear, "");
        if (DayTimeManager.TryParseLength(lentext, out DateTime len))
        {
            c.Length = len.TimeOfDay;
        }

        if (DayTimeManager.TryParseTime(StartTimeInput.text.Replace(TMP_Specials.clear, ""), out DateTime start))
        {
            if (SelectedCellColumn == 0)
            {
                DayTimeManager.instance.WeekDays[SelectedCellRow].StartTime = start.TimeOfDay;
            }
            else if (!c.cellUI.isbreak) // If it's a colspan, reducing all of the cells' lengths is probably not something you want to do.
            {
                int childIndex = TimetableEditor.instance.Grid.ColumnsList[SelectedCellColumn - 1].isBreak ? 0 : SelectedCellRow;
                CellInfo PrevInfo = TimetableEditor.instance.Grid.ColumnsList[SelectedCellColumn - 1].Children[childIndex].Info;

                long ticks = DayTimeManager.instance.TimeDiff(start.TimeOfDay, SelectedCellColumn - 1, SelectedCellRow).Ticks;

                // Instead of using Mathf.Abs, we manually make the value absolute to avoid losing any bits, since this is a long.
                if (ticks < 0) ticks = -ticks;

                PrevInfo.Length = new TimeSpan(ticks);

                PrevInfo.OverrideCommonLength = PrevInfo.Length != DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);
            }
        }

        if (TempPropertiesLayout.activeSelf)
        {
            c.TemporaryOverride.EventName = TempEventNameOverride.text.Replace(TMP_Specials.clear, "");
            c.TemporaryOverride.Info1 = TempInfo1Override.text.Replace(TMP_Specials.clear, "");
            c.TemporaryOverride.Info2 = TempInfo2Override.text.Replace(TMP_Specials.clear, "");
            c.TemporaryOverride.EventType = TempTypeOverride.value - 1;
            c.TemporaryOverride.OverrideFavourite = TempFavouriteOverride.value > 0;
            c.TemporaryOverride.Favourite = TempFavouriteOverride.value > 1;

            c.TempOverrideCommonLength = TempOverrideTimeToggle.isOn;

            c.OverrideDate = OverrideDate;
            if (int.TryParse(DelayInput.text.Replace(TMP_Specials.clear, ""), out int delay))
                c.WeeksDelay = delay;
            else
                c.WeeksDelay = 0;


            if (int.TryParse(LengthInput.text.Replace(TMP_Specials.clear, ""), out int length))
                c.WeeksLifetime = length;
            else
                c.WeeksLifetime = 0;


            WeekDay wd = DayTimeManager.instance.WeekDays[SelectedCellRow];
            int dayOfWeek=6; // The cell info's day of week
            for (int i = 64; i > 1; i/=2)
            {
                if (wd.Days / i % 2 == 1) break;
                dayOfWeek--;
            }
            if(dayOfWeek >= (int)c.OverrideDate.DayOfWeek)
            {
                c.ExpirationLength = dayOfWeek - (int)c.OverrideDate.DayOfWeek;
            }
            else
            {
                c.ExpirationLength = 7 - dayOfWeek + (int)c.OverrideDate.DayOfWeek;
            }
            //Debug.Log($"today: {OverrideDate.DayOfWeek}, goal: {dayOfWeek}");
            //Debug.Log("ExpirationDate: " + c.OverrideDate.AddDays(c.WeeksDelay * 7 + c.WeeksLifetime * 7 + c.ExpirationLength).Date);
        }
        else
        {
            c.WeeksLifetime = -1;
        }

        string templentext = TempLengthInputHours.text.Replace(TMP_Specials.clear, "") + ":" + TempLengthInputMinutes.text.Replace(TMP_Specials.clear, "");
        if (DayTimeManager.TryParseLength(templentext, out DateTime templen))
        {
            c.TempLength = templen.TimeOfDay;
        }

        // IDK if I'll overwrite start time.

        //if (DayTimeManager.TryParseTime(StartTimeInput.text.Replace(TMP_Specials.clear, ""), out DateTime tempstart))
        //{
        //    if (SelectedCellColumn == 0)
        //    {
        //        DayTimeManager.instance.WeekDays[SelectedCellRow].ExpirationDate = OverrideDate.AddDays(c.WeeksDelay*7 + c.WeeksLifetime*7);
        //        DayTimeManager.instance.WeekDays[SelectedCellRow].TempStartTime = tempstart.TimeOfDay;
                
        //    }
        //    else if (!c.cellUI.isbreak)
        //    {
        //        int childIndex = TimetableEditor.instance.Grid.ColumnsList[SelectedCellColumn - 1].isBreak ? 0 : SelectedCellRow;
        //        CellInfo PrevInfo = TimetableEditor.instance.Grid.ColumnsList[SelectedCellColumn - 1].Children[childIndex].Info;

        //        long ticks = DayTimeManager.instance.TimeDiff(start.TimeOfDay, SelectedCellColumn - 1, SelectedCellRow).Ticks;

        //        // Instead of using Mathf.Abs, we manually make the value absolute to avoid losing any bytes, since this is a long.
        //        if (ticks < 0) ticks = -ticks;

        //        PrevInfo.Length = new TimeSpan(ticks);

        //        PrevInfo.OverrideCommonLength = PrevInfo.Length != DayTimeManager.instance.GetCellCommonLength(SelectedCellRow);
        //    }
        //}


        DayTimeManager.instance.UpdateTimeIndexes();
        c.UpdateUI();

        SelectedCellColumn = -1;
        SelectedCellRow = -1;

        gameObject.SetActive(false);
    }
}
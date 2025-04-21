using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System;

public class CellInfoEditor : MonoBehaviour
{
    CellInfo SelectedInfo;
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


    public void SelectCell(CellInfo info)
    {
        SelectedInfo = info;
        originalEvent = SelectedInfo.SelectedEvent;
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

        TypeOverride.value = info.Override.EventType + 1;
        EventNameOverride.text = info.Override.EventName;
        Info1Override.text = info.Override.Info1;
        Info2Override.text = info.Override.Info2;

        FavouriteOverride.value = (info.Override.OverrideFavourite ? 1 : 0) * (1 + (info.Override.Favourite ? 1 : 0));

        UpdatePreviews();
    }
    public void ToggleOverrideTime(bool overridetime)
    {
        StartTimeInput.gameObject.SetActive(overridetime && !SelectedInfo.cellUI.isbreak);
        LengthInput.gameObject.SetActive(overridetime);
    }
    public void ParseLength(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if(!DayTimeManager.ParsableLength(text, out DateTime length))
        {
            LengthInput.text = $"{SelectedInfo.Length.Hours}:{SelectedInfo.Length.Minutes}";
        }
    }
    public void ParseTime(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (!DayTimeManager.ParsableLength(text, out DateTime length))
        {
            TimeSpan t = DayTimeManager.instance.GetCellTime(SelectedInfo);
            LengthInput.text = $"{t.Hours}:{(t.Minutes<10?"0":"")}{t.Minutes}";
        }
    }
    
    // This should be used by the 'Event Selector' overlay.
    public void ChangeInfoBase(int EventID)
    {
        SelectedInfo.SelectedEvent = EventID;
        SelectedInfo.UpdateUI();
        TimetableEditor.instance.EventSelectorOverlay.SetActive(false);

        UpdatePreviews();
    }
    public void UpdatePreviews()
    {
        EventItem e = EventManager.Instance.GetEvent(SelectedInfo.SelectedEvent);
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

        //TO DO: Make special override type of EventItem
        SelectedInfo.Override.EventName = EventNameOverride.text.Replace(TMP_Specials.clear, "");
        SelectedInfo.Override.Info1 = Info1Override.text.Replace(TMP_Specials.clear, "");
        SelectedInfo.Override.Info2 = Info2Override.text.Replace(TMP_Specials.clear, "");
        SelectedInfo.Override.EventType = TypeOverride.value - 1;
        SelectedInfo.Override.OverrideFavourite = FavouriteOverride.value > 0;
        SelectedInfo.Override.Favourite = FavouriteOverride.value > 1;
        SelectedInfo.UpdateUI();
        SelectedInfo = null;

        gameObject.SetActive(false);
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EventCreator : MonoBehaviour
{
    public int IDToModify;
    public CanvasGroup SelfGroup;

    [Header("Preview")]
    public TimetableCell PreviewCell;
    [Space]
    public TMP_Text TitleText;
    [Space(30)]
    [Header("Property Fields")]
    public TMP_InputField EventNameInput;
    public TMP_InputField Info1Input;
    public TMP_InputField Info2Input;
    [Space]
    public TMP_Dropdown EventTypeDropdown;
    public Toggle FavouriteToggle;
    [Space]
    public Button DeleteButton;
    public void OpenCreator(int ID)
    {
        DeleteButton.interactable = ID >= 0;

        TitleText.text = ID >= 0 ? LocalizationSystem.instance.GetText(gameObject.name, "EDIT_EVENT") : LocalizationSystem.instance.GetText(gameObject.name, "CREATE_EVENT");

        IDToModify = ID;

        //SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        //SelfGroup.alpha = 1;

        //Setting up the dropdown.
        EventTypeDropdown.ClearOptions();
        TMP_Dropdown.OptionData[] dropdownOptions = new TMP_Dropdown.OptionData[EventManager.Instance.EventTypes.Count];
        for (int i = 0; i < dropdownOptions.Length; i++)
        {
            dropdownOptions[i] = new(EventManager.Instance.EventTypes[i].TypeName);
        }
        EventTypeDropdown.AddOptions(dropdownOptions.ToList());


        // Setting Defaults
        if (ID >= 0)
        {
            EventItem a = EventManager.Instance.GetEvent(ID);

            // Text
            EventNameInput.text = a.EventName + TMP_Specials.clear;
            Info1Input.text = a.Info1 + TMP_Specials.clear;
            Info2Input.text = a.Info2 + TMP_Specials.clear;

            // Misc
            int eventtype = a.EventType >= 0 && a.EventType < EventManager.Instance.EventTypes.Count ? a.EventType : 0; // Prevents out of range exceptions
            EventTypeDropdown.value = EventManager.Instance.GetEventTypeIndex(eventtype);
            FavouriteToggle.isOn = a.Favourite;
        }
        else
        {
            // Text
            EventNameInput.text = PreviewCell.EventNameText.text =  EventManager.Instance.DefaultNewEvent.EventName;
            Info1Input.text = PreviewCell.Info1Text.text = EventManager.Instance.DefaultNewEvent.Info1;
            Info2Input.text = PreviewCell.Info2Text.text = EventManager.Instance.DefaultNewEvent.Info2;

            // Misc
            EventTypeDropdown.value = EventManager.Instance.DefaultNewEvent.EventType;

            FavouriteToggle.isOn = EventManager.Instance.DefaultNewEvent.Favourite;
        }

        // We do this to change the color of the preview, without requiring additional code.
        ChangeEventType(EventTypeDropdown.value);

        ChangeIsFavourite(FavouriteToggle.isOn);

        //EventManager.Instance.ShowEditingOverlay();
    }

    // These functions simply change the preview. They're only meant for visual feedback.
    public void ChangeEventName(string text)
    {
        PreviewCell.EventNameText.text = text;
    }
    public void ChangeInfo1Name(string text)
    {
        PreviewCell.Info1Text.text = text;
    }
    public void ChangeInfo2Name(string text)
    {
        PreviewCell.Info2Text.text = text;
    }
    public void ChangeEventType(int type)
    {
        EventTypeItem e = EventManager.Instance.EventTypes[type];
        PreviewCell.BackgroundImage.color = e.BackgroundColor;
        PreviewCell.EventNameText.color = PreviewCell.Info1Text.color = PreviewCell.Info2Text.color = e.TextColor;
    }
    public void ChangeIsFavourite(bool favourite)
    {
        PreviewCell.FavouriteImage.gameObject.SetActive(favourite);
    }
    public void Confirm()
    {
        if (IDToModify < 0)
        {
            // Create New
            EventManager.Instance.CreateNewEvent(out EventItem a);

            a.EventName = EventNameInput.text.Replace(TMP_Specials.clear, "");
            a.Info1 = Info1Input.text.Replace(TMP_Specials.clear, "");
            a.Info2 = Info2Input.text.Replace(TMP_Specials.clear, "");

            a.EventType = EventTypeDropdown.value;
            a.Favourite = FavouriteToggle.isOn;
        }
        else
        {
            // Edit Existing
            EventItem a = EventManager.Instance.GetEvent(IDToModify);

            a.EventName = EventNameInput.text.Replace(TMP_Specials.clear, "");
            a.Info1 = Info1Input.text.Replace(TMP_Specials.clear, "");
            a.Info2 = Info2Input.text.Replace(TMP_Specials.clear, "");

            a.EventType = EventManager.Instance.EventTypes[EventTypeDropdown.value].ItemID;
            a.Favourite = FavouriteToggle.isOn;
        }
        SaveManager.instance.ChangesMade();
        EventManager.Instance.UpdateEventPreviews(true);
        EventManager.Instance.UpdateEventSelectors();
        TimetableEditor.instance.UpdateSelectorPreview();
        gameObject.SetActive(false);
    }
    public void Delete(bool confirm)
    {
        if (!confirm)
        {
            ConfirmationManager.ButtonPrompt Cancel = new(LocalizationSystem.instance.GetText(gameObject.name, "BUTTONS_CANCEL"), null);
            ConfirmationManager.ButtonPrompt Confirm = new(LocalizationSystem.instance.GetText(gameObject.name, "BUTTONS_DELETE"), delegate { Delete(true); });
            ConfirmationManager.Instance.ShowConfirmation
            (
                LocalizationSystem.instance.GetText(gameObject.name, "PROMPT_TITLE_AREYOUSURE"),
                LocalizationSystem.instance.GetText(gameObject.name, "PROMPT_DESC_EVENT").Replace("{x}", EventNameInput.text.Replace(TMP_Specials.clear, "")),
                Cancel, Confirm
            );
            return;
        }
        // Prevent deletion if we're deleting a new event.
        if (IDToModify < 0) return;
        EventManager.Instance.DeleteEvent(IDToModify);
        EventManager.Instance.UpdateEventPreviews(true);
        EventManager.Instance.UpdateEventSelectors();
        TimetableEditor.instance.UpdateSelectorPreview();
        gameObject.SetActive(false);
    }
}
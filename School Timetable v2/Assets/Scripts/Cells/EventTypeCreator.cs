using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class EventTypeCreator : MonoBehaviour
{
    public int IDToModify;
    public CanvasGroup SelfGroup;
    [Header("Preview")]
    public TimetableCell PreviewCell;
    [Space]
    public TMP_Text TitleText;

    [Space(30)]

    [Header("Property Fields")]
    public TMP_InputField EventTypeNameInput;
    [Space]
    public Image ChangeTextColor;
    public Image ChangeBackgroundColor;
    [Space]
    //public Button ChangeTextButton;
    //public Button ChangeBackgroundButton;
    [Space]
    public Button DeleteButton;

    //private void Start()
    //{
    //    CloseCreator();
    //}

    public void OpenCreator(int ID)
    {
        DeleteButton.interactable = ID > 0; // Prevent the user from deleting the new or the default event type.
        EventTypeNameInput.interactable = ID != 0; // Prevent the user from changing the default event name.

        TitleText.text = ID >= 0 ? LocalizationSystem.instance.GetText(gameObject.name, "EDIT_EVENTTYPE") : LocalizationSystem.instance.GetText(gameObject.name, "CREATE_EVENTTYPE");

        IDToModify = ID;

        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;

        // Setting Defaults
        if(ID >= 0)
        {
            EventTypeItem a = EventManager.Instance.GetEventType(ID);

            // Text String
            EventTypeNameInput.text = a.TypeName;
            if (EventTypeNameInput.text != "") EventTypeNameInput.text += TMP_Specials.clear;

            PreviewCell.EventNameText.text = a.TypeName;

            // Background Color
            ChangeBackgroundColor.color =
                PreviewCell.BackgroundImage.color =
                a.BackgroundColor;

            // Text Color
            ChangeTextColor.color =
                PreviewCell.EventNameText.color =
                PreviewCell.Info1Text.color =
                PreviewCell.Info2Text.color =
                a.TextColor;
        }
        else
        {
            // Text String
            EventTypeNameInput.text = PreviewCell.EventNameText.text = EventManager.Instance.DefaultNewEventType.TypeName;

            // Background Color
            ChangeBackgroundColor.color = 
                PreviewCell.BackgroundImage.color = 
                EventManager.Instance.DefaultNewEventType.BackgroundColor;

            // Text Color
            ChangeTextColor.color = 
                PreviewCell.EventNameText.color = 
                PreviewCell.Info1Text.color = 
                PreviewCell.Info2Text.color = 
                EventManager.Instance.DefaultNewEventType.TextColor;
        }
    }

    public void CloseCreator()
    {
        ColorEditor.instance.ApplyColors();
        gameObject.SetActive(false);
    }


    public void ChangeEventTypeName(string text)
    {
        PreviewCell.EventNameText.text = text.Replace(TMP_Specials.clear, "");
    }


    public void ActivateColorEditor(bool ChangeBackground) // If it's not background, it's text.
    {
        if (ChangeBackground)
        {
            Color currentColor = ChangeBackgroundColor.color;

            ColorEditor.instance.Open(LocalizationSystem.instance.GetText(gameObject.name, "COLOREDITOR_PROMPT_BACKGROUND"), 
                currentColor, ChangeBackgroundColor, PreviewCell.BackgroundImage);
        }
        else
        {
            Color currentColor = ChangeTextColor.color;

            ColorEditor.instance.Open(LocalizationSystem.instance.GetText(gameObject.name, "COLOREDITOR_PROMPT_TEXT"), 
                currentColor, PreviewCell.EventNameText, PreviewCell.Info1Text, PreviewCell.Info2Text);

            ColorEditor.instance.AssignNewImages(ChangeTextColor);
        }
    }
    public void Confirm()
    {
        if (IDToModify < 0)
        {
            // Create New
            EventManager.Instance.CreateNewEventType(out EventTypeItem a);
            a.BackgroundColor = ChangeBackgroundColor.color;
            a.TextColor = ChangeTextColor.color;
            a.TypeName = EventTypeNameInput.text.Replace(TMP_Specials.clear, "");
        }
        else
        {
            // Edit Existing
            EventTypeItem a = EventManager.Instance.GetEventType(IDToModify);
            a.BackgroundColor = ChangeBackgroundColor.color;
            a.TextColor = ChangeTextColor.color;
            a.TypeName = EventTypeNameInput.text.Replace(TMP_Specials.clear, "");

            EventManager.Instance.UpdateEventPreviews(true);
            EventManager.Instance.UpdateEventSelectors();
            TimetableEditor.instance.UpdateSelectorPreview();
        }
        SaveManager.instance.ChangesMade();
        EventManager.Instance.UpdateEventTypePreviews(true);
        CloseCreator();
    }
    public void Delete(bool confirm)
    {
        if(IDToModify > 0)
        {
            if (!confirm)
            {
                ConfirmationManager.ButtonPrompt Cancel = new(LocalizationSystem.instance.GetText(gameObject.name, "BUTTONS_CANCEL"), null);
                ConfirmationManager.ButtonPrompt Confirm = new(LocalizationSystem.instance.GetText(gameObject.name, "BUTTONS_DELETE"), delegate { Delete(true); });
                ConfirmationManager.Instance.ShowConfirmation
                (
                    LocalizationSystem.instance.GetText(gameObject.name, "PROMPT_TITLE_AREYOUSURE"),
                    LocalizationSystem.instance.GetText(gameObject.name, "PROMPT_DESC_EVENTTYPE").Replace("{x}", EventTypeNameInput.text.Replace(TMP_Specials.clear, "")),
                    Cancel, Confirm
                );
                return;
            }
            EventManager.Instance.DeleteEventType(IDToModify);

            EventManager.Instance.UpdateEventTypePreviews(true);
            EventManager.Instance.UpdateEventPreviews(true);
            EventManager.Instance.UpdateEventSelectors();
            TimetableEditor.instance.UpdateSelectorPreview();
            CloseCreator();
        }
    }
}
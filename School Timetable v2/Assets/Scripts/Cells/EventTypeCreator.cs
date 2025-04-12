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

    [Space(30)]

    [Header("Property Fields")]
    public TMP_InputField EventTypeNameInput;
    [Space]
    public Image ChangeTextColor;
    public Image ChangeBackgroundColor;
    [Space]
    public Button ChangeTextButton;
    public Button ChangeBackgroundButton;
    [Space]
    public Button DeleteButton;

    [Space(30)]

    [Header("Color Editor")]
    public ColorEditor coloreditor;

    private void Start()
    {
        CloseCreator();
    }

    public void OpenCreator(int ID)
    {
        coloreditor.OnClose.RemoveAllListeners();
        coloreditor.OnClose.AddListener(delegate { ReEnableColorButtons(); });

        DeleteButton.interactable = ID > 0; // Prevent the user from deleting the new or the default event type.
        EventTypeNameInput.interactable = ID != 0; // Prevent the user from changing the default event name.

        string verb = ID >= 0 ? "Edit" : "Create new";
        CellManager.Instance.TitleText.text = $"{verb} Event Type";

        IDToModify = ID;

        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;

        // Setting Defaults
        if(ID >= 0)
        {
            EventTypeItem a = CellManager.Instance.GetEventType(ID);

            // Text String
            EventTypeNameInput.text = a.TypeName;

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
            EventTypeNameInput.text = CellManager.Instance.DefaultNewEventType.TypeName;

            // Background Color
            ChangeBackgroundColor.color = 
                PreviewCell.BackgroundImage.color = 
                CellManager.Instance.DefaultNewEventType.BackgroundColor;

            // Text Color
            ChangeTextColor.color = 
                PreviewCell.EventNameText.color = 
                PreviewCell.Info1Text.color = 
                PreviewCell.Info2Text.color = 
                CellManager.Instance.DefaultNewEventType.TextColor;
        }
        
        CellManager.Instance.ShowEditingOverlay();
    }

    public void CloseCreator()
    {
        SelfGroup.interactable = SelfGroup.blocksRaycasts = false;
        SelfGroup.alpha = 0;

        CellManager.Instance.HideEditingOverlay();
    }


    public void ChangeEventTypeName(string text)
    {
        PreviewCell.EventNameText.text = text;
    }


    public void ActivateColorEditor(bool ChangeBackground) // If it's not background, it's text.
    {
        ChangeTextButton.interactable = ChangeBackgroundButton.interactable = false;
        if (ChangeBackground)
        {
            Color currentColor = ChangeBackgroundColor.color;
            coloreditor.Open("Edit Background Color", currentColor, ChangeBackgroundColor, PreviewCell.BackgroundImage);
        }
        else
        {
            Color currentColor = ChangeTextColor.color;
            coloreditor.Open("", currentColor, PreviewCell.EventNameText, PreviewCell.Info1Text, PreviewCell.Info2Text);
            coloreditor.Open("Edit Text Color", currentColor, ChangeTextColor);
        }
    }

    public void ReEnableColorButtons()
    {
        ChangeTextButton.interactable = ChangeBackgroundButton.interactable = true;
    }

    public void Save()
    {
        if(IDToModify < 0)
        {
            // Create New
            CellManager.Instance.CreateNewEventType(out EventTypeItem a);
            a.BackgroundColor = ChangeBackgroundColor.color;
            a.TextColor = ChangeTextColor.color;
            a.TypeName = EventTypeNameInput.text.Replace(TMP_Specials.clear,"");
        }
        else
        {
            // Edit Existing
            EventTypeItem a = CellManager.Instance.GetEventType(IDToModify);
            a.BackgroundColor = ChangeBackgroundColor.color;
            a.TextColor = ChangeTextColor.color;
            a.TypeName = EventTypeNameInput.text.Replace(TMP_Specials.clear,"");

            CellManager.Instance.UpdateEventPreviews();
        }

        CellManager.Instance.UpdateEventTypePreviews();
        CloseCreator();
    }
    public void Delete()
    {
        if(IDToModify > 0)
        {
            CellManager.Instance.DeleteEventType(IDToModify);

            CellManager.Instance.UpdateEventTypePreviews();
            CellManager.Instance.UpdateEventPreviews();
            CloseCreator();
        }
    }
    public void Cancel()
    {
        coloreditor.Close();
        CloseCreator();
    }
}
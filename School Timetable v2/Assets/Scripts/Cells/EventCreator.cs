using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventCreator : MonoBehaviour
{
    public int IDToModify;

    [Header("Preview")]
    public TimetableCell PreviewCell;

    [Space(30)]

    [Header("Property Fields")]
    public TMP_InputField EventNameInput;
    public TMP_InputField Info1Input;
    public TMP_InputField Info2Input;
    [Space]
    public TMP_Dropdown EventType;
    public Toggle Favourite;

    // These functions simply change the preview. They're only meant for visual feedback.
    public void ChangeEventName(string text)
    {

    }
    public void ChangeInfo1Name(string text)
    {

    }
    public void ChangeInfo2Name(string text)
    {

    }
    public void ChangeEventType(int type)
    {

    }
    public void ChangeIsFavourite(bool favourite)
    {

    }
    public void Save()
    {
        if (IDToModify < 0)
        {
            // Create New
        }
        else
        {
            // Edit Existing
        }
    }
}
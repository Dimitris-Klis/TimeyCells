using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;

public class CellManager : MonoBehaviour
{
    public static CellManager Instance;
    [Header("General")]
    public TMP_Text TitleText; // We're going to edit the contents so it says "Create" or "Edit"
    public CanvasGroup EditingOverlay;

    [Space(30)]

    [Header("Creating/Editing Events")]

    public EventCreator EventCreatorUI;
    public EventItem DefaultNewEvent; // When creating a new event, we set the fields to the correct values.
    

    [Space(30)]

    [Header("Creating/Editing Event Types")]

    public EventTypeCreator EventTypeCreatorUI;
    public EventTypeItem DefaultNewEventType;
    
    [Space(30)]

    public List<EventTypeItem> EventTypes = new();
    public List<EventItem> Events = new();

    private void Awake()
    {
        Instance = this;
    }
    public void ShowEditingOverlay()
    {
        EditingOverlay.alpha = 1;
        EditingOverlay.interactable = EditingOverlay.blocksRaycasts = true;
    }
    public void HideEditingOverlay()
    {
        EditingOverlay.alpha = 0;
        EditingOverlay.interactable = EditingOverlay.blocksRaycasts = false;
    }
    public void CreateNewEventType(out EventTypeItem _item)
    {
        EventTypes.Add(new());
        EventTypeItem item = EventTypes[^1];
        

        // Get largest value.
        int maxVal = EventTypes.Max(p1 => p1.ItemID);
        if (EventTypes.Count == 1) item.ItemID = 1;
        else item.ItemID= maxVal+1;

        //Sort by ID
        EventTypes.Sort((item1, item2) => item1.ItemID.CompareTo(item2.ItemID));

        _item = item;
    }
    public EventTypeItem GetEventType(int ID)
    {
        for (int i = 0; i < EventTypes.Count; i++)
        {
            if (EventTypes[i].ItemID == ID) return EventTypes[i];
        }
        return null;
    }
    public EventItem GetEvent(int ID)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i].ItemID == ID) return Events[i];
        }
        return null;
    }

}
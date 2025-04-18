using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    
    [Header("General")]
    
    public TMP_Text TitleText; // We're going to edit the contents so it says "Create" or "Edit"
    public CanvasGroup EditingOverlay;
    public CellInfoEditor CellInfoEditor;
    public ScrollZoom ZoomHandler;

    [Space(30)]

    [Header("Creating/Editing Events")]

    public EventCreator EventCreator;
    public EventItem DefaultNewEvent; // When creating a new event, we set the fields to the correct values.
    

    [Space(30)]

    [Header("Creating/Editing Event Types")]

    public EventTypeCreator EventTypeCreator;
    public EventTypeItem DefaultNewEventType;
    

    [Space(30)]

    [Header("Lists")]

    public List<EventTypeItem> EventTypes = new();
    public List<EventItem> Events = new();


    [Space(30)]

    [Header("List GFX")]
    
    public TimetableCell CellPrefab;

    [Space]
    
    public Transform EventsParent;
    public Transform EventSelectorsParent;
    public List<TimetableCell> EventPreviews = new();
    public List<TimetableCell> EventSelectorPreviews = new();

    [Space]
    
    public Transform EventTypesParent;
    public List<TimetableCell> EventTypePreviews = new();

    private void Awake()
    {
        Instance = this;

        // TO DO: PREVENT DEFAULT EVENT GENERATION WHEN LOADING SAVE FILE.
        
        if (GetEventType(0) == null)
        {
            // The Default Event Type
            CreateNewEventType(out EventTypeItem defaultType);
            defaultType.TypeName = "Default";
            defaultType.BackgroundColor = Color.white;
            defaultType.TextColor = Color.black;
        }
        if (GetEvent(0) == null)
        {
            // The Default Event -> None
            CreateNewEvent(out EventItem defaultEvent);
            defaultEvent.EventName = "";
            defaultEvent.Info1 = "";
            defaultEvent.Info2 = "";
            defaultEvent.EventType = 0;
        }
        UpdateEventTypePreviews();
        UpdateEventPreviews();
        UpdateEventSelectors();
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
        
        if (EventTypes.Count == 1) item.ItemID = 0;
        else
        {
            // Get largest ID
            int maxVal = int.MinValue;
            for (int i = 0; i < EventTypes.Count; i++)
            {
                if (EventTypes[i].ItemID > maxVal) maxVal = EventTypes[i].ItemID;
            }
            item.ItemID = maxVal + 1;
        }

        //Sort by ID
        EventTypes.Sort((item1, item2) => item1.ItemID.CompareTo(item2.ItemID));

        _item = item;
    }
    public void CreateNewEvent(out EventItem _item)
    {
        Events.Add(new());
        EventItem item = Events[^1];

        if (Events.Count == 1) item.ItemID = 0;
        else
        {
            // Get largest ID
            int maxVal = int.MinValue;
            for(int i =0; i< Events.Count; i++)
            {
                if (Events[i].ItemID > maxVal) maxVal = Events[i].ItemID;
            }
            item.ItemID = maxVal + 1;
        }

        //Sort by ID
        Events.Sort((item1, item2) => item1.ItemID.CompareTo(item2.ItemID));

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
    public int GetEventTypeIndex(int ID)
    {
        for (int i = 0; i < EventTypes.Count; i++)
        {
            if (EventTypes[i].ItemID == ID) return i;
        }
        return -1;
    }

    public EventItem GetEvent(int ID)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i].ItemID == ID) return Events[i];
        }
        return null;
    }
    
    public void DeleteEventType(int ID)
    {
        int index = -1;
        for (int i = 0; i < EventTypes.Count; i++)
        {
            if (EventTypes[i].ItemID == ID)
            {
                index = i;
                break;

                // TO DO: update timetable cells when deleting event types to avoid cells with non existent event types.
            }
        }
        EventTypes.RemoveAt(index);
        if(index==-1)
            Debug.Log($"No event found with ID '{ID}'!");
    }
    public void DeleteEvent(int ID)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i].ItemID == ID)
            {
                Events.RemoveAt(i);

                // TO DO: Update timetable to not have any non existent events.

                return;
            }
        }
        Debug.Log($"No event found with ID '{ID}'!");
    }
    public void UpdateEventTypePreviews()
    {
        int modAmount = EventTypes.Count - EventTypePreviews.Count;
        
        //Debug.Log(modAmount);
        
        // Add missing or remove extra previews.
        if (modAmount > 0)
        {
            for (int i = 0; i < modAmount; i++)
            {
                TimetableCell c = Instantiate(CellPrefab, EventTypesParent);
                c.transform.localScale = Vector3.one * 1.5f;
                EventTypePreviews.Add(c);
            }
        }
        else
        {
            // Substitue while loop for a for loop to prevent crashing.
            for (int i = 0; i < EventTypes.Count+10; i++)
            {
                if (EventTypePreviews.Count == EventTypes.Count) break;
                Destroy(EventTypePreviews[^1].gameObject);
                EventTypePreviews.RemoveAt(EventTypePreviews.Count - 1);
            }
        }

        for (int i = 0; i < EventTypePreviews.Count; i++)
        {
            var c = EventTypePreviews[i];

            // Text Setup
            c.EventNameText.text = EventTypes[i].TypeName;
            c.Info1Text.text = string.Empty;
            c.Info2Text.text = string.Empty;

            // Color Setup
            EventTypeItem t = EventTypes[i];
            c.BackgroundImage.color = t.BackgroundColor;
            c.EventNameText.color = c.Info1Text.color = c.Info2Text.color = t.TextColor;

            // Fav Setup
            c.FavouriteImage.gameObject.SetActive(false);

            // Edit Button Setup
            SetButton(c.SelfButton, EventTypes[i].ItemID, true);
        }

        TimetableEditor.instance.Grid.UpdateAllCells();
    }
    public void UpdateEventPreviews()
    {
        int modAmount = Events.Count - EventPreviews.Count;

        // Add missing or remove extra previews.
        if (modAmount > 0)
        {
            for (int i = 0; i < modAmount; i++)
            {
                TimetableCell c = Instantiate(CellPrefab, EventsParent);

                c.transform.localScale = Vector3.one * 1.5f;
                EventPreviews.Add(c);
            }
        }
        else if (modAmount < 0)
        {
            // Substitue while loop for a for loop to prevent crashing.
            for(int i = 0; i < Events.Count+10; i++)
            {
                Destroy(EventPreviews[^1].gameObject);
                EventPreviews.RemoveAt(EventPreviews.Count-1);
                if (EventPreviews.Count <= Events.Count) break;
            }
        }

        // Styling the event previews & setting up button functionality.
        for (int i = 0; i < EventPreviews.Count; i++)
        {
            var c = EventPreviews[i];
            
            // Text Setup
            c.EventNameText.text = Events[i].EventName;
            c.Info1Text.text = Events[i].Info1;
            c.Info2Text.text = Events[i].Info2;

            // Color Setup
            EventTypeItem t = GetEventType(Events[i].EventType);
            if(t == null) t = GetEventType(0);
            c.BackgroundImage.color = t.BackgroundColor;
            c.EventNameText.color = c.Info1Text.color = c.Info2Text.color = t.TextColor;

            // Fav Setup
            c.FavouriteImage.gameObject.SetActive(Events[i].Favourite);

            // Edit Button Setup
            SetButton(c.SelfButton, Events[i].ItemID, false);
        }

        EventPreviews[0].EventNameText.text = "None";
        EventPreviews[0].SelfButton.interactable = false;

        TimetableEditor.instance.Grid.UpdateAllCells();
    }
    void SetButton(Button b, int id, bool EventType)
    {
        b.onClick.RemoveAllListeners();
        if(!EventType)
            b.onClick.AddListener(
            delegate {
                //Debug.Log($"Opening {id}");
                EventCreator.gameObject.SetActive(true);
                EventCreator.OpenCreator(id); 
            });
        else 
            b.onClick.AddListener(
            delegate 
            {
                //Debug.Log($"Opening {id}");
                EventTypeCreator.gameObject.SetActive(true);
                EventTypeCreator.OpenCreator(id); 
            });
    }
    public void UpdateEventSelectors()
    {
        int modAmount = Events.Count - EventSelectorPreviews.Count;

        // Add missing or remove extra previews.
        if (modAmount > 0)
        {
            for (int i = 0; i < modAmount; i++)
            {
                TimetableCell c = Instantiate(CellPrefab, EventSelectorsParent);

                c.transform.localScale = Vector3.one * 1.5f;
                EventSelectorPreviews.Add(c);
            }
        }
        else if (modAmount < 0)
        {
            // Substitue while loop for a for loop to prevent crashing.
            for (int i = 0; i < Events.Count + 10; i++)
            {
                Destroy(EventSelectorPreviews[^1].gameObject);
                EventSelectorPreviews.RemoveAt(EventSelectorPreviews.Count - 1);
                if (EventSelectorPreviews.Count <= Events.Count) break;
            }
        }

        // Styling the event previews & setting up button functionality.
        for (int i = 0; i < EventSelectorPreviews.Count; i++)
        {
            var c = EventSelectorPreviews[i];

            // Text Setup
            c.EventNameText.text = Events[i].EventName;
            c.Info1Text.text = Events[i].Info1;
            c.Info2Text.text = Events[i].Info2;

            // Color Setup
            EventTypeItem t = GetEventType(Events[i].EventType);
            if (t == null) t = GetEventType(0);
            c.BackgroundImage.color = t.BackgroundColor;
            c.EventNameText.color = c.Info1Text.color = c.Info2Text.color = t.TextColor;

            // Fav Setup
            c.FavouriteImage.gameObject.SetActive(Events[i].Favourite);

            // Edit Button Setup
            int id = Events[i].ItemID;
            c.SelfButton.onClick.RemoveAllListeners();
            if(TimetableEditor.instance.Editing)
                c.SelfButton.onClick.AddListener(delegate { TimetableEditor.instance.SelectEvent(id); });
            else
                c.SelfButton.onClick.AddListener(delegate { CellInfoEditor.ChangeInfoBase(id); });
        }

        EventSelectorPreviews[0].EventNameText.text = "None";
    }
}
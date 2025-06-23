using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimetableEditor : MonoBehaviour
{
    public static TimetableEditor instance;
    private void Awake()
    {
        instance = this;
    }

    [Header("     Read Only Variables")]
    [ReadOnly] public bool Editing;
    [ReadOnly] public int SelectedID;


    [Space(20)]


    [Header("References")]
    [Space(0)]
    [Header("--- Important Systems")]
    public DayTimeManager dayTimeManager;
    public TimetableGrid Grid;


    [Header("--- Updating Selected Cell UI")]
    public TimetableCell SelectedCellPreview;


    [Header("--- Timetable Name")]
    public TMP_Text TimetableNameText;
    public TMP_InputField TimetableNameInput;


    [Header("--- Event Selector")]
    public GameObject EventSelectorOverlay;
    public Button SelectorCancelButton;


    [Header("--- Editing Timetable Shape")]
    public Button TableDoneButton;


    [Space(20)]


    [Header("UI to Disable on Edit")]
    public GameObject[] OtherButtons;
    public CanvasGroup[] OtherGroups;
    public GameObject[] EditorButtons;



    public void SelectEvent(int ID)
    {
        SelectedID = ID;

        UpdateSelectorPreview();

        EventSelectorOverlay.SetActive(false);
    }

    public void UpdateSelectorPreview()
    {
        EventItem e = EventManager.Instance.GetEvent(SelectedID);
        EventTypeItem et = EventManager.Instance.GetEventType(e.EventType);

        SelectedCellPreview.EventNameText.text = e.EventName;
        SelectedCellPreview.Info1Text.text = e.Info1;
        SelectedCellPreview.Info2Text.text = e.Info2;

        if (SelectedID == 0) SelectedCellPreview.EventNameText.text = "None";

        SelectedCellPreview.BackgroundImage.color = et.BackgroundColor;

        SelectedCellPreview.EventNameText.color = et.TextColor;
        SelectedCellPreview.Info1Text.color = et.TextColor;
        SelectedCellPreview.Info2Text.color = et.TextColor;

        SelectedCellPreview.FavouriteImage.gameObject.SetActive(e.Favourite);
    }

    public void SetTimetableName(string text)
    {
        TimetableNameText.text = text;
    }
    


    public void Setup()
    {
        SelectEvent(0);
        EndEditTable();
        EndEditMode();
    }



    public void BeginEditMode()
    {
        Editing = true;
        TimetableNameText.gameObject.SetActive(!Editing);
        TimetableNameInput.gameObject.SetActive(Editing);
        EventManager.Instance.UpdateEventSelectorButtons();

        BindCellsForQuickAssign();

        for (int i = 0; i < OtherButtons.Length; i++)
        {
            OtherButtons[i].SetActive(false);
        }

        for (int i = 0; i < OtherGroups.Length; i++)
        {
            OtherGroups[i].alpha = 0;
            OtherGroups[i].blocksRaycasts = OtherGroups[i].interactable = false;
        }

        for (int i = 0; i < EditorButtons.Length; i++)
        {
            EditorButtons[i].SetActive(true);
        }

        SelectorCancelButton.onClick.RemoveAllListeners();
        SelectorCancelButton.onClick.AddListener(delegate
        {
            EventSelectorOverlay.SetActive(false);
        });
    }

    public void EndEditMode()
    {
        SaveManager.instance.ChangesMade();
        Editing = false;
        TimetableNameText.gameObject.SetActive(!Editing);
        TimetableNameInput.gameObject.SetActive(Editing);
        EventManager.Instance.UpdateEventSelectorButtons();
        
        BindCellsForManualAssign();

        for (int i = 0; i < OtherButtons.Length; i++)
        {
            OtherButtons[i].SetActive(true);
        }

        for (int i = 0; i < OtherGroups.Length; i++)
        {
            OtherGroups[i].alpha = 1;
            OtherGroups[i].blocksRaycasts = OtherGroups[i].interactable = true;
        }

        for (int i = 0; i < EditorButtons.Length; i++)
        {
            EditorButtons[i].SetActive(false);
        }

        SelectorCancelButton.onClick.RemoveAllListeners();
        SelectorCancelButton.onClick.AddListener(delegate
        {
            EventSelectorOverlay.SetActive(false);
        });
    }


    // Prepares editing for both columns and rows.
    public void BeginEditTable()
    {
        TimetableNameText.gameObject.SetActive(Editing);
        TimetableNameInput.gameObject.SetActive(!Editing);
        for (int i = 0; i < EditorButtons.Length; i++)
        {
            EditorButtons[i].SetActive(false);
        }
        TableDoneButton.gameObject.SetActive(true);

        for (int i = 0; i < DayTimeManager.instance.WeekDayPreviews.Count; i++)
        {
            DayTimeManager.instance.WeekDayPreviews[i].selfButton.interactable = false;
        }
        for (int i = 0; i < DayTimeManager.instance.TimeIndexPreviews.Count; i++)
        {
            DayTimeManager.instance.TimeIndexPreviews[i].button.interactable = false;
        }
    }

    // Ends editing for both columns and rows.
    public void EndEditTable()
    {
        TimetableNameText.gameObject.SetActive(!Editing);
        TimetableNameInput.gameObject.SetActive(Editing);

        Grid.DestroyColumnButtons();
        Grid.DestroyRowButtons();

        for (int i = 0; i < EditorButtons.Length; i++)
        {
            EditorButtons[i].SetActive(true);
        }

        TableDoneButton.gameObject.SetActive(false);

        // Button functionality for new columns
        BindCellsForQuickAssign();

        for (int i = 0; i < DayTimeManager.instance.WeekDayPreviews.Count; i++)
        {
            DayTimeManager.instance.WeekDayPreviews[i].selfButton.interactable = true;
        }

        for (int i = 0; i < DayTimeManager.instance.TimeIndexPreviews.Count; i++)
        {
            DayTimeManager.instance.TimeIndexPreviews[i].button.interactable = true;
        }

        dayTimeManager.Highlight.transform.SetAsLastSibling();
        DragHandleManager.instance.EndSwap();
    }



    public void BindCellsForQuickAssign()
    {
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            for (int j = 0; j < Grid.ColumnsList[i].Children.Count; j++)
            {
                var c = Grid.ColumnsList[i].Children[j];
                c.SelfButton.onClick.RemoveAllListeners();
                c.SelfButton.onClick.AddListener(delegate { c.Info.SetSelfToSelectedEvent(); });

            }
        }
    }

    public void BindCellsForManualAssign()
    {
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            for (int j = 0; j < Grid.ColumnsList[i].Children.Count; j++)
            {
                var c = Grid.ColumnsList[i].Children[j];
                c.SelfButton.onClick.RemoveAllListeners();

                int col = i, row = j;

                c.SelfButton.onClick.AddListener(
                delegate
                {
                    EventManager.Instance.CellInfoEditor.gameObject.SetActive(true);
                    EventManager.Instance.CellInfoEditor.SelectCell(col, row);
                });
            }
        }
    }
}
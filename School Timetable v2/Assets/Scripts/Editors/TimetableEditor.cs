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
    public bool Editing;
    public TMP_Text TimetableNameText;
    public TMP_InputField TimetableNameInput;
    public TimetableGrid Grid;
    public DayTimeManager dayTimeManager;
    public GameObject[] OtherButtons;
    public CanvasGroup[] OtherGroups;
    public GameObject[] EditorButtons;
    public Button ColumnDoneButton;
    [Space]
    public GameObject EventSelectorOverlay;
    public Button SelectorCancelButton;
    [Space]
    public TimetableCell SelectedCellPreview;
    public int SelectedID;
    public void SelectEvent(int ID)
    {
        SelectedID = ID;
        
        UpdateSelectorPreview();

        //EventManager.Instance.ZoomHandler.enabled = true;
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
    public void StartEdit()
    {
        Editing = true;
        TimetableNameText.gameObject.SetActive(!Editing);
        TimetableNameInput.gameObject.SetActive(Editing);
        EventManager.Instance.UpdateEventSelectors();
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            for (int j = 0; j < Grid.ColumnsList[i].Children.Count; j++)
            {
                var c = Grid.ColumnsList[i].Children[j];
                c.SelfButton.onClick.RemoveAllListeners();
                c.SelfButton.onClick.AddListener(delegate { c.Info.SetSelfToSelectedEvent(); });

            }
        }

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
        //for (int i = 0; i < DayTimeManager.instance.WeekDayPreviews.Count; i++)
        //{
        //    DayTimeManager.instance.WeekDayPreviews[i].selfButton.interactable = false;
        //}

        SelectorCancelButton.onClick.RemoveAllListeners();
        SelectorCancelButton.onClick.AddListener(delegate
        {
            EventSelectorOverlay.SetActive(false);
        });
    }
    public void EndEdit()
    {
        Editing = false;
        TimetableNameText.gameObject.SetActive(!Editing);
        TimetableNameInput.gameObject.SetActive(Editing);
        EventManager.Instance.UpdateEventSelectors();
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            for (int j = 0; j < Grid.ColumnsList[i].Children.Count; j++)
            {
                var c = Grid.ColumnsList[i].Children[j];
                c.SelfButton.onClick.RemoveAllListeners();

                int col = i, row=j;

                c.SelfButton.onClick.AddListener(
                delegate
                {
                    EventManager.Instance.CellInfoEditor.gameObject.SetActive(true);
                    EventManager.Instance.CellInfoEditor.SelectCell(col, row);
                });
            }
        }

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
        //for (int i = 0; i < DayTimeManager.instance.WeekDayPreviews.Count; i++)
        //{
        //    DayTimeManager.instance.WeekDayPreviews[i].selfButton.interactable = true;
        //}

        SelectorCancelButton.onClick.RemoveAllListeners();
        SelectorCancelButton.onClick.AddListener(delegate
        {
            EventSelectorOverlay.SetActive(false);
        });
    }

    public void StartEditColumns()
    {
        TimetableNameText.gameObject.SetActive(Editing);
        TimetableNameInput.gameObject.SetActive(!Editing);
        for (int i = 0; i < EditorButtons.Length; i++)
        {
            EditorButtons[i].SetActive(false);
        }
        ColumnDoneButton.gameObject.SetActive(true);

        for (int i = 0; i < DayTimeManager.instance.WeekDayPreviews.Count; i++)
        {
            DayTimeManager.instance.WeekDayPreviews[i].selfButton.interactable = false;
        }
        for (int i = 0; i < DayTimeManager.instance.TimeIndexPreviews.Count; i++)
        {
            DayTimeManager.instance.TimeIndexPreviews[i].button.interactable = false;
        }
    }
    public void EndEditColumns()
    {
        TimetableNameText.gameObject.SetActive(!Editing);
        TimetableNameInput.gameObject.SetActive(Editing);
        Grid.RemoveColumnButtons();
        Grid.RemoveRowButtons();
        for (int i = 0; i < EditorButtons.Length; i++)
        {
            EditorButtons[i].SetActive(true);
        }
        ColumnDoneButton.gameObject.SetActive(false);

        // Button functionality for new columns
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            for (int j = 0; j < Grid.ColumnsList[i].Children.Count; j++)
            {
                var c = Grid.ColumnsList[i].Children[j];
                c.SelfButton.onClick.RemoveAllListeners();
                c.SelfButton.onClick.AddListener(delegate { c.Info.SetSelfToSelectedEvent(); });

            }
        }

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
    public void SetTimetableName(string text)
    {
        TimetableNameText.text = text.Replace(TMP_Specials.clear, "");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup()
    {
        SelectEvent(0);
        EndEditColumns();
        EndEdit();
    }
}
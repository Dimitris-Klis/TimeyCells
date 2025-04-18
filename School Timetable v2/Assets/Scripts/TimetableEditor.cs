using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class TimetableEditor : MonoBehaviour
{
    public static TimetableEditor instance;
    private void Awake()
    {
        instance = this;
    }
    public bool Editing;
    public TimetableGrid Grid;
    public GameObject[] OtherButtons;
    public CanvasGroup[] OtherGroups;
    public GameObject[] EditorButtons;
    [Space]
    public GameObject EventSelectorOverlay;
    [Space]
    public TimetableCell SelectedCellPreview;
    public int SelectedID;
    public void SelectEvent(int ID)
    {
        SelectedID = ID;
        
        UpdateSelectorPreview();

        EventManager.Instance.ZoomHandler.enabled = true;
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
    }
    public void EndEdit()
    {
        Editing = false;
        EventManager.Instance.UpdateEventSelectors();
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            for (int j = 0; j < Grid.ColumnsList[i].Children.Count; j++)
            {
                var c = Grid.ColumnsList[i].Children[j];
                c.SelfButton.onClick.RemoveAllListeners();

                CellInfo info = c.Info;
                c.SelfButton.onClick.AddListener(
                    delegate
                    {
                        EventManager.Instance.CellInfoEditor.gameObject.SetActive(true);
                        EventManager.Instance.CellInfoEditor.SelectCell(info);
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
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SelectEvent(0);
        EndEdit();
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CellInfoEditor : MonoBehaviour
{
    CellInfo SelectedInfo;
    int originalEvent;
    public TimetableCell MainPreview;
    public TimetableCell BasePreview;
    public void SelectCell(CellInfo info)
    {
        SelectedInfo = info;
        originalEvent = SelectedInfo.SelectedEvent;
        EventManager.Instance.ZoomHandler.enabled = false;
        UpdatePreviews();
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


    }
    public void Cancel()
    {
        ChangeInfoBase(originalEvent);
        EventManager.Instance.ZoomHandler.enabled = true;
        gameObject.SetActive(false);
    }
    public void Save()
    {
        EventManager.Instance.ZoomHandler.enabled = true;
        gameObject.SetActive(false);
    }
}
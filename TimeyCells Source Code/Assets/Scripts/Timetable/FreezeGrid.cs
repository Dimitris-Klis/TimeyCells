using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FreezeGrid : MonoBehaviour
{
    public TimetableGrid Timetable;
    public RectTransform Child;
    public RectTransform rect;
    
    public RectTransform TimetableViewportRect;
    
    Vector2 originalDelta;
    Vector2 originalViewportDelta;

    public enum FreezeModes {FreezeX, FreezeY}
    public FreezeModes FreezeMode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalDelta = rect.sizeDelta;
        originalViewportDelta = TimetableViewportRect.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 WantedChildPos = Child.position;
        Vector2 WantedChildSizeDelta = Child.sizeDelta;

        Vector3 WantedScale = Vector3.one;

        if(FreezeMode == FreezeModes.FreezeY)
        {
            WantedChildPos.x = Timetable.transform.position.x;
            WantedChildSizeDelta.x = Timetable.rect.sizeDelta.x;

            WantedScale = Timetable.transform.localScale;

            Child.position = WantedChildPos;
            Child.sizeDelta = WantedChildSizeDelta;

            Child.localScale = WantedScale;
            rect.sizeDelta = new Vector2(TimetableViewportRect.sizeDelta.x, originalDelta.y * WantedScale.y);

            rect.position = new(TimetableViewportRect.position.x, rect.position.y, rect.position.z);

            TimetableViewportRect.sizeDelta = new Vector2(TimetableViewportRect.sizeDelta.x, originalViewportDelta.y - (originalDelta.y * (WantedScale.y - 1)));
        }
        else
        {
            WantedChildPos.y = Timetable.transform.position.y;
            WantedChildSizeDelta.y = Timetable.rect.sizeDelta.y;

            WantedScale = Timetable.transform.localScale;

            Child.position = WantedChildPos;
            Child.sizeDelta = WantedChildSizeDelta;

            Child.localScale = WantedScale;
            rect.sizeDelta = new Vector2(originalDelta.x * WantedScale.x, TimetableViewportRect.sizeDelta.y);

            rect.position = new(rect.position.x, TimetableViewportRect.position.y, rect.position.z);

            TimetableViewportRect.sizeDelta = new Vector2(originalViewportDelta.x - (originalDelta.x * (WantedScale.x - 1)), TimetableViewportRect.sizeDelta.y);
        }
        
    }
}

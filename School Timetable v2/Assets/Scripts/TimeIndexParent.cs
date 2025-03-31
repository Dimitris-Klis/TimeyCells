using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TimeIndexParent : MonoBehaviour
{
    public TimetableGrid Timetable;
    public RectTransform Child;
    public RectTransform rect;
    
    public RectTransform TimetableViewportRect;
    
    public Vector2 originalDelta;
    public Vector2 originalViewportDelta;
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

        WantedChildPos.x = Timetable.transform.position.x;
        WantedChildSizeDelta.x = Timetable.rect.sizeDelta.x;

        WantedScale = Timetable.transform.localScale;

        Child.position = WantedChildPos;
        Child.sizeDelta = WantedChildSizeDelta;

        Child.localScale = WantedScale;
        rect.sizeDelta = new Vector2(originalDelta.x, originalDelta.y * WantedScale.y);
        TimetableViewportRect.sizeDelta = new Vector2(originalViewportDelta.x, originalViewportDelta.y - (originalDelta.y * (WantedScale.y - 1)) );
    }
}

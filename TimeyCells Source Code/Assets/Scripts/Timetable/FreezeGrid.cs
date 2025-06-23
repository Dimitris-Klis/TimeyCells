using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FreezeGrid : MonoBehaviour
{
    public enum FreezeModes { FreezeX, FreezeY }

    [Header("References")]
    public TimetableGrid Timetable;
    public RectTransform TimetableViewportRect;
    [Space]
    public RectTransform SelfRect;
    public RectTransform Child;
    
    [Space(20)]
    [Header("FreezeMode")]
    public FreezeModes FreezeMode;


    // Stuff for Start()
    Vector2 originalDelta;
    Vector2 originalViewportDelta;

    // Stuff for Update()
    Vector3 WantedChildPos;
    Vector2 WantedChildSizeDelta;

    Vector3 WantedScale;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalDelta = SelfRect.sizeDelta;
        originalViewportDelta = TimetableViewportRect.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        WantedChildPos = Child.position;
        WantedChildSizeDelta = Child.sizeDelta;

        if (FreezeMode == FreezeModes.FreezeY)
        {
            WantedChildPos.x = Timetable.transform.position.x;
            WantedChildSizeDelta.x = Timetable.rect.sizeDelta.x;

            WantedScale = Timetable.transform.localScale;

            Child.position = WantedChildPos;
            Child.sizeDelta = WantedChildSizeDelta;

            Child.localScale = WantedScale;
            SelfRect.sizeDelta = new Vector2(TimetableViewportRect.sizeDelta.x, originalDelta.y * WantedScale.y);

            SelfRect.position = new(TimetableViewportRect.position.x, SelfRect.position.y, SelfRect.position.z);

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
            SelfRect.sizeDelta = new Vector2(originalDelta.x * WantedScale.x, TimetableViewportRect.sizeDelta.y);

            SelfRect.position = new(SelfRect.position.x, TimetableViewportRect.position.y, SelfRect.position.z);

            TimetableViewportRect.sizeDelta = new Vector2(originalViewportDelta.x - (originalDelta.x * (WantedScale.x - 1)), TimetableViewportRect.sizeDelta.y);
        }
        
    }
}

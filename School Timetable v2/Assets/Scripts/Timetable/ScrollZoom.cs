using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform ScrollView;
    public RectTransform Table;
    public float MinScale = 1, MaxScale = 3;
    public float ScrollSensitivity = .25f;
    bool mouseOver;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver=true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseOver) return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ScrollView, Input.mousePosition, null, out Vector2 mousepos);

        if (Input.mouseScrollDelta.y > 0)
        {
            Table.localScale += Vector3.one * ScrollSensitivity;
            if(Table.localScale.x > MaxScale)
            {
                Table.localScale = Vector3.one * MaxScale;
            }
            else
            {
                Table.localPosition -= (Vector3)mousepos;
            }
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            Table.localScale -= Vector3.one * ScrollSensitivity;
            if (Table.localScale.x < MinScale)
                Table.localScale = Vector3.one * MinScale;
        }
    }
}

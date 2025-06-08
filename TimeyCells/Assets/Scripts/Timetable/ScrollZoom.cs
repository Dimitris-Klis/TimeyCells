using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform ScrollView;
    public RectTransform Table;
    public float MinScale = 1, MaxScale = 3;
    public float ScrollSensitivity = .25f;
    bool mouseOver;
    [Space]
    public ScrollRect ScrollHandler;
    public RectTransform Viewport;
    public bool Dragging;
    public bool Horizontal;
    public float Speed = 20;
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
        Vector2 mousepos = Vector2.zero;
        // Debug.Log($"v: ({Viewport.rect.width}, {Viewport.rect.height}),      m: {mousepos}");
        if (Dragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Viewport, Input.mousePosition, null, out mousepos);
            if (Horizontal)
            {
                if (mousepos.x > Viewport.rect.width / 2)
                {
                    ScrollHandler.normalizedPosition += Speed * Time.deltaTime * Vector2.right;
                }
                else if (mousepos.x < -Viewport.rect.width / 2)
                {
                    ScrollHandler.normalizedPosition -= Speed * Time.deltaTime * Vector2.right;
                }
                ScrollHandler.horizontalNormalizedPosition = Mathf.Clamp(ScrollHandler.horizontalNormalizedPosition, 0, 1);
            }
            else
            {
                if (mousepos.y - Viewport.rect.height/2 > Viewport.rect.height / 2)
                {
                    ScrollHandler.normalizedPosition += Speed * Time.deltaTime * Vector2.up;
                }
                else if (mousepos.y - Viewport.rect.height / 2 < -Viewport.rect.height / 2)
                {
                    ScrollHandler.normalizedPosition -= Speed * Time.deltaTime * Vector2.up;
                }
                ScrollHandler.verticalNormalizedPosition = Mathf.Clamp(ScrollHandler.verticalNormalizedPosition, 0, 1);
            }
        }

        if (!mouseOver) return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ScrollView, Input.mousePosition, null, out mousepos);

        if (Input.mouseScrollDelta.y > 0)
        {
            Table.localScale += Vector3.one * ScrollSensitivity;
            if (Table.localScale.x > MaxScale)
            {
                Table.localScale = Vector3.one * MaxScale;
            }
            else
            {
                Table.localPosition -= (Vector3)mousepos;
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Table.localScale -= Vector3.one * ScrollSensitivity;
            if (Table.localScale.x < MinScale)
                Table.localScale = Vector3.one * MinScale;
        }
        if (Input.touchCount != 2) return;
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position in the previous frame of each touch
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (distance) between the touches in each frame
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between each frame
        float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

        // Adjust the scale based on the pinch distance change
        float scaleChange = deltaMagnitudeDiff * ScrollSensitivity * 0.01f; // scale it down for smoothness

        Table.localScale += Vector3.one * scaleChange;

        // Clamp the scale between min and max
        if (Table.localScale.x > MaxScale)
        {
            Table.localScale = Vector3.one * MaxScale;
        }
        else if (Table.localScale.x < MinScale)
        {
            Table.localScale = Vector3.one * MinScale;
        }
    }
}

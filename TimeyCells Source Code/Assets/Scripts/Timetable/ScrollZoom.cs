using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Main Camera")]
    public Camera MainCam;

    [Space(20)]
    [Header("Zoom References")]
    public RectTransform ScrollView;
    public RectTransform Table;

    [Header("Properties")]
    public float MinScale = 1, MaxScale = 3;
    public float ScrollSensitivity = .25f;
    bool mouseOver;

    [Space(20)]
    [Header("Drag References")] // When we're swapping columns/rows.
    public ScrollRect ScrollHandler;
    public RectTransform Viewport;
    [Space]
    public float DragSpeed = 20;

    [Header("     Drag Toggles")]
    [ReadOnly] public bool Dragging;
    [ReadOnly] public bool DragHorizontal;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver=true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }




    void HandleDrag()
    {
        if (!Dragging) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(Viewport, Input.mousePosition, MainCam, out Vector2 mousepos);
        
        if (DragHorizontal)
        {
            if (mousepos.x > Viewport.rect.width / 2)
            {
                ScrollHandler.normalizedPosition += DragSpeed * Time.deltaTime * Vector2.right;
            }
            else if (mousepos.x < -Viewport.rect.width / 2)
            {
                ScrollHandler.normalizedPosition -= DragSpeed * Time.deltaTime * Vector2.right;
            }
            ScrollHandler.horizontalNormalizedPosition = Mathf.Clamp(ScrollHandler.horizontalNormalizedPosition, 0, 1);
            return;
        }

        if (mousepos.y - Viewport.rect.height / 2 > Viewport.rect.height / 2)
        {
            ScrollHandler.normalizedPosition += DragSpeed * Time.deltaTime * Vector2.up;
        }
        else if (mousepos.y - Viewport.rect.height / 2 < -Viewport.rect.height / 2)
        {
            ScrollHandler.normalizedPosition -= DragSpeed * Time.deltaTime * Vector2.up;
        }
        ScrollHandler.verticalNormalizedPosition = Mathf.Clamp(ScrollHandler.verticalNormalizedPosition, 0, 1);
    }




    void HandleScrollZoom()
    {
        if (!mouseOver) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ScrollView, Input.mousePosition, MainCam, out Vector2 mousepos);

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
    }

    void HandlePinchZoom()
    {
        if (!mouseOver) return;
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
    }

    void ClampZoom()
    {
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




    // Update is called once per frame
    void Update()
    {
        HandleDrag();

        if(Application.isMobilePlatform) HandlePinchZoom();
        else HandleScrollZoom();

        ClampZoom();
    }
}
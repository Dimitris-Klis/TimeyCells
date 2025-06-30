using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] ScrollRect ScrollHandler;
    [SerializeField] RectTransform Viewport;
    [SerializeField] RectTransform Table;
    [SerializeField] RectTransform TempParent;

    [Space(20)]
    [Header("Zoom Properties")]
    [SerializeField] float MinScale = 1, MaxScale = 3;
    [SerializeField] float ScrollSensitivity = .25f;
    [SerializeField] float EdgeNudge = 20;

    [Space(20)]
    [Header("Drag Properties")] // When we're swapping columns/rows.
    [SerializeField] float DragSpeed = 8;

    [Space(20)]
    [Header("     Drag Toggles")]
    [ReadOnly] public bool Dragging;
    [ReadOnly] public bool DragHorizontal;

    bool mouseOver;

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

        Vector2 mousePos = Input.mousePosition;
        Rect rect = RectTransformToScreenSpace(Viewport);

        if (DragHorizontal)
        {
            if (mousePos.x > rect.xMax)
            {
                ScrollHandler.normalizedPosition += DragSpeed * Time.deltaTime * Vector2.right;
            }
            else if (mousePos.x < rect.xMin)
            {
                ScrollHandler.normalizedPosition -= DragSpeed * Time.deltaTime * Vector2.right;
            }
            ScrollHandler.horizontalNormalizedPosition = Mathf.Clamp01(ScrollHandler.horizontalNormalizedPosition);
            return;
        }

        if (mousePos.y > rect.yMax)
        {
            ScrollHandler.normalizedPosition += DragSpeed * Time.deltaTime * Vector2.up;
        }
        else if (mousePos.y < rect.yMin)
        {
            ScrollHandler.normalizedPosition -= DragSpeed * Time.deltaTime * Vector2.up;
        }
        ScrollHandler.verticalNormalizedPosition = Mathf.Clamp01(ScrollHandler.verticalNormalizedPosition);
    }

    Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector3[] corners = new Vector3[4];
        transform.GetWorldCorners(corners);

        // Bottom-left corner in screen space
        float x = corners[0].x;
        float y = corners[0].y;

        // Top-right corner
        float width = corners[2].x - x;
        float height = corners[2].y - y;

        return new Rect(x, y, width, height);
    }




    void HandleScrollZoom()
    {
        if (!mouseOver) return;
        float scrollDeltaY = Input.mouseScrollDelta.y;
        if (Mathf.Approximately(scrollDeltaY, 0)) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Viewport, Input.mousePosition, null, out Vector2 mousepos);

        TempParent.localPosition = mousepos;

        // Giving Table a temporary parent to allow for zooming towards the cursor.
        Table.transform.SetParent(TempParent);

        TempParent.localScale += Mathf.Sign(scrollDeltaY) * Vector3.one * ScrollSensitivity;

        if (TempParent.localScale.x > MaxScale)
        {
            TempParent.localScale = Vector3.one * MaxScale;
        }

        if (TempParent.localScale.x < MinScale)
        {
            TempParent.localScale = Vector3.one * MinScale;
        }

        Table.transform.SetParent(Viewport);

        // Since the height of the table is constant, we add this to scroll quickly to the top or bottom.
        if (mousepos.y < 10) ScrollHandler.verticalNormalizedPosition = 0;
        else if (mousepos.y > 200) ScrollHandler.verticalNormalizedPosition = 1;

        // When scrolling left/right, we also want to nudge the content towards that direction.
        float contentWidth = Table.rect.width * Table.localScale.x;
        float viewportWidth = Viewport.rect.width;

        float normalizedPerPixel = 1f / (contentWidth - viewportWidth);
        float normalizedNudge = EdgeNudge * normalizedPerPixel;


        if (mousepos.x < -220) ScrollHandler.horizontalNormalizedPosition -= normalizedNudge;
        else if (mousepos.x > 220) ScrollHandler.horizontalNormalizedPosition += normalizedNudge;

        ScrollHandler.horizontalNormalizedPosition = Mathf.Clamp01(ScrollHandler.horizontalNormalizedPosition);
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
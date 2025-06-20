using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.UI;

public class DragHandle : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public DragHandleManager.SwapAxis SwapAxis;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public int currIndex;

    public void OnDrag(PointerEventData eventData)
    {
        DragHandleManager.instance.ScrollViewManager.Dragging = true;
        DragHandleManager.instance.ScrollViewManager.Horizontal = SwapAxis == DragHandleManager.SwapAxis.Horizontal;
        Vector3 pos = transform.position;
        if (SwapAxis == DragHandleManager.SwapAxis.Horizontal)
        {
            pos.x = eventData.position.x;
            
        }
        else
        {
            pos.y = eventData.position.y;
        }
        transform.position = pos;
        int newIndex = DragHandleManager.instance.GetClosestIndex(transform.localPosition, SwapAxis);
        if(newIndex != -1 && newIndex != currIndex)
        {
            OnSwapDragged(currIndex, newIndex);

            if(SwapAxis == DragHandleManager.SwapAxis.Horizontal)
                DragHandleManager.instance.SwapHorizontal(currIndex, newIndex);
            else
                DragHandleManager.instance.SwapVertical(currIndex, newIndex);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragHandleManager.instance.ScrollViewManager.Dragging = false;
        transform.localPosition = startPos;
    }

    // This is used to call any other swapping functions (mainly for swapping the Timetable Grid's rows or columns)
    public virtual void OnSwapDragged(int IndexA, int IndexB)
    {

    }

    // We use this to update DragHandle GFX.
    public virtual void OnSwap()
    {

    }
}
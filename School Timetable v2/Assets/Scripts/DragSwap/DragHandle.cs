using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DragHandleManager.SwapAxis SwapAxis;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public int currIndex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;

        if (SwapAxis == DragHandleManager.SwapAxis.Horizontal)
            currIndex = DragHandleManager.instance.HandlesHorizontal.IndexOf(this);
        else
            currIndex = DragHandleManager.instance.HandlesVertical.IndexOf(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = transform.position;
        if(SwapAxis == DragHandleManager.SwapAxis.Horizontal)
        {
            pos.x = eventData.position.x;
        }
        else
        {
            pos.y = eventData.position.y;
        }
        transform.position = pos;
        int newIndex = DragHandleManager.instance.GetClosestIndex(transform.position, SwapAxis);
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
        transform.position = startPos;
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
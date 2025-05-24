using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public DragNSwapTest.SwapDimensions SwapDimension;
    public RectTransform Child;
    public RectTransform Self;
    public Vector3 startPos;
    public float moveSpeed = 5;
    bool mouse;
    public void OnPointerDown(PointerEventData eventData)
    {
        mouse = true;
        Child.transform.SetAsLastSibling();
        transform.SetAsLastSibling();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        mouse = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 vec = transform.localPosition;
        if (SwapDimension == DragNSwapTest.SwapDimensions.Horizontal)
        {
            vec.x = Child.localPosition.x;
            transform.localPosition = vec;
        }
        else
        {
            vec.y = Child.localPosition.y;
            transform.localPosition = vec;
        }
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        Vector3 childpos = Child.localPosition;
        if (mouse)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(DragNSwapTest.instance.DragParent, Input.mousePosition, null, out Vector2 mousepos);
            if (SwapDimension == DragNSwapTest.SwapDimensions.Horizontal)
                pos.x = childpos.x = mousepos.x;
            else
                pos.y = childpos.y = mousepos.y;

            DragNSwapTest.instance.TrySwap(this);
        }
        else
        {
            if (SwapDimension == DragNSwapTest.SwapDimensions.Horizontal)
            {
                if(pos.x != startPos.x)
                {
                    pos.x += Mathf.Sign(startPos.x - pos.x) * moveSpeed * 100 * Time.deltaTime;
                    if (Mathf.Abs(pos.x - startPos.x) < .5f) pos.x = startPos.x;
                }
                
                childpos.x = pos.x;
            }
            else
            {
                if (pos.y != startPos.y)
                {
                    pos.y += Mathf.Sign(startPos.y - pos.y) * moveSpeed * 100 * Time.deltaTime;
                    if (Mathf.Abs(pos.y - startPos.y) < .5f) pos.y = startPos.y;
                }

                childpos.x = pos.x;
            }
        }
        transform.localPosition = pos;
        Child.localPosition = childpos;
    }
}
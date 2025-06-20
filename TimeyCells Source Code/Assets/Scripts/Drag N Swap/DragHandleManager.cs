using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DragHandleManager : MonoBehaviour
{
    public static DragHandleManager instance;
    private void Awake()
    {
        instance = this;
    }
    public enum SwapAxis {Horizontal, Vertical};
    public ScrollZoom ScrollViewManager;
    public DragHandle HorizontalDragPrefab; // The ones that move left or right.
    public DragHandle VerticalDragPrefab; // The ones that move up or down.
    [Space]
    public CanvasGroup DaysOfWeekParent;
    public CanvasGroup TimeIndexesParent;
    [Space]
    public CanvasGroup DaysOfWeekDRAGParent;
    public CanvasGroup TimeIndexesDRAGParent;

    public Transform HorizontalParent;
    public Transform VerticalParent;
    [Space]
    public CustomLayoutGroup HorizontalLayout;
    [Space]
    public CustomLayoutGroup VerticalLayout;
    [Space]
    public List<DragHandle> HandlesVertical = new();
    public List<DragHandle> HandlesHorizontal = new();



    public List<RectTransform> objects;

    public void StartSwap(bool horizontal)
    {
        DaysOfWeekParent.interactable = DaysOfWeekParent.blocksRaycasts = false;
        TimeIndexesParent.interactable = TimeIndexesParent.blocksRaycasts = false;

        if (horizontal)
        {
            TimeIndexesParent.alpha = 0;
            DaysOfWeekParent.alpha = 1;

            DaysOfWeekDRAGParent.alpha = 0;
            TimeIndexesDRAGParent.alpha = 1;

            TimeIndexesDRAGParent.interactable = TimeIndexesDRAGParent.blocksRaycasts = true;
            DaysOfWeekDRAGParent.interactable = DaysOfWeekDRAGParent.blocksRaycasts = false;
            

            for (int i = 0; i < DayTimeManager.instance.Grid.Columns; i++)
            {
                DragHandle d = Instantiate(HorizontalDragPrefab, HorizontalParent);
                d.currIndex = i;
                d.OnSwap();
                HandlesHorizontal.Add(d);
            }

            HorizontalLayout.UpdateLayout();

            for (int i = 0; i < HandlesHorizontal.Count; i++)
            {
                HandlesHorizontal[i].startPos = HandlesHorizontal[i].transform.localPosition;
            }
        }
        else
        {
            TimeIndexesParent.alpha = 1;
            DaysOfWeekParent.alpha = 0;

            DaysOfWeekDRAGParent.alpha = 1;
            TimeIndexesDRAGParent.alpha = 0;

            TimeIndexesDRAGParent.interactable = TimeIndexesDRAGParent.blocksRaycasts = false;
            DaysOfWeekDRAGParent.interactable = DaysOfWeekDRAGParent.blocksRaycasts = true;

            VerticalLayout.enabled = true;
            for (int i = 0; i < DayTimeManager.instance.Grid.Rows; i++)
            {
                DragHandle d = Instantiate(VerticalDragPrefab, VerticalParent);
                d.currIndex = i;
                d.OnSwap();
                HandlesVertical.Add(d);
            }

            VerticalLayout.UpdateLayout();

            for (int i = 0; i < HandlesVertical.Count; i++)
            {
                HandlesVertical[i].startPos = HandlesVertical[i].transform.localPosition;
            }
        }
    }
    public void EndSwap()
    {
        DaysOfWeekParent.interactable = DaysOfWeekParent.blocksRaycasts = true;
        TimeIndexesParent.interactable = TimeIndexesParent.blocksRaycasts = true;

        TimeIndexesParent.alpha = 1;
        DaysOfWeekParent.alpha = 1;

        TimeIndexesDRAGParent.alpha = 0;
        DaysOfWeekDRAGParent.alpha = 0;

        TimeIndexesDRAGParent.interactable = TimeIndexesDRAGParent.blocksRaycasts = false;
        DaysOfWeekDRAGParent.interactable = DaysOfWeekDRAGParent.blocksRaycasts = false;

        for (int i = 0; i < HandlesHorizontal.Count; i++)
        {
            Destroy(HandlesHorizontal[i].gameObject);
        }
        HandlesHorizontal.Clear();

        for (int i = 0; i < HandlesVertical.Count; i++)
        {
            Destroy(HandlesVertical[i].gameObject);
        }
        HandlesVertical.Clear();
    }

    public void SwapHorizontal(int IndexA, int IndexB)
    {
        Vector3 startPosA = HandlesHorizontal[IndexA].startPos;
        HandlesHorizontal[IndexA].startPos = HandlesHorizontal[IndexB].startPos;
        HandlesHorizontal[IndexB].startPos = startPosA;

        HandlesHorizontal[IndexB].transform.localPosition = startPosA;
        
        HandlesHorizontal[IndexA].currIndex = IndexB;
        HandlesHorizontal[IndexB].currIndex = IndexA;

        DragHandle hA = HandlesHorizontal[IndexA];

        HandlesHorizontal[IndexA] = HandlesHorizontal[IndexB];
        HandlesHorizontal[IndexB] = hA;
        HandlesHorizontal[IndexA].OnSwap();
        HandlesHorizontal[IndexB].OnSwap();
    }
    public void SwapVertical(int IndexA, int IndexB)
    {
        Vector3 startPosA = HandlesVertical[IndexA].startPos;
        HandlesVertical[IndexA].startPos = HandlesVertical[IndexB].startPos;
        HandlesVertical[IndexB].startPos = startPosA;

        HandlesVertical[IndexB].transform.localPosition = startPosA;

        HandlesVertical[IndexA].currIndex = IndexB;
        HandlesVertical[IndexB].currIndex = IndexA;

        DragHandle hA = HandlesVertical[IndexA];

        HandlesVertical[IndexA] = HandlesVertical[IndexB];
        HandlesVertical[IndexB] = hA;
        HandlesVertical[IndexA].OnSwap();
        HandlesVertical[IndexB].OnSwap();
    }

    public int GetClosestIndex(Vector3 dragPos, SwapAxis axis)
    {
        float closest = float.MaxValue;
        int closestIndex = -1;
        if(axis == SwapAxis.Horizontal)
        {
            for (int i = 0; i < HandlesHorizontal.Count; i++)
            {
                float dist = 0;
                dist = Mathf.Abs(HandlesHorizontal[i].startPos.x - dragPos.x);

                if (dist < closest)
                {
                    closest = dist;
                    closestIndex = i;
                }
            }
        }
        else
        {
            for (int i = 0; i < HandlesVertical.Count; i++)
            {
                float dist = 0;
                dist = Mathf.Abs(HandlesVertical[i].startPos.y - dragPos.y);

                if (dist < closest)
                {
                    closest = dist;
                    closestIndex = i;
                }
            }
        }
        return closestIndex;
    }
}

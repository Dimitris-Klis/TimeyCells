using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DragHandleManager : MonoBehaviour
{
    public static DragHandleManager instance;
    private void Awake()
    {
        instance = this;
    }
    public RectTransform DragParent;
    public enum SwapAxis {Horizontal, Vertical};

    public List<DragHandle> HandlesVertical;
    public List<DragHandle> HandlesHorizontal;
    public List<RectTransform> objects;

    public void SwapHorizontal(int IndexA, int IndexB)
    {
        Vector3 startPosA = HandlesHorizontal[IndexA].startPos;
        HandlesHorizontal[IndexA].startPos = HandlesHorizontal[IndexB].startPos;
        HandlesHorizontal[IndexB].startPos = startPosA;

        HandlesHorizontal[IndexB].transform.position = startPosA;
        
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

        HandlesVertical[IndexB].transform.position = startPosA;

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
        for (int i = 0; i < HandlesHorizontal.Count; i++)
        {
            float dist = 0;
            if (axis == SwapAxis.Horizontal)
            {
                dist = Mathf.Abs(HandlesHorizontal[i].startPos.x - dragPos.x);
                
            }
            else
            {
                dist = Mathf.Abs(HandlesVertical[i].startPos.y - dragPos.y);

            }
            if (dist < closest)
            {
                closest = dist;
                closestIndex = i;
            }
        }
        return closestIndex;
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DragNSwapTest : MonoBehaviour
{
    public static DragNSwapTest instance;
    private void Awake()
    {
        instance = this;
    }
    public RectTransform DragParent;
    public enum SwapDimensions {Horizontal, Vertical};
    [SerializeField] SwapDimensions SwapDimension;

    public List<DragHandle> Handles;
    public List<RectTransform> objects;

    public void TrySwap(DragHandle currentHandle)
    {
        if (Handles.Count < 2) return;
        int handleIndex = -1;
        for (int i = 0; i < Handles.Count; i++)
        {
            if (Handles[i] == currentHandle)
            {
                handleIndex = i;
                break;
            }
        }
        int indexToSwap = -1;
        float biggestdiff = 0;
        for (int i = 0; i < Handles.Count; i++)
        {
            if (i == handleIndex) continue;

            if (Handles[handleIndex].Self.localPosition.x < Handles[i].startPos.x)
            {
                float diff = Mathf.Abs(Handles[handleIndex].Self.localPosition.x - Handles[i].startPos.x);
                if ( diff > biggestdiff)
                {
                    indexToSwap = i;
                    biggestdiff = diff;
                }
                    
                //Swapping positions
                Vector2 oldstartpos = Handles[handleIndex].startPos;
                Handles[handleIndex].startPos = Handles[i].startPos;
                Handles[i].startPos = oldstartpos;

                //Swapping indexes
                //DragHandle prev = Handles[i];
                //Handles[i] = Handles[handleIndex];
                //Handles[handleIndex] = prev;

                //RectTransform prevc = objects[i];
                //objects[i] = objects[handleIndex];
                //objects[handleIndex] = prevc;
            }
        }
        if(indexToSwap >= 0)
        {
            Handles.Insert(indexToSwap, Handles[handleIndex]);
            Handles.RemoveAt(handleIndex);
        }
        
    }
}

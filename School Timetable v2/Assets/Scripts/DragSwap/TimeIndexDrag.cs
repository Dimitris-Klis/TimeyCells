using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class TimeIndexDrag : DragHandle
{
    public TMP_Text IndexText;
    private void Start()
    {
        OnSwap();
    }
    public override void OnSwapDragged(int IndexA, int IndexB)
    {
        DayTimeManager.instance.Grid.SwapColumns(IndexA, IndexB);
    }
    public override void OnSwap()
    {
        IndexText.text = DayTimeManager.instance.GetColumnIndexAt(currIndex);
    }
}
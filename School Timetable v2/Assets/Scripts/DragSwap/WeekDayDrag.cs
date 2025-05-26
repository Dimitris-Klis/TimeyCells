using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class WeekDayDrag : DragHandle
{
    public TMP_Text WeekDayText;
    public override void OnSwapDragged(int IndexA, int IndexB)
    {
        DayTimeManager.instance.Grid.SwapRows(IndexA, IndexB);
    }
    public override void OnSwap()
    {
        WeekDayText.text = DayTimeManager.instance.WeekDays[currIndex].DayName;
    }
}
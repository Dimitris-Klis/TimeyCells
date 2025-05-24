using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class testdayofweek : MonoBehaviour
{
    [Range(0, 6)] public int day = 0;
    [ContextMenu("Test day of week")]
    public void Test()
    {
        int diff = day - (int)DateTime.Now.DayOfWeek;
        Debug.Log(DateTime.Now.AddDays(diff).DayOfWeek);
    }
}

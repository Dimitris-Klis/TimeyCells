using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class WeekDay
{
    // The days for which the day will run.
    public bool Mon, Tue, Wed, Thu, Fri, Sat, Sun;
    [Space]
    public string DayName;
    public TimeSpan StartTime = new(7, 30, 0);
    public TimeSpan CommonLength = new(1, 0, 0);
}
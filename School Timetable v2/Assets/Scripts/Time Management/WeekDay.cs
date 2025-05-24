using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class WeekDay
{
    // The days for which the day will run.
    public uint Days; // max is 127
    [Space]
    public string DayName;
    public TimeSpan StartTime = new(7, 30, 0);
    
    public TimeSpan TempStartTime = new(7, 30, 0);
    public DateTime ExpirationDate = DateTime.MinValue;
    
    public TimeSpan CommonLength = new(1, 0, 0);

    public WeekDay(string _DayName, uint _Days)
    {
        DayName= _DayName;
        Days = _Days;
    }
}
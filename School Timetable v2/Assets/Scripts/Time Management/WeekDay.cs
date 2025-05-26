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
    
    public TimeSpan CommonLength = new(1, 0, 0);

    public TimeSpan TempStartTime = new(7, 30, 0);
    public TimeSpan TempCommonLength = new(1, 0, 0);

    public int WeeksDelay = 0;
    public int WeeksLifetime = -1;
    public int OverrideMode = 0; // 0= No Override, 1= Override TempStartTime, 2=Override TempCommonLength, 3=OverrideAll

    public DateTime OverrideDate = DateTime.MinValue;
    public int ExpirationLength = 0;

    public WeekDay(string _DayName, uint _Days)
    {
        DayName= _DayName;
        Days = _Days;
    }
    public void CheckExpirationDate()
    {
        if (WeeksLifetime >= 0 && DateTime.Now > OverrideDate.AddDays(WeeksDelay * 7 + WeeksLifetime * 7 + ExpirationLength).AddHours(23).AddMinutes(59))
        {
            WeeksLifetime = -1;
        }
    }
}
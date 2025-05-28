using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class WeekDay
{
    public WeekDay(TimetableData.WeekDayData data)
    {
        DayName = data.WeekDayName;
        Days = (uint)data.Days;

        StartTime = new(data.StartTime[0], data.StartTime[1], 0);
        CommonLength = new(data.CommonLength[0], data.CommonLength[1], 0);

        OverrideExtraLengthWeeks = data.ExtraOverrideLengthWeeks;

        if (data.ExtraOverrideLengthWeeks >= 0)
        {
            OverrideDate = new(data.OverrideDate[0], data.OverrideDate[1], data.OverrideDate[2]);

            OverrideLength = data.OverrideLength;
            OverrideDelayWeeks = data.OverrideDelayWeeks;
            OverrideMode = data.OverrideMode;

            TempStartTime = new(data.TempStartTime[0], data.TempStartTime[1], 0);

            TempCommonLength = new(data.TempCommonLength[0], data.TempCommonLength[1], 0);
        }
    }

    // The days for which the day will run.
    public uint Days; // max is 127
    [Space]
    public string DayName;

    public TimeSpan StartTime = new(7, 30, 0);
    public TimeSpan CommonLength = new(1, 0, 0);




    public TimeSpan TempStartTime = new(7, 30, 0);
    public TimeSpan TempCommonLength = new(1, 0, 0);

    public int OverrideDelayWeeks = 0;
    public int OverrideExtraLengthWeeks = -1;
    public int OverrideMode = 0; // 0: No Override, 1: Override StartTime, 2: Override CommonLength, 3: OverrideAll

    public DateTime OverrideDate = DateTime.MinValue;
    public int OverrideLength = 0;

    public WeekDay(string _DayName, uint _Days)
    {
        DayName= _DayName;
        Days = _Days;
    }
    public void CheckExpirationDate()
    {
        if (OverrideExtraLengthWeeks >= 0 && DateTime.Now > OverrideDate.AddDays(OverrideDelayWeeks * 7 + OverrideExtraLengthWeeks * 7 + OverrideLength).AddHours(23).AddMinutes(59))
        {
            OverrideExtraLengthWeeks = -1;
        }
    }
}
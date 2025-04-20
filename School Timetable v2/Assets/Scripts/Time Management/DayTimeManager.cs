using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using System;
using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    public TimetableGrid Grid;
    public string testtime = "12:00";
    public List<WeekDay> WeekDays = new List<WeekDay>(); // Up to 7 weekdays


    public void Setup()
    {
        // Get weekdays save. If no weekdays, set weekdays to 5.
        // Set Grid rows to weekdays count.
        // Get current day. loop thru rows and columns.
        // If cellInfo overrides CommonTime, use the override instead.
    }

    [ContextMenu("Test Parse Time")]
    public void test()
    {
        //DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

        // Adding support for 24h and 12h formats. Specifically: 16:00, 9:00 PM, 9:00PM
        if (DateTime.TryParseExact(testtime, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ||
            DateTime.TryParseExact(testtime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(testtime, "h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            Debug.Log($"{result.TimeOfDay}");
        }

    }
}
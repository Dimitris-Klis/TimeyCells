using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class DayTimeManager : MonoBehaviour
{
    public static DayTimeManager instance;
    private void Awake()
    {
        instance = this;
    }
    public TimetableGrid Grid;
    public string testtime = "12:00";
    public List<WeekDay> WeekDays = new List<WeekDay>(); // Up to 7 weekdays
    public GameObject Highlight;
    public RectTransform HighlightRect;
    public TMP_Text TimeLeftText;
    DateTime wantedTime = DateTime.Now;

    public void Setup()
    {
        WeekDays.Clear();
        // If no save is found:

        // Default week days:                 SFTWTMS
                                           // 0000001 = 01 Sun -> Since DateTime.DayOfWeek uses Sunday as 0, I too have to use sunday as 0.
           WeekDays.Add(new("Monday", 2)); // 0000010 = 02 Mon
          WeekDays.Add(new("Tuesday", 4)); // 0000100 = 04 Tue
        WeekDays.Add(new("Wednesday", 8)); // 0001000 = 08 Wed
        WeekDays.Add(new("Thursday", 16)); // 0010000 = 16 Thu
          WeekDays.Add(new("Friday", 32)); // 0100000 = 32 Fri

        // Set Grid rows to weekdays count.
        Grid.Rows = WeekDays.Count;
        Grid.Setup();
        TimetableEditor.instance.Setup();
        Grid.UpdateAllCells();
        Highlight.transform.SetAsLastSibling();
        // Get current day. loop thru rows and columns.
        // If cellInfo overrides CommonTime, use the override instead.
    }
    private void Start()
    {
        Setup();
    }

    // Finds a weekday that contains day 0-6.
    public int GetWeekDayIndex(int day) // 0-6
    {
        // Sun=0, Mon=1, Tue=2, Wed=3, Thu=4, Fri=5, Sat=6

        int divider = (int)Mathf.Pow(2, day);

        for (int i = 0; i < WeekDays.Count; i++)
        {
            if (Mathf.FloorToInt(WeekDays[i].Days / divider % 2) == 1) return i;
        }
        return -1;
    }
    public TimeSpan GetCellCommonLength(int weekday)
    {
        return WeekDays[weekday].CommonLength;
    }
    // Compare new startTime with a cell's time
    public TimeSpan TimeDiff(TimeSpan newStartTime, int col, int weekday)
    {
        TimeSpan t = GetCellStartTime(col, weekday);
        return newStartTime.Subtract(t);
    }
    public TimeSpan GetCellStartTime(int col, int weekday)
    {
        if (weekday < 0)
        {
            Debug.LogWarning("Weekday Index out of range!!!");
            return TimeSpan.Zero;
        }

        WeekDay wd = WeekDays[weekday];
        TimeSpan t = wd.StartTime;

        for (int i = 0; i < col-1; i++)
        {
            int index = weekday;
            if (Grid.ColumnsList[i].isBreak) index = 0; // Accounting for rowspans

            var c = Grid.ColumnsList[i].Children[index].Info;

            bool nullOverride = c.Override.EventName == "" && c.Override.Info1 == "" && c.Override.Info2 == "";
            if (nullOverride) nullOverride = c.Override.EventType < 0 && c.Override.OverrideFavourite == false;

            if (c.SelectedEvent == 0 && nullOverride) continue; // If the cell is 'None' and has no overrides, ignore it.

            if (c.OverrideCommonLength)
                t += c.Length;
            else
                t += wd.CommonLength;
        }
        return t;
    }
    public CellInfo GetCurrentCellInfo(int weekdayindex, out TimeSpan diff)
    {
        WeekDay wd = WeekDays[weekdayindex];
        TimeSpan t = wd.StartTime;
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            int index = weekdayindex;
            if (Grid.ColumnsList[i].isBreak) index = 0; // Accounting for rowspans

            var c = Grid.ColumnsList[i].Children[index].Info;

            bool nullOverride = c.Override.EventName == "" && c.Override.Info1 == "" && c.Override.Info2 == "";
            if(nullOverride) nullOverride = c.Override.EventType < 0 && c.Override.OverrideFavourite == false;

            if (c.SelectedEvent == 0 && nullOverride) continue; // If the cell is 'None' and has no overrides, ignore it.
            if (c.OverrideCommonLength)
                t += c.Length;
            else
                t += wd.CommonLength;

            // We keep adding cellInfo lengths until they pass the current time.
            // The moment we pass the time, we've found the info.
            if (DateTime.Now.TimeOfDay < t) 
            {
                diff = t - DateTime.Now.TimeOfDay;
                return c;
            }
        }
        diff = TimeSpan.Zero;
        return null;
    }
    private void Update()
    {
        if(DateTime.Now >= wantedTime) // Updating content every 1 system second.
        {

            wantedTime = DateTime.Now;
            wantedTime = wantedTime.AddTicks(-(wantedTime.Ticks % TimeSpan.TicksPerSecond)) + new TimeSpan(0, 0, 0, 0, 500);
            int weekdayindex = GetWeekDayIndex((int)DateTime.Now.DayOfWeek);
            // If today has no events
            if (weekdayindex < 0)
            {
                // Hide texts and highlights
                HideHighlights();
                return;
            }

            WeekDay wd = WeekDays[weekdayindex];

            // If our day hasn't begun
            if (DateTime.Now.TimeOfDay < wd.StartTime)
            {
                // Hide texts and highlights
                HideHighlights();
                return;
            }


            CellInfo CurrentInfo = GetCurrentCellInfo(weekdayindex, out TimeSpan timeUntilNextLesson);

            // If our day has ended or no infos are in said day
            if (CurrentInfo == null)
            {
                // Hide texts and highlights
                HideHighlights();
                return;
            }
            Highlight.SetActive(!TimetableEditor.instance.Editing);
            Highlight.transform.position = CurrentInfo.transform.position;
            HighlightRect.sizeDelta = CurrentInfo.GetComponent<RectTransform>().sizeDelta + (Vector2.one * 6.2f);
            TimeLeftText.text = !TimetableEditor.instance.Editing ? "Next event in:" + timeUntilNextLesson.ToString(@"mm\:ss") : "";
        }
    }

    public void HideHighlights()
    {
        Highlight.SetActive(false);
        TimeLeftText.text = "";
    }

    [ContextMenu("Test Parse Time")]
    public void test()
    {
        Debug.Log((int)DayOfWeek.Sunday);
        //DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

        // Adding support for 24h and 12h formats. Specifically: 16:00, 9:00 PM, 9:00PM
        if (DateTime.TryParseExact(testtime, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ||
            DateTime.TryParseExact(testtime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(testtime, "h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            // Supporting the british
            DateTime.TryParseExact(testtime, "H.mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(testtime, "h.mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(testtime, "h.mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            Debug.Log($"{result.TimeOfDay}");
        }

    }
    public static bool TryParseTime(string text, out DateTime result)
    {
        return
            DateTime.TryParseExact(text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            // Supporting the british
            DateTime.TryParseExact(text, "H.mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "h.mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "h.mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }
    public static bool TryParseLength(string text, out DateTime result)
    {
        return DateTime.TryParseExact(text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            // Supporting the british
            DateTime.TryParseExact(text, "H.mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }
    
}
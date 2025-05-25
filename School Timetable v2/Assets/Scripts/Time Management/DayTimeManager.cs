using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayTimeManager : MonoBehaviour
{
    public static DayTimeManager instance;
    private void Awake()
    {
        instance = this;
    }
    [Header("The Basics")]
    public TimetableGrid Grid;
    public List<WeekDay> WeekDays = new List<WeekDay>(); // Up to 7 weekdays

    [Space(20)]
    [Header("Displaying Time")]
    public GameObject Highlight;
    public RectTransform HighlightRect;
    public TMP_Text TimeLeftText;

    [Space(20)]
    [Header("Spawning Previews")]
    public WeekdayEditor WeekdayEditor;
    public WeekDayObject WeekDayPrefab;
    public Transform WeekDaysParent;
    public List<WeekDayObject> WeekDayPreviews = new();
    [Space]
    public bool _24hFormat;
    public bool EnglishFormat;
    public TimeIndexObject TimeIndexPrefab;
    public Transform TimeIndexesParent;
    public List<TimeIndexObject> TimeIndexPreviews = new();
    public List<LabelIndex> TimeLabels = new();

    [Space(20)]
    [Header("IndexProperties")]
    public LabelEditor labelEditor;
    public Toggle CustomColSpansToggle;
    public TMP_InputField ColSpansLabelInput;

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
        UpdateWeekDays();
        UpdateTimeIndexes();
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

        for (int i = 0; i < col; i++)
        {
            int index = weekday;
            if (Grid.ColumnsList[i].isBreak) index = 0; // Accounting for rowspans

            var c = Grid.ColumnsList[i].Children[index].Info;

            bool noText = c.Override.EventName == "" && c.Override.Info1 == "" && c.Override.Info2 == "";
            bool noEventsOrFavs = c.Override.EventType < 0 && !c.Override.OverrideFavourite;

            if (c.SelectedEventBase == 0 && noText && noEventsOrFavs) continue; // If the cell is 'None' and has no overrides, ignore it.

            if (c.WeeksLifetime >= 0 && c.TempOverrideCommonLength)
                t += c.TempLength;
            else if (c.OverrideCommonLength)
                t += c.Length;
            else
                t += wd.CommonLength;
        }
        //Debug.Log($"{t.Hours}:{t.Minutes}");
        return t;
    }
    public bool isEmpty(int col, int weekday)
    {
        int wantedIndex = weekday;
        if (Grid.ColumnsList[col].isBreak) wantedIndex = 0;


        var c = Grid.ColumnsList[col].isBreak ? Grid.ColumnsList[col].Children[0].Info : Grid.ColumnsList[col].Children[wantedIndex].Info;

        bool noText = c.Override.EventName == "" && c.Override.Info1 == "" && c.Override.Info2 == "";
        bool noEventsOrFavs = c.Override.EventType < 0 && !c.Override.OverrideFavourite;

        return c.SelectedEventBase == 0 && noText && noEventsOrFavs;
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
            if (nullOverride) nullOverride = c.Override.EventType < 0 && c.Override.OverrideFavourite == false;

            if (c.SelectedEventBase == 0 && nullOverride) continue; // If the cell is 'None' and has no overrides, ignore it.
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
        if (DateTime.Now >= wantedTime) // Updating content every 1 system second.
        {
            wantedTime = DateTime.Now;
            wantedTime = wantedTime.AddTicks(-(wantedTime.Ticks % TimeSpan.TicksPerSecond)) + new TimeSpan(0, 0, 0, 0, 500);
            int weekdayindex = GetWeekDayIndex((int)DateTime.Now.DayOfWeek);

            Grid.CheckCellTempEvents();

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

    public void UpdateWeekDays()
    {
        for (int i = 0; i < WeekDayPreviews.Count; i++)
        {
            Destroy(WeekDayPreviews[i].gameObject);
        }
        WeekDayPreviews.Clear();
        for (int i = 0; i < WeekDays.Count; i++)
        {
            WeekDayObject w = Instantiate(WeekDayPrefab, WeekDaysParent);
            WeekDayPreviews.Add(w);

            w.WeekDayName.text = WeekDays[i].DayName;
            int weekdayToOpen = i;
            w.selfButton.onClick.AddListener(
            delegate
            {
                WeekdayEditor.gameObject.SetActive(true);
                WeekdayEditor.OpenWeekday(weekdayToOpen);
            });
        }
    }
    public void UpdateTimeIndexes()
    {
        while (Grid.ColumnsList.Count > TimeLabels.Count)
        {
            TimeLabels.Add(new());
        }

        for (int i = 0; i < TimeIndexPreviews.Count; i++)
        {
            Destroy(TimeIndexPreviews[i].gameObject);
        }
        TimeIndexPreviews.Clear();

        int rowIndex = GetWeekDayIndex((int)DateTime.Now.DayOfWeek);

        int ColumnIndex = 1;
        for (int i = 0; i < Grid.ColumnsList.Count; i++)
        {
            // TO DO: ADD A BUTTON TO THE OBJECT THAT ON CLICK: OPENS THE LABEL EDITOR AT THE INDEX OF i.
            TimeIndexObject ti = Instantiate(TimeIndexPrefab, TimeIndexesParent);
            TimeIndexPreviews.Add(ti);
            int LabelIndex = i;
            ti.button.onClick.AddListener(delegate { labelEditor.gameObject.SetActive(true); labelEditor.ActivateEditor(LabelIndex); });


            //ti.TimeParent.SetActive(rowIndex >= 0);
            if (rowIndex < 0 || isEmpty(i, rowIndex))
            {
                ti.TimeText.text = "";
                if (TimeLabels[i].IsCustomLabel)
                {
                    ti.IndexText.text = TimeLabels[i].CustomLabelName;
                    if (TimeLabels[i].CountAsIndex) ColumnIndex++;
                }
                else
                {
                    ti.IndexText.text = "";
                }
                continue;
            }

            TimeSpan t = GetCellStartTime(i, rowIndex);
            string tstring = FormatTime(t);

            ti.TimeText.text = tstring;

            if (TimeLabels[i].IsCustomLabel)
            {
                ti.IndexText.text = TimeLabels[i].CustomLabelName;
                if (TimeLabels[i].CountAsIndex) ColumnIndex++;
                continue;
            }
            ti.IndexText.text = ColumnIndex.ToString();
            ColumnIndex++;

            //w.selfButton.onClick.AddListener(WeekDayCreator.OpenCreator)
        }
    }
    public void AddNewWeekday(int index)
    {
        WeekDays.Insert(index, new WeekDay("New Weekday", 0));
        UpdateWeekDays();
        UpdateTimeIndexes();
    }
    public void RemoveWeekday(int index)
    {
        WeekDays.RemoveAt(index);
        UpdateWeekDays();
        UpdateTimeIndexes();
    }
    public void SwapWeekDays(int IndexA, int IndexB)
    {
        WeekDay wdA = WeekDays[IndexA];
        WeekDays[IndexA] = WeekDays[IndexB];
        WeekDays[IndexB] = wdA;

        UpdateWeekDays();
    }


    public void AddIndexLabel(int index)
    {
        TimeLabels.Insert(index, new());
    }
    public void RemoveIndexLabel(int index)
    {
        TimeLabels.RemoveAt(index);
    }
    public void SwapIndexLabels(int IndexA, int IndexB)
    {
        LabelIndex LabelATemp = TimeLabels[IndexA];
        TimeLabels[IndexA] = TimeLabels[IndexB];
        TimeLabels[IndexB] = LabelATemp;
    }
    public string GetColumnIndexAt(int index)
    {
        int rowIndex = GetWeekDayIndex((int)DateTime.Now.DayOfWeek);
        if (TimeLabels[index].IsCustomLabel)
        {
            return TimeLabels[index].CustomLabelName;
        }
        else if(isEmpty(index, rowIndex))
        {
            return "";
        }
        return index.ToString();
    }

    public string FormatTime(TimeSpan t)
    {
        string tstring = "";
        string punctuation = EnglishFormat ? "." : ":";
        if (_24hFormat)
        {
            tstring = $"{t.Hours}{punctuation}{t.Minutes}";
        }
        else
        {
            int hours = t.Hours;
            string tt = "AM";
            if (hours > 12)
            {
                hours -= 12;
                tt = "PM";
            }

            if (hours == 0) hours += 12;
            tstring = $"{hours}{punctuation}{t.Minutes} {tt}";
        }
        return tstring;
    }
    public string FormatLength(TimeSpan t)
    {
        return $"{t.Hours}:{string.Format("{0:00}", t.Minutes)}";
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
    
    public void Set24h(bool is24)
    {
        _24hFormat = is24;
        UpdateTimeIndexes();
    }
    public void SetEnglish(bool english)
    {
        EnglishFormat = english;
        UpdateTimeIndexes();
    }
}
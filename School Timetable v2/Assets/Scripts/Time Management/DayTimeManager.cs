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
    [Space]
    public Toggle _24hToggle;
    public Toggle EnglishToggle;
    [Space]
    public TimeIndexObject TimeIndexPrefab;
    public Transform TimeIndexesParent;
    public List<TimeIndexObject> TimeIndexPreviews = new();
    public List<LabelIndex> TimeLabels = new();

    [Space(20)]
    [Header("IndexProperties")]
    public LabelEditor labelEditor;

    DateTime wantedTime = DateTime.Now;

    public bool begin = false;

    public void Setup() // This should be called by SaveManager
    {
        WeekDays.Clear();
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
        int hours = newStartTime.Hours - t.Hours - (newStartTime.Minutes < t.Minutes ? 1 : 0);
        int mins = newStartTime.Minutes - t.Minutes + (newStartTime.Minutes < t.Minutes ? 60 : 0);
        return new(hours, mins, 0);
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
            if (Grid.ColumnsList[i].IsMultirow) index = 0; // Accounting for rowspans

            var c = Grid.ColumnsList[i].Children[index].Info;

            bool noText = c.Override.EventName == "" && c.Override.Info1 == "" && c.Override.Info2 == "";

            if (c.SelectedEventBase == 0 && noText && !c.Override.OverrideFavourite) continue; // If the cell is 'None' and has no overrides, ignore it.

            if (c.OverrideExtraLengthWeeks >= 0 && c.TempOverrideCommonLength)
                t += c.TempNewLength;
            else if (c.OverrideCommonLength)
                t += c.NewLength;
            else
                t += wd.CommonLength;
        }
        
        return t;
    }
    public bool isEmpty(int col, int weekday)
    {
        if (weekday < 0) return true;
        int wantedIndex = weekday;
        if (Grid.ColumnsList[col].IsMultirow) wantedIndex = 0;


        var c = Grid.ColumnsList[col].IsMultirow ? Grid.ColumnsList[col].Children[0].Info : Grid.ColumnsList[col].Children[wantedIndex].Info;

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
            if (Grid.ColumnsList[i].IsMultirow) index = 0; // Accounting for rowspans

            var c = Grid.ColumnsList[i].Children[index].Info;

            bool nullOverride = c.Override.EventName == "" && c.Override.Info1 == "" && c.Override.Info2 == "";
            if (nullOverride) nullOverride = c.Override.EventType < 0 && c.Override.OverrideFavourite == false;

            if (c.SelectedEventBase == 0 && nullOverride) continue; // If the cell is 'None' and has no overrides, ignore it.
            if (c.OverrideCommonLength)
                t += c.NewLength;
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
        if (!begin) return;
        long diff = (DateTime.Now - wantedTime).Ticks;
        if (diff < 0) diff = -diff;
        if (DateTime.Now >= wantedTime || diff > 30000000) // Updating content every 1 system second or if the difference is too big
        {
            wantedTime = DateTime.Now;
            wantedTime = wantedTime.AddTicks(-(wantedTime.Ticks % TimeSpan.TicksPerSecond)) + new TimeSpan(0, 0, 0, 0, 500);
            int weekdayindex = GetWeekDayIndex((int)DateTime.Now.DayOfWeek);

            Grid.CheckCellTempEvents();
            for (int i = 0; i < WeekDays.Count; i++)
            {
                WeekDays[i].CheckExpirationDate();
            }
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
            TimeLeftText.text = !TimetableEditor.instance.Editing ? LocalizationSystem.instance.GetText(gameObject.name, "NEXT_EVENT")+ " " + timeUntilNextLesson.ToString(@"mm\:ss") : "";
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

            // Making sure that we're not editing.
            w.selfButton.interactable = !TimetableEditor.instance.Editing;

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
        while (Grid.ColumnsList.Count < TimeLabels.Count)
        {
            TimeLabels.RemoveAt(TimeLabels.Count-1);
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
            TimeIndexObject ti = Instantiate(TimeIndexPrefab, TimeIndexesParent);
            
            // Making sure that we're not editing.
            ti.button.interactable = !TimetableEditor.instance.Editing;

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
        return (index+1).ToString();
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
            string tt = hours < 12 ? "AM" : "PM";
            if (hours > 12)
            {
                hours -= 12;
            }

            if (hours == 0) hours += 12;

            string extraZero = t.Minutes < 10 ? "0" : "";

            tstring = $"{hours}{punctuation}{extraZero}{t.Minutes} {tt}";
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
            // 12h Formats
            DateTime.TryParseExact(text, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "h.mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "h.mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            // 24h Formats
            DateTime.TryParseExact(text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ||
            DateTime.TryParseExact(text, "H.mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }
    public static bool TryParseLength(string hours, string minutes, out TimeSpan result)
    {
        if(int.TryParse(hours, out int h) && int.TryParse(minutes, out int m))
        {
            if (h > 23) h = 23; // No event will last 24 hours!
            if( m  > 59) m = 59;

            result = new(h, m, 0);
            return true;
        }
        result = new TimeSpan(0, 0, 0);
        return false;
    }
    
    public void Set24h(bool is24)
    {
        _24hFormat = is24;
        SaveManager.instance.SaveSettings();
        UpdateTimeIndexes();
    }
    public void SetEnglish(bool english)
    {
        EnglishFormat = english;
        SaveManager.instance.SaveSettings();
        UpdateTimeIndexes();
    }
}
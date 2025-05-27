using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class TimetableData
{
    [System.Serializable]
    public class SerializableList<T> // This makes it 10 times easier to store lists. Thank you c8theino!
    {
        public List<T> list = new List<T>();
    }

    [System.Serializable]
    public class WeekDayData
    {
        public string WeekDayName;
        public int Days;

        public int[] StartTime = new int[2] { 7, 30 }; // Hours, Minutes
        public int[] CommonLength = new int[2] { 1, 0 }; // Hours, Minutes

        public int[] OverrideDate = new int[3] { 0, 0, 0 }; // Year, Month, Day
        public int OverrideLength = 0;
        public int ExtraOverrideLengthWeeks = -1; // By default, we aren't overriding.
        public int OverrideDelayWeeks = 0;

        public int OverrideMode = 0; // 0: No Override, 1: Override StartTime, 2: Override CommonLength, 3: OverrideAll

        public int[] TempStartTime = new int[2] { 7, 30 }; // Hours, Minutes
        public int[] TempCommonLength = new int[2] { 1, 0 }; // Hours, Minutes


        public WeekDayData(WeekDay Weekday)
        {
            WeekDayName = Weekday.DayName;
            Days = (int)Weekday.Days;
            
            StartTime[0] = Weekday.StartTime.Hours;
            StartTime[1] = Weekday.StartTime.Minutes;

            CommonLength[0] = Weekday.CommonLength.Hours;
            CommonLength[1] = Weekday.CommonLength.Minutes;

            ExtraOverrideLengthWeeks = Weekday.ExtraOverrideLengthWeeks;

            if(ExtraOverrideLengthWeeks >= 0)
            {
                OverrideDate[0] = Weekday.OverrideDate.Year;
                OverrideDate[1] = Weekday.OverrideDate.Month;
                OverrideDate[2] = Weekday.OverrideDate.Day;

                OverrideLength = Weekday.OverrideLength;
                OverrideDelayWeeks = Weekday.OverrideDelayWeeks;
                OverrideMode = Weekday.OverrideMode;
                
                TempStartTime[0] = Weekday.TempStartTime.Hours;
                TempStartTime[1] = Weekday.TempStartTime.Minutes;

                TempCommonLength[0] = Weekday.TempCommonLength.Hours;
                TempCommonLength[1] = Weekday.TempCommonLength.Minutes;
            }
        }
    }

    [System.Serializable]
    public class CellInfoData
    {
        public int SelectedEvent;

        // Normal Overriding
        public string EventNameOverride;
        public string Info1Override;
        public string Info2Override;

        public int EventTypeOverride;

        public int OverrideFavourite; // 0: Don't Override, 1: No, 2: Yes

        public bool OverrideCommonLength;
        public int[] NewLength = new int[2] { 0, 0 }; // Hours, Minutes

        // Temporary Overriding

        // Expiration Stuff
        public int[] OverrideDate = new int[3] { 0, 0, 0 }; // Year, Month, Day
        public int OverrideLength = 0;
        public int ExtraOverrideLengthWeeks = -1; // By default, we aren't overriding.
        public int OverrideDelayWeeks = 0;

        // Temporary Overrides
        public int TempSelectedEvent;

        public string TempEventNameOverride;
        public string TempInfo1Override;
        public string TempInfo2Override;

        public int TempEventTypeOverride;

        public int TempOverrideFavourite; // 0: Don't Override, 1: No, 2: Yes

        public bool TempOverrideCommonLength;
        public int[] TempNewLength = new int[2] { 0, 0 }; // Hours, Minutes

        public CellInfoData(CellInfo c)
        {
            SelectedEvent = c.SelectedEventBase;

            EventNameOverride = c.Override.EventName;
            Info1Override = c.Override.Info1;
            Info2Override = c.Override.Info2;
            EventTypeOverride = c.Override.EventType;
            
            OverrideFavourite = (c.Override.OverrideFavourite ? 1 : 0) + (c.Override.OverrideFavourite && c.Override.Favourite ? 1 : 0);

            OverrideCommonLength = c.OverrideCommonLength;
            NewLength[0] = c.NewLength.Hours;
            NewLength[1] = c.NewLength.Minutes;

            if (ExtraOverrideLengthWeeks >= 0)
            {
                // Expiration Date Stuff
                OverrideDate[0] = c.OverrideDate.Year;
                OverrideDate[1] = c.OverrideDate.Month;
                OverrideDate[2] = c.OverrideDate.Day;

                OverrideLength = c.OverrideLength;
                OverrideDelayWeeks = c.OverrideDelayWeeks;

                // Temporary Overrides
                TempSelectedEvent = c.TemporaryBase;

                TempEventNameOverride = c.TemporaryOverride.EventName;
                TempInfo1Override = c.TemporaryOverride.Info1;
                TempInfo2Override = c.TemporaryOverride.Info2;
                TempEventTypeOverride = c.TemporaryOverride.EventType;
                
                TempOverrideFavourite = (c.TemporaryOverride.OverrideFavourite ? 1 : 0) + 
                    (c.TemporaryOverride.OverrideFavourite && c.TemporaryOverride.Favourite ? 1 : 0);

                TempOverrideCommonLength = c.TempOverrideCommonLength;
                TempNewLength[0] = c.TempNewLength.Hours;
                TempNewLength[1] = c.TempNewLength.Minutes;
            }
        }
    }

    [System.Serializable]
    public class ColumnData
    {
        public SerializableList<CellInfoData> children;
        public bool IsMultirow;

        public ColumnData(TimetableGrid.Column c)
        {
            IsMultirow = c.IsMultirow;
            for (int i = 0; i < c.Children.Count; i++)
            {
                children.list.Add(new(c.Children[i].Info));
            }
        }
    }

    [System.Serializable]
    public class EventTypeData
    {
        public int ItemID;
        public string TypeName;
        public float[] TextColor = new float[4] { 0, 0, 0, 1 }; // Default: Black
        public float[] BackgroundColor = new float[4] { 1, 1, 1, 1 }; // Default: White
        public EventTypeData(EventTypeItem et)
        {
            ItemID = et.ItemID;
            TypeName = et.TypeName;

            TextColor[0] = et.TextColor.r;
            TextColor[1] = et.TextColor.g;
            TextColor[2] = et.TextColor.b;
            TextColor[3] = et.TextColor.a;

            BackgroundColor[0] = et.BackgroundColor.r;
            BackgroundColor[1] = et.BackgroundColor.g;
            BackgroundColor[2] = et.BackgroundColor.b;
            BackgroundColor[3] = et.BackgroundColor.a;
        }
    }

    public string TimetableName;
    public SerializableList<EventTypeData> EventTypes;
    public SerializableList<EventItem> Events;
    public SerializableList<ColumnData> Columns;  // Use the length of this to figure out Column Count

    public SerializableList<WeekDayData> Weekdays; // Use the length of this to figure out Row Count
    public SerializableList<LabelIndex> Labels;
}
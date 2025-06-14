using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class SerializableList<T> // This makes it 10 times easier to store lists. Thank you c8theino!
{
    public List<T> list = new List<T>();
}

[System.Serializable]
public class TimetableData
{
    

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

            ExtraOverrideLengthWeeks = Weekday.OverrideExtraLengthWeeks;

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
        public WeekDayData()
        {
            WeekDayName = "";
            Days = 0;

            StartTime[0] = 7;
            StartTime[1] = 30;

            CommonLength[0] = 1;
            CommonLength[1] = 0;

            ExtraOverrideLengthWeeks = -1;

            OverrideDate[0] = DateTime.Today.Year;
            OverrideDate[1] = DateTime.Today.Month;
            OverrideDate[2] = DateTime.Today.Day;

            OverrideLength = 0;
            OverrideDelayWeeks = 0;
            OverrideMode = 0;

            TempStartTime[0] = 7;
            TempStartTime[1] = 30;

            TempCommonLength[0] = 1;
            TempCommonLength[1] = 0;
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

        public CellInfoData()
        {
            SelectedEvent = 0;

            // Normal Overriding
            EventNameOverride = "";
            Info1Override = "";
            Info2Override = "";

            EventTypeOverride = 0;

            OverrideFavourite = 0; // 0: Don't Override, 1: No, 2: Yes

            OverrideCommonLength = false;
            NewLength = new int[2] { 0, 0 }; // Hours, Minutes

            // Temporary Overriding

            // Expiration Stuff
            OverrideDate = new int[3] { 0, 0, 0 }; // Year, Month, Day
            OverrideLength = 0;
            ExtraOverrideLengthWeeks = -1; // By default, we aren't overriding.
            OverrideDelayWeeks = 0;

            // Temporary Overrides
            TempSelectedEvent = 0;

            TempEventNameOverride = "";
            TempInfo1Override = "";
            TempInfo2Override = "";

            TempEventTypeOverride = 0;

            TempOverrideFavourite = 0; // 0: Don't Override, 1: No, 2: Yes

            TempOverrideCommonLength = false;
            TempNewLength = new int[2] { 0, 0 }; // Hours, Minutes
        }
    }
    public class ExtraCellInfoData : CellInfoData
    {
        public string StartTime = "";
        public string TempStartTime = "";

        public ExtraCellInfoData()
        {
            StartTime = "";
            TempStartTime = "";
            SelectedEvent = 0;

            // Normal Overriding
            EventNameOverride = "";
            Info1Override = "";
            Info2Override = "";

            EventTypeOverride = 0;

            OverrideFavourite = 0; // 0: Don't Override, 1: No, 2: Yes

            OverrideCommonLength = false;
            NewLength = new int[2] { 0, 0 }; // Hours, Minutes

            // Temporary Overriding

            // Expiration Stuff
            OverrideDate = new int[3] { 0, 0, 0 }; // Year, Month, Day
            OverrideLength = 0;
            ExtraOverrideLengthWeeks = -1; // By default, we aren't overriding.
            OverrideDelayWeeks = 0;

            // Temporary Overrides
            TempSelectedEvent = 0;

            TempEventNameOverride = "";
            TempInfo1Override = "";
            TempInfo2Override = "";

            TempEventTypeOverride = 0;

            TempOverrideFavourite = 0; // 0: Don't Override, 1: No, 2: Yes

            TempOverrideCommonLength = false;
            TempNewLength = new int[2] { 0, 0 }; // Hours, Minutes
        }
    }

    [System.Serializable]
    public class ColumnData
    {
        public SerializableList<CellInfoData> children = new();
        public bool IsMultirow;

        public ColumnData(TimetableGrid.Column c)
        {
            children.list = new();
            IsMultirow = c.IsMultirow;
            for (int i = 0; i < c.Children.Count; i++)
            {
                children.list.Add(new(c.Children[i].Info));
            }
        }
        public ColumnData()
        {
            children = new();
            children.list = new();
            IsMultirow = false;
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
        public EventTypeData()
        {
            ItemID =0;
            TypeName = "";
            TextColor = new float[4] { 0, 0, 0, 1 };
            BackgroundColor = new float[4] { 1, 1, 1, 1 };
        }
    }

    public string TimetableName;
    public SerializableList<EventTypeData> EventTypes = new();
    public SerializableList<EventItem> Events = new();
    public SerializableList<ColumnData> Columns = new();  // Use the length of this to figure out Column Count

    public SerializableList<WeekDayData> Weekdays = new(); // Use the length of this to figure out Row Count
    public SerializableList<LabelIndex> Labels = new();

    public TimetableData()
    {
        TimetableName = "";
        EventTypes.list = new();
        Events.list = new();
        Columns.list = new();
        Weekdays.list = new();
        Labels.list = new();
    }
}
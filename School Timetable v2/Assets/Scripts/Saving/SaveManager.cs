using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;

public class SaveManager : MonoBehaviour
{
    string FilePath;
    
    string TimetablesPath = Path.AltDirectorySeparatorChar + "Timetables";
    string ThemesPath = Path.AltDirectorySeparatorChar + "Themes";

    public TimetableEditor TimetableEditor;
    public DayTimeManager DayTimeManager;
    public EventManager EventManager;

    char[] reservedChars =
        new char[]
        {
            '/','\\','*', '"','<','>',':', '|', '?', (char)0x7F, (char)0
        };
    string[] reservedNames = new string[]
    {
        "CON", "PRN", "AUX","NUL",
        "COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
        "COM\u00B9","COM\u2074","COM\u2075",
        "LPT0", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
        "LPT\u00B9","LPT\u2074","LPT\u2075",
    };
    class SaveProperties
    {
        public bool IsPortable = false;
    }
    SaveProperties saveProperties;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Application.isMobilePlatform)
        {
            StreamReader fread = new StreamReader(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + "ExtraProperties.json");
            string propertiesData = fread.ReadToEnd();
            fread.Close();

            saveProperties = JsonUtility.FromJson<SaveProperties>(propertiesData);
            if (saveProperties.IsPortable)
            {
                FilePath = System.AppDomain.CurrentDomain.BaseDirectory + Path.AltDirectorySeparatorChar + "Save Data";
                return;
            }
        }
        FilePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Save Data";

        // Load last opened timetable. If non exist, create a new timetable.

        // Load a new timetable (for now)
        LoadNewTimetable();
    }

    public void SaveTimetable()
    {
        TimetableData data = new TimetableData();
        data.TimetableName = TimetableEditor.TimetableNameText.text;

        for (int i = 1; i < EventManager.EventTypes.Count; i++)
        {
            data.EventTypes.list.Add(new(EventManager.EventTypes[i]));
        }
        data.Events.list = EventManager.Events;
        data.Events.list.RemoveAt(0); // Removing the default event made by the EventManager.

        for(int i=0; i< DayTimeManager.Grid.ColumnsList.Count; i++)
        {
            data.Columns.list.Add(new(DayTimeManager.Grid.ColumnsList[i]));
        }

        for (int i = 0; i < DayTimeManager.WeekDays.Count; i++)
        {
            data.Weekdays.list.Add(new(DayTimeManager.WeekDays[i]));
        }

        data.Labels.list = DayTimeManager.TimeLabels;
        

        // Preparing the file path.
        string fileName = removeReserved(data.TimetableName);
        string savePath = FilePath + TimetablesPath;
        ensureDirectoryExists(savePath);
        savePath += Path.AltDirectorySeparatorChar + fileName + ".json";

        // Converting to json and saving.
        string json = JsonUtility.ToJson(data, true);
        StreamWriter fwrite = new StreamWriter(savePath);
        fwrite.Write(json);
        fwrite.Close();
    }

    public void LoadTimetable(string timetable)
    {
        StreamReader fread = new StreamReader(FilePath + TimetablesPath + Path.AltDirectorySeparatorChar + timetable + ".json");
        string json = fread.ReadToEnd();
        fread.Close();

        TimetableData data = JsonUtility.FromJson<TimetableData>(json);

        TimetableEditor.TimetableNameText.text = data.TimetableName;
        TimetableEditor.TimetableNameInput.SetTextWithoutNotify(data.TimetableName + TMP_Specials.clear);

        DayTimeManager.instance.WeekDays.Clear();
        DayTimeManager.instance.TimeLabels.Clear();
        EventManager.EventTypes.Clear();
        EventManager.InitializeLists();

        for (int i = 0; i < data.EventTypes.list.Count; i++)
        {
            EventManager.EventTypes.Add(new(data.EventTypes.list[i]));
        }

        for (int i = 0; i < data.Events.list.Count; i++)
        {
            EventManager.Events.Add(data.Events.list[i]);
        }


        DayTimeManager.Grid.Columns = data.Columns.list.Count;
        DayTimeManager.Grid.Rows = data.Weekdays.list.Count;

        DayTimeManager.Grid.Setup();

        for (int i = 0; i < data.Columns.list.Count; i++)
        {
            if (data.Columns.list[i].IsMultirow)
            {
                DayTimeManager.Grid.ReplaceColumnWithMultirowAt(i);
            }
            for(int j = 0; j < data.Columns.list[i].children.list.Count; j++)
            {
                DayTimeManager.instance.Grid.ColumnsList[i].Children[j].Info = 
                    new(DayTimeManager.instance.Grid.ColumnsList[i].Children[j], data.Columns.list[i].children.list[j]);
            }
        }

        for (int i = 0; i < data.Weekdays.list.Count; i++)
        {
            DayTimeManager.WeekDays.Add(new(data.Weekdays.list[i]));
        }
        DayTimeManager.TimeLabels = data.Labels.list;

        TimetableEditor.instance.Setup();
        DayTimeManager.Grid.UpdateAllCells();
        DayTimeManager.Highlight.transform.SetAsLastSibling();
        DayTimeManager.UpdateWeekDays();
        DayTimeManager.UpdateTimeIndexes();
    }

    public void DeleteTimetable(string timetable)
    {

    }

    public void LoadNewTimetable()
    {
        EventManager.InitializeLists();

        // Default week days:                 SFTWTMS
        // 0000001 = 01 Sun -> Since DateTime.DayOfWeek uses Sunday as 0, I too have to use sunday as 0.
        DayTimeManager.WeekDays.Add(new("Monday", 2)); // 0000010 = 02 Mon
        DayTimeManager.WeekDays.Add(new("Tuesday", 4)); // 0000100 = 04 Tue
        DayTimeManager.WeekDays.Add(new("Wednesday", 8)); // 0001000 = 08 Wed
        DayTimeManager.WeekDays.Add(new("Thursday", 16)); // 0010000 = 16 Thu
        DayTimeManager.WeekDays.Add(new("Friday", 32)); // 0100000 = 32 Fri

        // Set Grid rows to weekdays count.
        DayTimeManager.Grid.Rows = DayTimeManager.WeekDays.Count;
        DayTimeManager.Grid.Setup();
        TimetableEditor.instance.Setup();
        DayTimeManager.Grid.UpdateAllCells();
        DayTimeManager.Highlight.transform.SetAsLastSibling();
        DayTimeManager.UpdateWeekDays();
        DayTimeManager.UpdateTimeIndexes();
    }

    void ensureDirectoryExists(string dir)
    {
        FileInfo fi = new FileInfo(dir);
        if (!fi.Directory.Exists)
        {
            Directory.CreateDirectory(dir);
        }
    }
    string removeReserved(string text)
    {
        string newString = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (reservedChars.Contains(text[i]))
            {
                newString += '_';
                continue;
            }
            newString += text[i];
        }
        for (int i = 0; i < reservedNames.Length; i++)
        {
            if (newString.StartsWith(reservedNames[i]))
            {
                newString.Insert(0, "_");
            }
        }
        return newString;
    }
}
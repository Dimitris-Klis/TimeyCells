using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private void Awake()
    {
        instance = this;
    }
    string FilePath;
    
    string TimetablesPath = Path.AltDirectorySeparatorChar + "Timetables";
    string SettingsPath = Path.AltDirectorySeparatorChar + "Settings.json";

    public TimetableEditor TimetableEditor;
    public DayTimeManager DayTimeManager;
    public EventManager EventManager;
    public ColorStylizer Stylizer;
    [Space]
    public GameObject OpenOverlay;
    public TimetableButton TimetableButtonPrefab;
    public Transform ButtonsParent;
    List<TimetableButton> Buttons = new();

    string LastTimetable;

    static bool saved;

    public static void ChangesMade()
    {
        saved = false;
    }

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
        LoadSettings();
        LoadTimetable(LastTimetable, false);
        LoadButtons();
    }

    public void LoadButtons()
    {
        ensureDirectoryExists(FilePath + TimetablesPath);
        FileInfo fi = new(FilePath + TimetablesPath);
        string[] filepaths = Directory.GetFiles(FilePath + TimetablesPath);

        // I need to figure out how to load the last file opened.

        for (int i = 0; i < Buttons.Count; i++)
        {
            Destroy(Buttons[i].gameObject);
        }
        Buttons.Clear();

        if (filepaths.Length == 0) return;

        for (int i = 0; i < filepaths.Length; i++)
        {
            string filename = Path.GetFileNameWithoutExtension(filepaths[i]);
            TimetableButton b = Instantiate(TimetableButtonPrefab, ButtonsParent);
            b.Text.text = filename;

            string f = filename;
            b.Self.onClick.AddListener(delegate {LoadTimetable(f, true); OpenOverlay.gameObject.SetActive(false); });
            b.DeleteButton.onClick.AddListener(delegate { DeleteTimetable(f, false); });
            Buttons.Add(b);
        }
    }
    public void CopyTimetableAsJson()
    {
        TimetableData data = new();

        data.TimetableName = TimetableEditor.TimetableNameText.text;

        for (int i = 0; i < EventManager.EventTypes.Count; i++)
        {
            data.EventTypes.list.Add(new(EventManager.EventTypes[i]));
        }
        data.Events.list = new(EventManager.Events);
        data.Events.list.RemoveAt(0); // Removing the default event made by the EventManager.

        for (int i = 0; i < DayTimeManager.Grid.ColumnsList.Count; i++)
        {
            data.Columns.list.Add(new(DayTimeManager.Grid.ColumnsList[i]));
        }

        for (int i = 0; i < DayTimeManager.WeekDays.Count; i++)
        {
            data.Weekdays.list.Add(new(DayTimeManager.WeekDays[i]));
        }

        data.Labels.list = DayTimeManager.TimeLabels;

        // By disabling pretty print, less memory is required for copying.
        GUIUtility.systemCopyBuffer = JsonUtility.ToJson(data, false);
    }
    public void PasteJsonAsTimetable(bool checkSave)
    {
        DayTimeManager.begin = false;
        if (!saved && checkSave)
        {
            ConfirmationManager.ButtonPrompt Cancel = new("Cancel", null);
            ConfirmationManager.ButtonPrompt Continue = new("Don't Save", delegate { PasteJsonAsTimetable(false); });
            ConfirmationManager.ButtonPrompt Save = new("Save & Continue", delegate { SaveTimetable(); PasteJsonAsTimetable(false); });


            ConfirmationManager.Instance.ShowConfirmation("Unsaved work!", "You have unsaved work! Would you like to save first?",
                Cancel, Continue, Save);
            return;
        }

        // Loading the data.
        string json = GUIUtility.systemCopyBuffer;

        TimetableData data = null;

        // This is meant to prevent any weird behaviour if you paste anything other than a json file.
        try
        {
            data = JsonUtility.FromJson<TimetableData>(json);
        }
        catch(Exception)
        {
            return;
        }
        if (data == null)
        {
            // TO DO: Add some backwards compatability - convert old timetables into new ones!

            return;
        }
        // Using the data.
        TimetableEditor.TimetableNameText.text = data.TimetableName;
        TimetableEditor.TimetableNameInput.SetTextWithoutNotify(data.TimetableName + TMP_Specials.clear);

        DayTimeManager.instance.WeekDays.Clear();
        DayTimeManager.instance.TimeLabels.Clear();
        EventManager.EventTypes.Clear();

        // We also store the colors of the Default event, so we call this before IntializingLists.
        for (int i = 0; i < data.EventTypes.list.Count; i++)
        {
            EventManager.EventTypes.Add(new(data.EventTypes.list[i]));
        }

        EventManager.InitializeLists();

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
            for (int j = 0; j < data.Columns.list[i].children.list.Count; j++)
            {
                DayTimeManager.instance.Grid.ColumnsList[i].Children[j].Info.SetupSelf(data.Columns.list[i].children.list[j]);
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

        DayTimeManager.begin = true;
        saved = true;
        Stylizer.Setup();
    }
    public void SaveTimetable()
    {
        TimetableData data = new();

        data.TimetableName = TimetableEditor.TimetableNameText.text;

        for (int i = 0; i < EventManager.EventTypes.Count; i++)
        {
            data.EventTypes.list.Add(new(EventManager.EventTypes[i]));
        }
        data.Events.list = new(EventManager.Events);
        data.Events.list.RemoveAt(0); // Removing the default event made by the EventManager.

        for (int i = 0; i < DayTimeManager.Grid.ColumnsList.Count; i++)
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

        LastTimetable = fileName;

        // Converting to json and saving.
        string json = JsonUtility.ToJson(data, true);
        StreamWriter fwrite = new StreamWriter(savePath);
        fwrite.Write(json);
        fwrite.Close();

        saved = true;
        LoadButtons();
    }

    public void LoadTimetable(string timetable, bool checkSave)
    {
        ensureDirectoryExists(FilePath + TimetablesPath);
        DayTimeManager.begin = false;
        if (!saved && checkSave)
        {
            ConfirmationManager.ButtonPrompt Cancel = new("Cancel", null);
            ConfirmationManager.ButtonPrompt Continue = new("Don't Save", delegate { LoadTimetable(timetable, false); });
            ConfirmationManager.ButtonPrompt Save = new("Save & Continue", delegate { SaveTimetable(); LoadTimetable(timetable, false); });


            ConfirmationManager.Instance.ShowConfirmation("Unsaved work!", "You have unsaved work! Would you like to save first?",
                Cancel, Continue, Save);
            return;
        }

        // Loading a new timetable, if the timetable we load doesn't exist!
        FileInfo fi = new FileInfo(FilePath + TimetablesPath + Path.AltDirectorySeparatorChar + timetable + ".json");
        if (!fi.Exists)
        {
            LoadNewTimetable(false);
            return;
        }

        // Loading the data.
        StreamReader fread = new StreamReader(FilePath + TimetablesPath + Path.AltDirectorySeparatorChar + timetable + ".json");
        string json = fread.ReadToEnd();
        fread.Close();

        TimetableData data = JsonUtility.FromJson<TimetableData>(json);


        // Using the data.
        TimetableEditor.TimetableNameText.text = data.TimetableName;
        TimetableEditor.TimetableNameInput.SetTextWithoutNotify(data.TimetableName + TMP_Specials.clear);
        
        LastTimetable = timetable;

        DayTimeManager.instance.WeekDays.Clear();
        DayTimeManager.instance.TimeLabels.Clear();
        EventManager.EventTypes.Clear();

        // We also store the colors of the Default event, so we call this before IntializingLists.
        for (int i = 0; i < data.EventTypes.list.Count; i++)
        {
            EventManager.EventTypes.Add(new(data.EventTypes.list[i]));
        }

        EventManager.InitializeLists();

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
                DayTimeManager.instance.Grid.ColumnsList[i].Children[j].Info.SetupSelf(data.Columns.list[i].children.list[j]);
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

        DayTimeManager.begin = true;
        saved = true;
        Stylizer.Setup();
    }

    public void DeleteTimetable(string timetable, bool confirm)
    {
        if (!confirm)
        {
            ConfirmationManager.ButtonPrompt Cancel = new("Cancel", null);
            ConfirmationManager.ButtonPrompt Confirm = new("Delete", delegate { DeleteTimetable(timetable, true); });
            ConfirmationManager.Instance.ShowConfirmation
            (
                "Are you sure?", $"Are you sure you want to delete the timetable '{timetable}'?",
                Cancel, Confirm
            );
            return;
        }
        string path = FilePath + TimetablesPath + Path.AltDirectorySeparatorChar + timetable + ".json";

        File.Delete(path);

        if (timetable == TimetableEditor.TimetableNameText.text) saved = false;
        LoadButtons();
    }

    public void LoadNewTimetable(bool checkSave)
    {
        TimetableEditor.TimetableNameText.text = "New Timetable";
        TimetableEditor.TimetableNameInput.SetTextWithoutNotify("New Timetable" + TMP_Specials.clear);
        DayTimeManager.begin = false;
        if (!saved && checkSave)
        {
            ConfirmationManager.ButtonPrompt Cancel = new("Cancel", null);
            ConfirmationManager.ButtonPrompt Continue = new("Don't Save", delegate { LoadNewTimetable(false); });
            ConfirmationManager.ButtonPrompt Save = new("Save and Continue", delegate { SaveTimetable(); LoadNewTimetable(false); });
            
            
            ConfirmationManager.Instance.ShowConfirmation("Unsaved work!", "You have unsaved work! Would you like to save first?",
                Cancel, Continue, Save);
            return;
        }
        EventManager.InitializeLists();
        DayTimeManager.WeekDays.Clear();

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
        
        DayTimeManager.Grid.UpdateAllCells();
        TimetableEditor.instance.Setup();
        DayTimeManager.Highlight.transform.SetAsLastSibling();
        DayTimeManager.UpdateWeekDays();
        DayTimeManager.UpdateTimeIndexes();
        DayTimeManager.begin = true;
        saved = true;

        SaveSettings();
        Stylizer.Setup();
    }

    public void SaveSettings()
    {
        SettingsData settingsData = new SettingsData();
        settingsData.Use24HFormat = DayTimeManager._24hFormat;
        settingsData.UseEnglishFormat = DayTimeManager.EnglishFormat;

        settingsData.CurrentTheme = Stylizer.wantedPreset;

        for (int i = 0; i < Stylizer.ColorStyles.Count; i++)
        {
            if (Stylizer.ColorStyles[i].IsCustomPreset)
            {
                settingsData.CustomThemes.list.Add(new(Stylizer.ColorStyles[i]));
            }
        }
        string json = JsonUtility.ToJson(settingsData, true);
        StreamWriter fwrite = new StreamWriter(FilePath + SettingsPath);

        fwrite.Write(json);

        fwrite.Close();
    }
    public void LoadSettings()
    {
        ensureDirectoryExists(FilePath);

        // Loading default settings if they don't exist.
        FileInfo fi = new FileInfo(FilePath + SettingsPath);
        if (!fi.Exists)
        {
            SaveSettings();
        }

        // Loading the data.
        StreamReader fread = new StreamReader(FilePath + SettingsPath);
        string json = fread.ReadToEnd();
        fread.Close();
        SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);


        // Using the data.
        LastTimetable = settingsData.LastOpenedTimetable;


        DayTimeManager._24hFormat = settingsData.Use24HFormat;
        DayTimeManager.EnglishFormat = settingsData.UseEnglishFormat;

        DayTimeManager._24hToggle.isOn = settingsData.Use24HFormat;
        DayTimeManager.EnglishToggle.isOn = settingsData.UseEnglishFormat;

        for (int i = 0; i < Stylizer.ColorStyles.Count; i++)
        {
            if (Stylizer.ColorStyles[i].IsCustomPreset)
            {
                Stylizer.ColorStyles.RemoveAt(i);
            }
        }
        for (int i = 0; i < settingsData.CustomThemes.list.Count; i++)
        {
            
            Stylizer.ColorStyles.Add(new(settingsData.CustomThemes.list[i]));
        }
        
        Stylizer.wantedPreset = settingsData.CurrentTheme;
        Stylizer.Setup();
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
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.UI;

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
    public LocalizationSystem LocalizationSystem;
    [Space]
    public GameObject OpenOverlay;
    public TimetableButton TimetableButtonPrefab;
    public Transform ButtonsParent;
    List<TimetableButton> Buttons = new();
    public S_ProgramData debug_olddata;
    public TimetableData debug_newdata;
    string LastTimetable;
    public Image UnsavedIndicator;

    static bool saved;

    public void ChangesMade()
    {
        saved = false;
        UnsavedIndicator.enabled = true;
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
        FilePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Save Data";
        if (!Application.isMobilePlatform)
        {
            StreamReader fread = new StreamReader(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + "ExtraProperties.json");
            string propertiesData = fread.ReadToEnd();
            fread.Close();

            saveProperties = JsonUtility.FromJson<SaveProperties>(propertiesData);
            if (saveProperties.IsPortable)
            {
                FilePath = System.AppDomain.CurrentDomain.BaseDirectory + Path.AltDirectorySeparatorChar + "Save Data";
            }
        }

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
            b.Self.onClick.AddListener(delegate {LoadTimetable(f, true); });
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
            ConfirmationManager.ButtonPrompt Cancel = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_CANCEL"), null);
            ConfirmationManager.ButtonPrompt Continue = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_DONTSAVE"), delegate { PasteJsonAsTimetable(false); });
            ConfirmationManager.ButtonPrompt Save = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_SAVECONTINUE"), delegate { SaveTimetable(); PasteJsonAsTimetable(false); });


            ConfirmationManager.Instance.ShowConfirmation(LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_TITLE"),
                LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_DESC"),
                Cancel, Continue, Save);
            return;
        }

        // Loading the data.
        string json = GUIUtility.systemCopyBuffer;

        TimetableData data = null;
        bool foundolddata = false;
        try
        {
            // Converting old timetables from School Timetable into new ones!
            S_ProgramData olddata = JsonUtility.FromJson<S_ProgramData>(json);
            debug_olddata = olddata;
            if (olddata != null)
            {
                data = ConvertOldDataToNew(olddata);
                foundolddata = true;
            }
        }
        catch (Exception)
        {

        }

        // This is meant to prevent any weird behaviour if you paste anything other than a json file.
        if (!foundolddata)
        {
            try
            {
                data = JsonUtility.FromJson<TimetableData>(json);
            }
            catch (Exception)
            {
                LoadNewTimetable(false);
                return;
            }
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
        DayTimeManager.UpdateTimeIndexes();
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
        saved = false;
        Stylizer.Setup();
    }
    int[] oldCellOrder = new int[]
    { 
        0, 30, 10, 20, 35, 5, 15, 25, 
        31, 21, 11, 1, 36, 26, 16, 6, 
        32, 12, 2, 22, 7, 37, 27, 17, 
        33, 3, 13, 23, 8, 18, 38, 28, 
        4, 14, 34, 24, 29, 9, 19, 39
    };
    public TimetableData ConvertOldDataToNew(S_ProgramData old_data)
    {
        TimetableData newdata = new();

        S_ProgramData olddata = SortOldCells(old_data);

        TimetableData.EventTypeData TestedType = new ();
        TimetableData.EventTypeData TestedMovingType = new();

        TimetableData.EventTypeData UntestedType = new();
        TimetableData.EventTypeData UntestedMovingType = new();

        TimetableData.EventTypeData GymType = new();
        TimetableData.EventTypeData SupportType = new();

        TimetableData.EventTypeData BreakType = new();

        TestedType.ItemID = 1;
        TestedType.TypeName = "Examined Subject";
        TestedType.BackgroundColor = new float[] { 0.7803921568627451f, 0.8627450980392157f, 0.8156862745098039f, 1f };
        TestedType.TextColor = new float[] {0f, 0f, 0f, 1f };

        TestedMovingType.ItemID = 2;
        TestedMovingType.TypeName = "Examined Subject - Different Class";
        TestedMovingType.BackgroundColor = new float[] { 0.7803921568627451f, 0.8627450980392157f, 0.8156862745098039f, 1f };
        TestedMovingType.TextColor = new float[] { 0.3019607961177826f, 0.3960784375667572f, 0.7058823704719543f, 1f };

        UntestedType.ItemID = 3;
        UntestedType.TypeName = "Non Examined Subject";
        UntestedType.BackgroundColor = new float[] { 0.3294117748737335f, 0.4941176474094391f, 0.3921568691730499f, 1f };
        UntestedType.TextColor = new float[] { 0f, 0f, 0f, 1f };

        UntestedMovingType.ItemID = 4;
        UntestedMovingType.TypeName = "Non Examined Subject - Different Class";
        UntestedMovingType.BackgroundColor = new float[] { 0.3294117748737335f, 0.4941176474094391f, 0.3921568691730499f, 1f };
        UntestedMovingType.TextColor = new float[] { 0.8352941274642944f, 0.8784313797950745f, 0.2941176593303680f, 1f };

        GymType.ItemID = 5;
        GymType.TypeName = "Exercise Subject";
        GymType.BackgroundColor = new float[] { 0.5607843399047852f, 0.8274509906768799f, 1f, 1f };
        GymType.TextColor = new float[] { 0f, 0f, 0f, 1f };

        SupportType.ItemID = 6;
        SupportType.TypeName = "Extra Lesson";
        SupportType.BackgroundColor = new float[] { 0.5647059082984924f, 0.3686274588108063f, 0.6627451181411743f, 1f };
        SupportType.TextColor = new float[] { 0f, 0f, 0f, 1f };

        BreakType.ItemID = 7;
        BreakType.TypeName = "Break";
        BreakType.BackgroundColor = new float[] { 0.2431372553110123f, 0.2078431397676468f, 0.2745098173618317f, 1f };
        BreakType.TextColor = new float[] { 1f, 1f, 1f, 1f };

        newdata.EventTypes.list.Add(new(EventManager.DefaultNewEventType));
        newdata.EventTypes.list.Add(TestedType);
        newdata.EventTypes.list.Add(TestedMovingType);

        newdata.EventTypes.list.Add(UntestedType);
        newdata.EventTypes.list.Add(UntestedMovingType);

        newdata.EventTypes.list.Add(GymType);
        newdata.EventTypes.list.Add(SupportType);
        newdata.EventTypes.list.Add(BreakType);

        for (int i = 0; i < 8; i++)
        {
            newdata.Columns.list.Add(new());
            newdata.Columns.list[i].children = new();
            // Debug.Log($"{i * olddata._7hDays.Length}<{i * olddata._7hDays.Length + olddata._7hDays.Length}");
            for(int j = i* olddata._7hDays.Length; j < i * olddata._7hDays.Length + olddata._7hDays.Length; j++)
            {
                TimetableData.CellInfoData c = new();
                
                c.EventNameOverride = olddata.Cells[j].LessonName;
                c.Info1Override = olddata.Cells[j].RoomIndex;
                c.Info2Override = olddata.Cells[j].TeacherName;
                
                c.OverrideFavourite = 1 + (olddata.Cells[j].Favourite ? 1 : 0);
                switch (olddata.Cells[j].LessonType)
                {
                    case 0:
                        c.EventTypeOverride = olddata.Cells[j].Tested ? 1 : 3;
                        break;
                    case 1:
                        c.EventTypeOverride = olddata.Cells[j].Tested ? 2 : 4;
                        break;
                    case 2:
                        c.EventTypeOverride = 5;
                        break;
                    case 3:
                        c.EventTypeOverride = 6;
                        break;
                }
                
                newdata.Columns.list[i].children.list.Add(c);
            }
        }
        for(int i = 0; i < olddata.BreakLengths_7h.Length; i++)
        {
            TimetableData.ColumnData ColToAdd = new();
            ColToAdd.children = new();
            ColToAdd.children.list = new();

            if (olddata.BreakLengths_7h[i] == olddata.BreakLengths_8h[i])
            {
                ColToAdd.IsMultirow = true;

                TimetableData.CellInfoData c = new();

                c.EventNameOverride = $"Break {i+1}";
                c.Info1Override = c.Info2Override = "";
                c.OverrideFavourite = 0;
                c.EventTypeOverride = 7;
                c.OverrideCommonLength = true;
                c.NewLength[0] = 0;
                c.NewLength[1] = olddata.BreakLengths_8h[i];

                ColToAdd.children.list.Add(c);
            }
            else
            {
                for (int d = 0; d < olddata._7hDays.Length; d++)
                {
                    TimetableData.CellInfoData c = new();

                    c.EventNameOverride = $"Break {i + 1}";
                    c.Info1Override = c.Info2Override = "";
                    c.OverrideFavourite = 0;
                    c.EventTypeOverride = 7;
                    c.OverrideCommonLength = true;

                    if (olddata._7hDays[d])
                    {
                        c.NewLength[0] = 0;
                        c.NewLength[1] = olddata.BreakLengths_7h[i];
                    }
                    else
                    {
                        c.NewLength[0] = 0;
                        c.NewLength[1] = olddata.BreakLengths_8h[i];
                    }
                    ColToAdd.children.list.Add(c);
                }
            }
            newdata.Columns.list.Insert(((i + 1) * 3) - 1, ColToAdd);
        }

        for (int i = 0; i < olddata._7hDays.Length; i++)
        {
            TimetableData.WeekDayData wdata = new();
            wdata.WeekDayName = ((DayOfWeek)i + 1).ToString();
            wdata.CommonLength[0] = 0;
            wdata.Days = (int)Mathf.Pow(2, i + 1);
            wdata.StartTime = olddata.StartTime;
            
            if (olddata._7hDays[i])
            {
                wdata.CommonLength[1] = olddata._7hDuration;
            }
            else
            {
                wdata.CommonLength[1] = olddata._8hDuration;
            }

            newdata.Weekdays.list.Add(wdata);
        }
        newdata.Labels = new();
        newdata.Labels.list = new();
        for (int i =0; i<newdata.Columns.list.Count; i++)
        {
            LabelIndex label = new();
            label.CountAsIndex = false;
            label.IsCustomLabel = newdata.Columns.list[i].IsMultirow;
            if (label.IsCustomLabel) label.CustomLabelName = "Break";

            newdata.Labels.list.Add(label);
        }
        newdata.TimetableName = olddata.FileName;

        debug_newdata = newdata;
        return newdata;
    }
    public S_ProgramData SortOldCells(S_ProgramData olddata)
    {
        List<S_ProgramData.LessonCellData> sortedcells = new(olddata.Cells);
        for (int i = 0; i < olddata.Cells.Count; i++)
        {
            sortedcells[oldCellOrder[i]] = olddata.Cells[i];
        }
        olddata.Cells = sortedcells;
        return olddata;
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
        UnsavedIndicator.enabled=false;
        LoadButtons();
    }

    public void LoadTimetable(string timetable, bool checkSave)
    {
        ensureDirectoryExists(FilePath + TimetablesPath + Path.AltDirectorySeparatorChar);
        DayTimeManager.begin = false;
        if (!saved && checkSave)
        {
            ConfirmationManager.ButtonPrompt Cancel = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_CANCEL"), null);
            ConfirmationManager.ButtonPrompt Continue = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_DONTSAVE"), delegate { LoadTimetable(timetable, false); });
            ConfirmationManager.ButtonPrompt Save = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_SAVECONTINUE"), delegate { SaveTimetable(); LoadTimetable(timetable, false); });


            ConfirmationManager.Instance.ShowConfirmation(LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_TITLE"),
                LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_DESC"),
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
        EventManager.Events.Clear();

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
        DayTimeManager.UpdateTimeIndexes();

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
        UnsavedIndicator.enabled = false;

        Stylizer.Setup();

        EventManager.UpdateEventPreviews(false);
        EventManager.UpdateEventTypePreviews(true);

        OpenOverlay.gameObject.SetActive(false);
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

        if (timetable == TimetableEditor.TimetableNameText.text)
        {
            saved = false;
            UnsavedIndicator.enabled = true;
        }
        LoadButtons();
    }

    public void LoadNewTimetable(bool checkSave)
    {
        TimetableEditor.TimetableNameText.text = "New Timetable";
        TimetableEditor.TimetableNameInput.SetTextWithoutNotify("New Timetable" + TMP_Specials.clear);
        DayTimeManager.begin = false;
        if (!saved && checkSave)
        {
            ConfirmationManager.ButtonPrompt Cancel = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_CANCEL"), null);
            ConfirmationManager.ButtonPrompt Continue = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_DONTSAVE"), delegate { LoadNewTimetable(false); });
            ConfirmationManager.ButtonPrompt Save = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_SAVECONTINUE"), delegate { SaveTimetable(); LoadNewTimetable(false); });


            ConfirmationManager.Instance.ShowConfirmation(LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_TITLE"),
                LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_DESC"),
                Cancel, Continue, Save);
            return;
        }
        DayTimeManager.instance.WeekDays.Clear();
        DayTimeManager.instance.TimeLabels.Clear();
        EventManager.EventTypes.Clear();
        EventManager.Events.Clear();
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
        
        DayTimeManager.Grid.UpdateAllCells();
        TimetableEditor.instance.Setup();
        DayTimeManager.Highlight.transform.SetAsLastSibling();
        DayTimeManager.UpdateWeekDays();
        DayTimeManager.UpdateTimeIndexes();
        DayTimeManager.begin = true;

        saved = true;
        UnsavedIndicator.enabled = false;

        SaveSettings();
        Stylizer.Setup();
    }

    public void SaveSettings()
    {
        ensureDirectoryExists(FilePath);

        SettingsData settingsData = new SettingsData();
        settingsData.Use24HFormat = DayTimeManager._24hFormat;
        settingsData.UseEnglishFormat = DayTimeManager.EnglishFormat;

        settingsData.CurrentTheme = Stylizer.wantedPreset;
        settingsData.SelectedLanguage = LocalizationSystem.SelectedLanguage;

        settingsData.LastOpenedTimetable = LastTimetable;

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
            SaveSettings(); // Saving the default settings.
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
        LocalizationSystem.Setup();
        LocalizationSystem.SetLanguage(settingsData.SelectedLanguage);
    }

    void ensureDirectoryExists(string dir)
    {
        if (!Directory.Exists(dir))
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

    public void Quit(bool checkSave)
    {
        if (!saved && checkSave)
        {
            ConfirmationManager.ButtonPrompt Cancel = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_CANCEL"), null);
            ConfirmationManager.ButtonPrompt Continue = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_DONTSAVE"), delegate { Quit(false); });
            ConfirmationManager.ButtonPrompt Save = new(LocalizationSystem.GetText(gameObject.name, "BUTTONS_SAVEQUIT"), delegate { SaveTimetable(); Quit(false); });

            ConfirmationManager.Instance.ShowConfirmation(LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_TITLE"),
                LocalizationSystem.GetText(gameObject.name, "WARNING_UNSAVED_DESC"),
                Cancel, Continue, Save);
            return;
        }
        Application.Quit();
    }
}
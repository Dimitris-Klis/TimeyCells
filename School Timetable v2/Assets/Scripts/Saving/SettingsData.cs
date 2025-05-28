using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    [System.Serializable]
    public class CustomThemeData
    {
        public string ThemeName;

        public float[] PrimaryColor = new float[4] { 1, 1, 1, 0 };
        public float[] SecondaryColor = new float[4] { 1, 1, 1, 0 };
        public float[] BackgroundColor = new float[4] { 1, 1, 1, 0 };

        public CustomThemeData(ColorStylePreset cs)
        {
            ThemeName = cs.PaletteName;

            PrimaryColor[0] = cs.PrimaryColor.r;
            PrimaryColor[1] = cs.PrimaryColor.g;
            PrimaryColor[2] = cs.PrimaryColor.b;
            PrimaryColor[3] = cs.PrimaryColor.a;

            SecondaryColor[0] = cs.SecondaryColor.r;
            SecondaryColor[1] = cs.SecondaryColor.g;
            SecondaryColor[2] = cs.SecondaryColor.b;
            SecondaryColor[3] = cs.SecondaryColor.a;

            BackgroundColor[0] = cs.BackgroundColor.r;
            BackgroundColor[1] = cs.BackgroundColor.g;
            BackgroundColor[2] = cs.BackgroundColor.b;
            BackgroundColor[3] = cs.BackgroundColor.a;
        }
    }

    public string LastOpenedTimetable;
    
    public bool Use24HFormat;
    public bool UseEnglishFormat;
    
    public SerializableList<CustomThemeData> CustomThemes = new();
    public int CurrentTheme;

    public SettingsData()
    {
        LastOpenedTimetable = "";

        Use24HFormat = false;
        UseEnglishFormat = false;

        CustomThemes = new();
        CustomThemes.list = new();
        CurrentTheme = 0;
    }
}
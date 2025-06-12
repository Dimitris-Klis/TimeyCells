using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class ColorStylePreset
{
    public string PaletteName;
    [Space]
    public Color PrimaryColor; // Buttons
    public Color SecondaryColor; // Buttons Background
    public Color BackgroundColor; // Camera Background
    [Space]
    public bool IsCustomPreset;

    public ColorStylePreset(SettingsData.CustomThemeData themeData)
    {
        PaletteName = themeData.ThemeName;

        PrimaryColor = new(themeData.PrimaryColor[0], themeData.PrimaryColor[1], themeData.PrimaryColor[2], themeData.PrimaryColor[3]);
        SecondaryColor = new(themeData.SecondaryColor[0], themeData.SecondaryColor[1], themeData.SecondaryColor[2], themeData.SecondaryColor[3]);
        BackgroundColor = new(themeData.BackgroundColor[0], themeData.BackgroundColor[1], themeData.BackgroundColor[2], themeData.BackgroundColor[3]);

        IsCustomPreset = true;
    }
    public ColorStylePreset()
    {
        PaletteName = "";
        PrimaryColor = Color.white;
        SecondaryColor = Color.white;
        BackgroundColor = Color.white;
        IsCustomPreset = false;
    }
}

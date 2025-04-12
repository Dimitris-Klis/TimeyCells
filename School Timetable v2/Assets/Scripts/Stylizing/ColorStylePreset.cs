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
}

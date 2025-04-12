using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaletteObject : MonoBehaviour
{

    [Header("Functionality")]
    public PaletteDropdown paletteDropdown;
    public int paletteIndex;
    public Toggle toggle;

    [Space(20)]
    [Header("GFX")]
    public TMP_Text PaletteNameText;
    public Image BackgroundColorImage;
    public Image SecondaryColorImage;
    public Image PrimaryColorImage;


    public void SetPaletteDropdown(bool isOn)
    {
        if (isOn)
        {
            paletteDropdown.ChangeValue(paletteIndex);
        }
    }
}
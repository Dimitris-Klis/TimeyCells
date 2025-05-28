using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorStylizer : MonoBehaviour
{
    public int wantedPreset;
    
    [Space]
    public PaletteDropdown paletteDropdown;
    public List<ColorStylePreset> ColorStyles;

    [Space]
    public Camera Camera;
    public Image[] Backgrounds;
    public Image[] Buttons;
    public TMP_Text[] Texts;

    public void Setup()
    {
        UpdateDropdown();
        GetElements();
        RefreshPreset();
    }
    public int GetIndex(ColorStylePreset preset)
    {
        for (int i = 0; i < ColorStyles.Count; i++)
        {
            if (ColorStyles[i] == preset) return i;
        }
        return -1;
    }

    public void DeleteStyle(int index)
    {
        ColorStyles.RemoveAt(index);
        if (wantedPreset > index) wantedPreset--;
        if (wantedPreset >= ColorStyles.Count)
        {
            wantedPreset = 0;
        }
        RefreshPreset();
        UpdateDropdown();
    }

    public void ChangePreset(int index)
    {
        wantedPreset = index;
        RefreshPreset();
        SaveManager.instance.SaveSettings();
    }

    [ContextMenu("Get Relevant Elements")]
    public void GetElements()
    {
        List<GameObject> InactiveObjects = new();
        foreach (GameObject o in FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (!o.activeSelf)
            {
                InactiveObjects.Add(o.gameObject);
                o.gameObject.SetActive(true);
            }
        }
        GameObject[] ButtonObjects = GameObject.FindGameObjectsWithTag("Styled/Button");
        GameObject[] BGObjects = GameObject.FindGameObjectsWithTag("Styled/Background");
        GameObject[] TextObjects = GameObject.FindGameObjectsWithTag("Styled/Text");

        Buttons = new Image[ButtonObjects.Length];
        Backgrounds = new Image[BGObjects.Length];
        Texts = new TMP_Text[TextObjects.Length];

        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i] = ButtonObjects[i].GetComponent<Image>();
        }

        for (int i = 0; i < Backgrounds.Length; i++)
        {
            Backgrounds[i] = BGObjects[i].GetComponent<Image>();
        }

        for (int i = 0; i < Texts.Length; i++)
        {
            Texts[i] = TextObjects[i].GetComponent<TMP_Text>();
        }

        foreach(GameObject o in InactiveObjects)
        {
            o.SetActive(false);
        }
    }
    public void UpdateDropdown()
    {
        paletteDropdown.Setup(ColorStyles.ToArray());
        paletteDropdown.SetValueWithoutNotify(wantedPreset);
        GetElements();
        RefreshPreset();
    }
    [ContextMenu("Refresh Preset")]
    public void RefreshPreset()
    {
        if (wantedPreset >= ColorStyles.Count) wantedPreset = 0;

        // The Selected Preset
        ColorStylePreset preset = ColorStyles[wantedPreset];

        // Background
        Camera.backgroundColor = preset.BackgroundColor;

        // UI Backgrounds
        for (int i = 0; i < Backgrounds.Length; i++)
        {
            if (Backgrounds[i] == null) continue;
            Backgrounds[i].color = preset.SecondaryColor;
        }

        // Texts
        for (int i = 0; i < Texts.Length; i++)
        {
            if (Texts[i] == null) continue;
            Texts[i].color = preset.SecondaryColor;
        }

        // Buttons
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (Buttons[i] == null) continue;
            Buttons[i].color = preset.PrimaryColor;
        }
    }
}
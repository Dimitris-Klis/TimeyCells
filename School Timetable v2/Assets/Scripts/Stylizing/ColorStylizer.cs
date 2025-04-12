using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;

public class ColorStylizer : MonoBehaviour
{
    public int wantedIndex;
    
    [Space]
    public PaletteDropdown paletteDropdown;
    public List<ColorStylePreset> ColorStyles;

    [Space]
    public Camera Camera;
    public Image[] Backgrounds;
    public Image[] Buttons;
    public TMP_Text[] Texts;

    private void Start()
    {
        UpdateDropdown();
        GetElements();
        ChangePreset(wantedIndex);
    }

    [ContextMenu("Change Preset to wantedIndex")]
    public void test()
    {
        ChangePreset(wantedIndex);
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
    void UpdateDropdown()
    {
        paletteDropdown.Setup(ColorStyles.ToArray());
    }
    public void ChangePreset(int index)
    {
        // The Selected Preset
        ColorStylePreset preset = ColorStyles[index];

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
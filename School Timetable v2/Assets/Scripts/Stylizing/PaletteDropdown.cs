using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PaletteDropdown : MonoBehaviour
{
    [Header("References")]
    public PaletteObject Template;
    public Transform PalettesParent;
    public ColorStylizer Stylizer;

    [Space(20)]
    [Header("Functionality")]
    public UnityEvent<int> onValueChanged;
    public int value;
    
    public void Setup(ColorStylePreset[] presets)
    {
        foreach(Transform child in PalettesParent)
        {
            if (child.name.Contains("Template")) continue;
            Destroy(child.gameObject);
        }
        Template.gameObject.SetActive(true);
        for (int i = 0; i < presets.Length; i++)
        {
            PaletteObject p = Instantiate(Template, PalettesParent);
            p.transform.name = $"Palette Object ({i})";

            p.paletteIndex = i;
            
            p.PaletteNameText.text = presets[i].PaletteName;
            p.BackgroundColorImage.color = presets[i].BackgroundColor;
            p.SecondaryColorImage.color = presets[i].SecondaryColor;
            p.PrimaryColorImage.color = presets[i].PrimaryColor;
        }
        Template.gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Template.gameObject.SetActive(false);
    }

    public void ChangeValue(int newvalue)
    {
        value= newvalue;
        onValueChanged.Invoke(value);
    }
}
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

    public List<PaletteObject> paletteChildren = new();
    public void Setup(ColorStylePreset[] presets)
    {
        for (int i = 0; i < paletteChildren.Count; i++)
        {
            Destroy(paletteChildren[i].gameObject);
        }
        paletteChildren.Clear();

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
            p.toggle.SetIsOnWithoutNotify(value == i);
            paletteChildren.Add(p);
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
        
        if (newvalue < 0 || newvalue >= Stylizer.ColorStyles.Count)
        {
            Debug.LogWarning($"Invalid value {newvalue} in ChangeValue. Resetting to 0.");
            newvalue = 0;
        }
        value = newvalue;

        onValueChanged.Invoke(value);

        for (int i = 0; i < paletteChildren.Count; i++)
        {
            paletteChildren[i].toggle.SetIsOnWithoutNotify(paletteChildren[i].paletteIndex == newvalue);
        }
    }
    public void SetValueWithoutNotify(int newvalue)
    {
        
        if (newvalue < 0 || newvalue >= Stylizer.ColorStyles.Count)
        {
            Debug.LogWarning($"Invalid value {newvalue} in ChangeValue. Resetting to 0.");
            newvalue = 0;
        }
        value = newvalue;

        for (int i = 0; i < paletteChildren.Count; i++)
        {
            paletteChildren[i].toggle.SetIsOnWithoutNotify(paletteChildren[i].paletteIndex == newvalue);
        }
    }
}
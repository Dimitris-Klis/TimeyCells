using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PaletteEditor : MonoBehaviour
{
    [Header("References")]
    public PaletteCreator PaletteCreator;
    public PaletteObject Template;
    public Transform PalettesParent;
    public ColorStylizer Stylizer;
    public GameObject Message;
    public void AddCustomPalettes()
    {
        List<ColorStylePreset> customPresets = new List<ColorStylePreset>();
        for (int i = 0; i < Stylizer.ColorStyles.Count; i++)
        {
            if (Stylizer.ColorStyles[i].IsCustomPreset)
            {
                customPresets.Add(Stylizer.ColorStyles[i]);
            }
        }
        Setup(customPresets.ToArray());
    }
    public void Setup(ColorStylePreset[] presets)
    {
        foreach (Transform child in PalettesParent)
        {
            if (child.name.Contains("Template")) continue;
            if (child.name.Contains("Title")) continue;
            Destroy(child.gameObject);
        }
        Template.gameObject.SetActive(true);
        for (int i = 0; i < presets.Length; i++)
        {
            PaletteObject p = Instantiate(Template, PalettesParent);
            p.transform.name = $"Palette Object ({i})";

            p.paletteIndex = Stylizer.GetIndex(presets[i]);

            Button b = p.GetComponent<Button>();
            int index = p.paletteIndex;
            b.onClick.AddListener(
            delegate 
            {
                PaletteCreator.gameObject.SetActive(true);
                PaletteCreator.OpenCreator(index); 
            });

            p.PaletteNameText.text = presets[i].PaletteName;
            p.BackgroundColorImage.color = presets[i].BackgroundColor;
            p.SecondaryColorImage.color = presets[i].SecondaryColor;
            p.PrimaryColorImage.color = presets[i].PrimaryColor;
        }
        Template.gameObject.SetActive(false);
        Message.SetActive(PalettesParent.childCount <= 2);
    }
}

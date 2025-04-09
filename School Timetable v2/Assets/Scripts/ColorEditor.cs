using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ColorEditor : MonoBehaviour
{
    public CanvasGroup SelfGroup;
    public UnityEvent OnColorChange;
    [Header("Slider References")]
    public Slider HueSlider;
    public Slider SaturationSlider;
    public Slider ValueSlider;
    [Space]
    public Slider AlphaSlider;
    [Space]
    public TMP_InputField HexCodeField;
    [Header("GFX References")]
    public Image HueValImage; // The rainbow gradient
    public Image HueSatImage; // The white overlay
    [Space]
    public Image SatValHueImage; // The main sat image
    public Image SatValImage; // The child sat image
    [Space]
    public Image ValHueSatImage;
    [Space]
    public Image AlphaRGBImage;
    [Space]
    public Color FinalColor;
    public Image ColorPreview;

    private void Start()
    {
        Close();
    }

    public void Open(Color color)
    {
        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;
        FinalColor = color;
        UpdateSliders();
    }
    public void Close()
    {
        SelfGroup.interactable = SelfGroup.blocksRaycasts = false;
        SelfGroup.alpha = 0;
    }


    public void UpdateColor()
    {
        FinalColor = Color.HSVToRGB(HueSlider.value, SaturationSlider.value, ValueSlider.value);
        FinalColor.a = AlphaSlider.value;

        // Setting the HUE Slider color
        HueValImage.color = Color.HSVToRGB(0, 0, ValueSlider.value);

        Color HueSat = HueSatImage.color;
        HueSat.a = 1 - SaturationSlider.value;
        HueSatImage.color = HueSat;

        // Setting the SAT Slider color
        SatValHueImage.color = Color.HSVToRGB(HueSlider.value, 1, ValueSlider.value);
        SatValImage.color = Color.HSVToRGB(1, 0, ValueSlider.value);

        // Setting the VAL Slider color
        ValHueSatImage.color = Color.HSVToRGB(HueSlider.value,SaturationSlider.value, 1);

        // Setting the ALPHA Slider color.
        AlphaRGBImage.color = new(FinalColor.r, FinalColor.g, FinalColor.b, 1);

        // Setting the Color Preview Color.
        ColorPreview.color = FinalColor;

        HexCodeField.text = ColorUtility.ToHtmlStringRGBA(FinalColor);
        OnColorChange.Invoke();
    }
    public void UpdateSliders()
    {
        float H, S, V;
        Color.RGBToHSV(FinalColor, out H, out S, out V);
        HueSlider.value = H;
        SaturationSlider.value = S;
        ValueSlider.value = V;
        AlphaSlider.value = FinalColor.a;
        UpdateColor();
    }

    public void SetColor(string hexCode)
    {
        if (!hexCode.Contains("#"))
        {
            hexCode = hexCode.Insert(0, "#");
        }

        Color c = FinalColor;
        ColorUtility.TryParseHtmlString(hexCode, out c);
        FinalColor = c;
        UpdateSliders();
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ColorEditor : MonoBehaviour
{
    public CanvasGroup SelfGroup;
    public UnityEvent OnColorChange;
    public UnityEvent OnClose;
    [Header("Slider References")]
    public Slider HueSlider;
    public Slider SaturationSlider;
    public Slider ValueSlider;
    [Space]
    public Slider AlphaSlider;
    [Space]
    public TMP_InputField HexCodeField;
    [Header("GFX References")]
    public TMP_Text PromptText;
    [Space]
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
    //public ref Color colorToChange;
    public Color FinalColor;
    public Color PrevColor;
    public Image ColorPreview;

    public Image[] imageRefs;
    public TMP_Text[] textRefs;
    private void Start()
    {
        Close();
    }

    public void Open(string prompt, Color color, params Image[] images)
    {
        PromptText.text = prompt;

        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;

        PrevColor = color;
        FinalColor = color;

        imageRefs = images;
        //Debug.Log(FinalColor.a);
        UpdateSliders();
    }
    public void Open(string prompt, Color color, params TMP_Text[] texts)
    {
        PromptText.text = prompt;

        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;

        FinalColor = PrevColor = color;

        textRefs = texts;
        //Debug.Log(FinalColor.a);
        UpdateSliders();
    }
    public void Close()
    {
        SelfGroup.interactable = SelfGroup.blocksRaycasts = false;
        SelfGroup.alpha = 0;

        imageRefs = null;
        textRefs = null;

        OnClose.Invoke();
    }

    public void ApplyColor()
    {
        if(imageRefs != null)
        {
            for (int i = 0; i < imageRefs.Length; i++)
            {
                imageRefs[i].color = FinalColor;
            }
        }
        if(textRefs != null)
        {
            for (int i = 0; i < textRefs.Length; i++)
            {
                textRefs[i].color = FinalColor;
            }
        }
    }
    public void CancelColor()
    {
        if (imageRefs != null)
        {
            for (int i = 0; i < imageRefs.Length; i++)
            {
                imageRefs[i].color = PrevColor;
            }
        }
        if (textRefs != null)
        {
            for (int i = 0; i < textRefs.Length; i++)
            {
                textRefs[i].color = PrevColor;
            }
        }
        FinalColor = PrevColor;
        UpdateSliders();
        Close();
    }
    public void EndEditing()
    {
        PrevColor = FinalColor;
        UpdateSliders();
        Close();
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

        HexCodeField.text = ColorUtility.ToHtmlStringRGBA(FinalColor) + TMP_Specials.clear;

        ApplyColor();

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
        Debug.Log($"AlphaSlider: {AlphaSlider.value}, FinalColor Alpha: {FinalColor.a}");
        UpdateColor();
    }

    public void SetColor(string hexCode)
    {
        hexCode = hexCode.Replace(TMP_Specials.clear, "");
        hexCode = hexCode.Replace("#", "");
        
        hexCode = hexCode.Insert(0, "#");
        //Debug.Log(hexCode);

        Color c = FinalColor;
        ColorUtility.TryParseHtmlString(hexCode, out c);
        FinalColor = c;
        UpdateSliders();
    }
}

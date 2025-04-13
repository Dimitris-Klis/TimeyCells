using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ColorEditorFixed : MonoBehaviour
{
    public static ColorEditorFixed instance;
    private void Awake()
    {
        instance = this;
    }
    [Header("Properties")]
    public string oldPrompt;
    public Color CurrentColor, PreviousColor;

    public Image[] imageRefs;
    public TMP_Text[] textRefs;

    [Header("Slider References")]
    public Slider HueSlider;
    public Slider SaturationSlider;
    public Slider ValueSlider;
    [Space]
    public Slider AlphaSlider;
    [Space]
    public TMP_InputField HexCodeField;
    
    [Header("GFX References")]
    public CanvasGroup SelfGroup;
    [Space]
    public Image ColorPreview;
    [Space]
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

    public void Open(string prompt, Color color, params Image[] images)
    {
        imageRefs = null;
        textRefs = null;

        PromptText.text = prompt;

        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;

        if (prompt != oldPrompt)
        {
            PreviousColor = CurrentColor = color;
            textRefs = null;
            imageRefs = null;
        }

        imageRefs = images;

        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        HueSlider.value = H;
        SaturationSlider.value = S;
        ValueSlider.value = V;
        AlphaSlider.value = color.a;

        UpdateColor();
    }
    public void Open(string prompt, Color color, params TMP_Text[] texts)
    {
        imageRefs = null;
        textRefs = null;

        PromptText.text = prompt;

        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;

        if (prompt != oldPrompt)
        {
            PreviousColor = CurrentColor = color;
            textRefs = null;
            imageRefs = null;
        }
            

        textRefs = texts;

        oldPrompt = prompt;

        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        HueSlider.value = H;
        SaturationSlider.value = S;
        ValueSlider.value = V;
        AlphaSlider.value = color.a;

        UpdateColor();
    }

    public void AssignNewImages(params Image[] images)
    {
        imageRefs = images;
    }
    public void AssignNewTexts(params TMP_Text[] texts)
    {
        textRefs = texts;
    }

    public void UpdateColor()
    {
        CurrentColor = Color.HSVToRGB(HueSlider.value, SaturationSlider.value, ValueSlider.value);
        CurrentColor.a = AlphaSlider.value;

        // Setting the HUE Slider color
        HueValImage.color = Color.HSVToRGB(0, 0, ValueSlider.value);

        Color HueSat = HueSatImage.color;
        HueSat.a = 1 - SaturationSlider.value;
        HueSatImage.color = HueSat;

        // Setting the SAT Slider color
        SatValHueImage.color = Color.HSVToRGB(HueSlider.value, 1, ValueSlider.value);
        SatValImage.color = Color.HSVToRGB(1, 0, ValueSlider.value);

        // Setting the VAL Slider color
        ValHueSatImage.color = Color.HSVToRGB(HueSlider.value, SaturationSlider.value, 1);

        // Setting the ALPHA Slider color.
        AlphaRGBImage.color = new(CurrentColor.r, CurrentColor.g, CurrentColor.b, 1);

        // Setting the Color Preview Color.
        ColorPreview.color = CurrentColor;

        HexCodeField.text = ColorUtility.ToHtmlStringRGBA(CurrentColor) + TMP_Specials.clear;

        UpdateRefs();

        //OnColorChange.Invoke();
    }
    public void UpdateRefs()
    {
        if (imageRefs != null)
        {
            for (int i = 0; i < imageRefs.Length; i++)
            {
                imageRefs[i].color = CurrentColor;
            }
        }
        if (textRefs != null)
        {
            for (int i = 0; i < textRefs.Length; i++)
            {
                textRefs[i].color = CurrentColor;
            }
        }
    }

    public void ApplyColors()
    {
        oldPrompt = "";
        UpdateRefs();
        UpdateSliders();
        HideColorEditor();
    }
    public void CancelColors()
    {
        if (imageRefs != null)
        {
            for (int i = 0; i < imageRefs.Length; i++)
            {
                imageRefs[i].color = PreviousColor;
            }
        }
        if (textRefs != null)
        {
            for (int i = 0; i < textRefs.Length; i++)
            {
                textRefs[i].color = PreviousColor;
            }
        }
        CurrentColor = PreviousColor;
        UpdateSliders();
        HideColorEditor();
    }
    public void HideColorEditor()
    {
        SelfGroup.interactable = SelfGroup.blocksRaycasts = false;
        SelfGroup.alpha = 0;

        imageRefs = null;
        textRefs = null;

        //OnClose.Invoke();
    }
    public void UpdateSliders()
    {
        float H, S, V;
        Color.RGBToHSV(CurrentColor, out H, out S, out V);
        HueSlider.value = H;
        SaturationSlider.value = S;
        ValueSlider.value = V;
        //Debug.Log(CurrentColor);
        AlphaSlider.value = CurrentColor.a;
        //Debug.Log($"AlphaSlider: {AlphaSlider.value}, CurrentColor Alpha: {CurrentColor.a}");
        //UpdateColor();
    }
    public void SetColor(string hexCode)
    {
        hexCode = hexCode.Replace(TMP_Specials.clear, "");
        hexCode = hexCode.Replace("#", "");

        hexCode = hexCode.Insert(0, "#");
        //Debug.Log(hexCode);

        Color c = CurrentColor;
        ColorUtility.TryParseHtmlString(hexCode, out c);
        CurrentColor = c;
        UpdateSliders();
    }
}

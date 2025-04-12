using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using static EventTypeCreator;

public class PaletteCreator : MonoBehaviour
{
    public int IDToModify;
    public ColorStylePreset DefaultPreset;
    public CanvasGroup SelfGroup;

    [Header("Color Editor")]
    public ColorEditor coloreditor;

    [Header("Preview")]
    public PaletteObject PalettePreview;

    [Space(20)]

    [Header("Property Fields")]
    public TMP_InputField PaletteNameInput;

    [Space(20)]

    public Image PrimaryColorImage;
    public Image SecondaryColorImage;
    public Image BackgroundColorImage;

    [Space(20)]

    public Button PrimaryColorButton;
    public Button SecondaryColorButton;
    public Button BackgroundColorButton;

    [Space(20)]

    public Button DeleteButton;

    // 0: Background, 1: Secondary, 2: Primary
    public int ColorToChange = 0;
    public void OpenCreator(int ID)
    {
        CellManager.Instance.ShowEditingOverlay();
        SelfGroup.interactable = SelfGroup.blocksRaycasts = true;
        SelfGroup.alpha = 1;
        IDToModify = ID;

        if(ID < 0)
        {
            PrimaryColorImage.color = PalettePreview.PrimaryColorImage.color = DefaultPreset.PrimaryColor;
            SecondaryColorImage.color = PalettePreview.SecondaryColorImage.color = DefaultPreset.SecondaryColor;
            BackgroundColorImage.color = PalettePreview.BackgroundColorImage.color = DefaultPreset.BackgroundColor;
        }

        coloreditor.OnClose.RemoveAllListeners();
        coloreditor.OnClose.AddListener(delegate { ReEnableColorButtons(); });
    }
    public void Close()
    {
        CellManager.Instance.HideEditingOverlay();
    }
    public void ReEnableColorButtons()
    {
        PrimaryColorButton.interactable = SecondaryColorButton.interactable = BackgroundColorButton.interactable = true;
    }
    public void ActivateColorEditor(int colorToChange)
    {
        ColorToChange = colorToChange;
        PrimaryColorButton.interactable = SecondaryColorButton.interactable = BackgroundColorButton.interactable = false;
        Color currentColor = Color.white;
        switch (ColorToChange)
        {
            case 0:
                currentColor = BackgroundColorImage.color;
                coloreditor.Open("Edit Background Color", currentColor, BackgroundColorImage, PalettePreview.BackgroundColorImage);
                break;
            case 1:
                currentColor = SecondaryColorImage.color;
                coloreditor.Open("Edit Secondary Color", currentColor, SecondaryColorImage, PalettePreview.SecondaryColorImage);
                break;
            case 2:
                currentColor = PrimaryColorImage.color;
                coloreditor.Open("Edit Primary Color", currentColor, PrimaryColorImage, PalettePreview.PrimaryColorImage);
                break;
            default:
                Debug.Log("Index out of range!");
                break;
        }
    }
}
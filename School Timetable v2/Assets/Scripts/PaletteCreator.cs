using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaletteCreator : MonoBehaviour
{
    public static PaletteCreator Instance;
    private void Awake()
    {
        Instance = this;
    }
    public ColorStylizer Stylizer;
    public PaletteEditor PaletteEditor;
    public int IDToModify;
    public ColorStylePreset DefaultPreset;
    public CanvasGroup SelfGroup;

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

    //public Button PrimaryColorButton;
    //public Button SecondaryColorButton;
    //public Button BackgroundColorButton;

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
        DeleteButton.interactable = ID >= 0;
        if(ID < 0)
        {
            PaletteNameInput.text = "Palette Name";
            PrimaryColorImage.color = PalettePreview.PrimaryColorImage.color = DefaultPreset.PrimaryColor;
            SecondaryColorImage.color = PalettePreview.SecondaryColorImage.color = DefaultPreset.SecondaryColor;
            BackgroundColorImage.color = PalettePreview.BackgroundColorImage.color = DefaultPreset.BackgroundColor;
        }
        else
        {
            PrimaryColorImage.color = PalettePreview.PrimaryColorImage.color = Stylizer.ColorStyles[ID].PrimaryColor;
            SecondaryColorImage.color = PalettePreview.SecondaryColorImage.color = Stylizer.ColorStyles[ID].SecondaryColor;
            BackgroundColorImage.color = PalettePreview.BackgroundColorImage.color = Stylizer.ColorStyles[ID].BackgroundColor;
        }

        //coloreditor.OnClose.RemoveAllListeners();
        //coloreditor.OnClose.AddListener(delegate { ReEnableColorButtons(); });
    }
    public void Close()
    {
        ColorEditorFixed.instance.ApplyColors();
        CellManager.Instance.HideEditingOverlay();
    }
    //public void ReEnableColorButtons()
    //{
    //    PrimaryColorButton.interactable = SecondaryColorButton.interactable = BackgroundColorButton.interactable = true;
    //}
    public void ActivateColorEditor(int colorToChange)
    {
        ColorToChange = colorToChange;
        //PrimaryColorButton.interactable = SecondaryColorButton.interactable = BackgroundColorButton.interactable = false;
        Color currentColor = Color.white;
        switch (ColorToChange)
        {
            case 0:
                currentColor = BackgroundColorImage.color;
                ColorEditorFixed.instance.Open("Edit Background Color", currentColor, BackgroundColorImage, PalettePreview.BackgroundColorImage);
                break;
            case 1:
                currentColor = SecondaryColorImage.color;
                ColorEditorFixed.instance.Open("Edit Secondary Color", currentColor, SecondaryColorImage, PalettePreview.SecondaryColorImage);
                break;
            case 2:
                currentColor = PrimaryColorImage.color;
                ColorEditorFixed.instance.Open("Edit Primary Color", currentColor, PrimaryColorImage, PalettePreview.PrimaryColorImage);
                break;
            default:
                Debug.Log("Index out of range!");
                break;
        }
    }
    public void ChangePaletteName(string name)
    {
        PalettePreview.PaletteNameText.text = name;
    }
    public void DeleteColor() 
    {
        Stylizer.DeleteStyle(IDToModify);
        Close();
        PaletteEditor.AddCustomPalettes();
    }
    public void Save()
    {
        if(IDToModify >= 0)
        {
            Stylizer.ColorStyles[IDToModify].PaletteName = PaletteNameInput.text;
            Stylizer.ColorStyles[IDToModify].PrimaryColor = PrimaryColorImage.color;
            Stylizer.ColorStyles[IDToModify].SecondaryColor = SecondaryColorImage.color;
            Stylizer.ColorStyles[IDToModify].BackgroundColor = BackgroundColorImage.color;
        }
        else
        {
            ColorStylePreset p = new();
            p.IsCustomPreset = true;
            p.PaletteName = PaletteNameInput.text;
            p.PrimaryColor = PrimaryColorImage.color;
            p.SecondaryColor = SecondaryColorImage.color;
            p.BackgroundColor = BackgroundColorImage.color;
            Stylizer.ColorStyles.Add(p);
        }

        Stylizer.GetElements();
        Stylizer.UpdateDropdown();
        PaletteEditor.AddCustomPalettes();
        Close();
    }
}
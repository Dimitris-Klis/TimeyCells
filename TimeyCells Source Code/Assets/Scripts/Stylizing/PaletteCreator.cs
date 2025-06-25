using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaletteCreator : MonoBehaviour
{
    [Header("     Creator Properties")]
    [ReadOnly] public int IDToModify;
    [ReadOnly] public int ColorToChange = 0; // 0: Background, 1: Secondary, 2: Primary

    [Space(20)]
    [Header("Defaults")]
    public ColorStylePreset ThemeDefaults;

    [Space(20)]
    [Header("References")]
    [Space(0)]
    
    [Header("--- Important Systems")]
    public ColorStylizer Stylizer;
    public CustomPaletteLister CustomPaletteLister;

    [Header("--- Preview")]
    public PaletteObject PalettePreview;
    [Space]
    public TMP_Text TitleText;

    [Space(20)]
    [Header("--- Property Fields")]
    public TMP_InputField PaletteNameInput;

    [Space(20)]
    [Header("--- Colors UI")]
    public Image PrimaryColorImage;
    public Image SecondaryColorImage;
    public Image BackgroundColorImage;

    [Space(20)]
    [Header("--- Delete Button")]
    public Button DeleteButton;


    public void OpenCreator(int ID)
    {
        TitleText.text = ID >= 0 ? LocalizationSystem.instance.GetText(gameObject.name, "EDIT_THEME") : LocalizationSystem.instance.GetText(gameObject.name, "CREATE_THEME");

        IDToModify = ID;
        DeleteButton.interactable = ID >= 0;
        if(ID < 0)
        {
            PaletteNameInput.text = ThemeDefaults.PaletteName;
            PalettePreview.PaletteNameText.text = ThemeDefaults.PaletteName;
            PrimaryColorImage.color = PalettePreview.PrimaryColorImage.color = ThemeDefaults.PrimaryColor;
            SecondaryColorImage.color = PalettePreview.SecondaryColorImage.color = ThemeDefaults.SecondaryColor;
            BackgroundColorImage.color = PalettePreview.BackgroundColorImage.color = ThemeDefaults.BackgroundColor;
        }
        else
        {
            PaletteNameInput.text = Stylizer.ColorStyles[ID].PaletteName;
            PalettePreview.PaletteNameText.text = Stylizer.ColorStyles[ID].PaletteName;
            PrimaryColorImage.color = PalettePreview.PrimaryColorImage.color = Stylizer.ColorStyles[ID].PrimaryColor;
            SecondaryColorImage.color = PalettePreview.SecondaryColorImage.color = Stylizer.ColorStyles[ID].SecondaryColor;
            BackgroundColorImage.color = PalettePreview.BackgroundColorImage.color = Stylizer.ColorStyles[ID].BackgroundColor;
        }
    }

    public void CloseCreator()
    {
        ColorEditor.instance.ApplyColors();
        gameObject.SetActive(false);
    }




    public void ActivateColorEditor(int colorToChange)
    {
        ColorToChange = colorToChange;
        Color currentColor = Color.white;
        switch (ColorToChange)
        {
            case 0:
                currentColor = BackgroundColorImage.color;
                ColorEditor.instance.Open(
                    LocalizationSystem.instance.GetText(gameObject.name, "COLOREDITOR_PROMPT_BACKGROUND"), currentColor, BackgroundColorImage, PalettePreview.BackgroundColorImage);
                break;
            case 1:
                currentColor = SecondaryColorImage.color;
                ColorEditor.instance.Open(
                    LocalizationSystem.instance.GetText(gameObject.name, "COLOREDITOR_PROMPT_SECONDARY"), currentColor, SecondaryColorImage, PalettePreview.SecondaryColorImage);
                break;
            case 2:
                currentColor = PrimaryColorImage.color;
                ColorEditor.instance.Open(
                    LocalizationSystem.instance.GetText(gameObject.name, "COLOREDITOR_PROMPT_PRIMARY"), currentColor, PrimaryColorImage, PalettePreview.PrimaryColorImage);
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




    public void Delete(bool confirm)
    {
        if (!confirm)
        {
            ConfirmationManager.ButtonPrompt Cancel = new(LocalizationSystem.instance.GetText(gameObject.name, "BUTTONS_CANCEL"), null);
            ConfirmationManager.ButtonPrompt Confirm = new(LocalizationSystem.instance.GetText(gameObject.name, "BUTTONS_DELETE"), delegate { Delete(true); });
            ConfirmationManager.Instance.ShowConfirmation
            (
                LocalizationSystem.instance.GetText(gameObject.name, "PROMPT_TITLE_AREYOUSURE"),
                LocalizationSystem.instance.GetText(gameObject.name, "PROMPT_DESC_THEME").Replace("{x}", PaletteNameInput.text),
                Cancel, Confirm
            );
            return;
        }
        Stylizer.DeleteStyle(IDToModify);
        CloseCreator();
        CustomPaletteLister.AddCustomPalettes();
        SaveManager.instance.SaveSettings();
    }

    public void Confirm()
    {
        if (IDToModify >= 0)
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
        CustomPaletteLister.AddCustomPalettes();
        SaveManager.instance.SaveSettings();
        CloseCreator();
    }
}
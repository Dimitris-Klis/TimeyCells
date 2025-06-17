using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabelEditor : MonoBehaviour
{
    public TimeIndexObject TimeIndexPreview;
    int objectToModify;
    public Toggle CustomLabelToggle;
    [Space]
    public TMP_InputField CustomLabelInput;
    public Toggle CountAsIndexToggle;
    public void ActivateEditor(int objToEdit)
    {
        objectToModify = objToEdit;
        var obj = DayTimeManager.instance.TimeLabels[objectToModify];
        CustomLabelToggle.isOn = 
            CustomLabelInput.interactable = 
            CountAsIndexToggle.interactable = 
            obj.IsCustomLabel;

        TimeIndexPreview.TimeText.text = DayTimeManager.instance.TimeIndexPreviews[objectToModify].TimeText.text;
        TimeIndexPreview.IndexText.text = obj.CustomLabelName;

        if (obj.IsCustomLabel)
        {
            CustomLabelInput.text = obj.CustomLabelName;
        }
        else
        {
            CustomLabelInput.text = "";
        }
    }
    public void IsCustomLabel(bool isTrue)
    {
        CustomLabelInput.interactable = CountAsIndexToggle.interactable = isTrue;
    }
    public void SetCustomLabel(string text)
    {
        TimeIndexPreview.IndexText.text = text;
    }
    public void Confirm()
    {
        SaveManager.instance.ChangesMade();
        var obj = DayTimeManager.instance.TimeLabels[objectToModify];
        obj.IsCustomLabel = CustomLabelToggle.isOn;
        obj.CountAsIndex = CountAsIndexToggle.isOn;
        obj.CustomLabelName = CustomLabelInput.text;
        DayTimeManager.instance.UpdateTimeIndexes();
        gameObject.SetActive(false);
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

// This one is meant for copy-pasting simpler things, like cell info, events, event types, etc.
public class CopyPasteManager : MonoBehaviour
{
    public SaveManager SaveManager;
    public CellInfoEditor CellInfoEditor;
    public void CopyCellInfo()
    {
        TimetableData.ExtraCellInfoData cellData = new();
        cellData.SelectedEvent = CellInfoEditor.GetSelectedInfo().SelectedEventBase;
        cellData.EventNameOverride = CellInfoEditor.EventNameOverride.text.Replace(TMP_Specials.clear, "");
        cellData.Info1Override = CellInfoEditor.Info1Override.text.Replace(TMP_Specials.clear, "");
        cellData.Info2Override = CellInfoEditor.Info2Override.text.Replace(TMP_Specials.clear, "");
        cellData.EventTypeOverride = CellInfoEditor.TypeOverride.value - 1;
        cellData.OverrideFavourite = CellInfoEditor.FavouriteOverride.value;

        cellData.OverrideCommonLength = CellInfoEditor.OverrideTimeToggle.isOn;

        string hours = CellInfoEditor.LengthInputHours.text.Replace(TMP_Specials.clear, "");
        string mins = CellInfoEditor.LengthInputMinutes.text.Replace(TMP_Specials.clear, "");
        if (DayTimeManager.TryParseLength(hours, mins, out TimeSpan len))
        {
            cellData.NewLength[0] = len.Hours;
            cellData.NewLength[1] = len.Minutes;
        }

        if (CellInfoEditor.TempPropertiesLayout.activeSelf)
        {
            cellData.TempEventNameOverride= CellInfoEditor.TempEventNameOverride.text.Replace(TMP_Specials.clear, "");
            cellData.TempInfo1Override = CellInfoEditor.TempInfo1Override.text.Replace(TMP_Specials.clear, "");
            cellData.TempInfo2Override= CellInfoEditor.TempInfo2Override.text.Replace(TMP_Specials.clear, "");
            cellData.TempEventTypeOverride = CellInfoEditor.TempTypeOverride.value - 1;
            cellData.TempOverrideFavourite= CellInfoEditor.TempFavouriteOverride.value;

            cellData.TempOverrideCommonLength = CellInfoEditor.TempOverrideTimeToggle.isOn;

            cellData.OverrideDate[0] = DateTime.Today.Year;
            cellData.OverrideDate[1] = DateTime.Today.Month;
            cellData.OverrideDate[2] = DateTime.Today.Day;
            if (int.TryParse(CellInfoEditor.DelayInput.text.Replace(TMP_Specials.clear, ""), out int delay))
                cellData.OverrideDelayWeeks = delay;
            else
                cellData.OverrideDelayWeeks = 0;


            if (int.TryParse(CellInfoEditor.LengthInput.text.Replace(TMP_Specials.clear, ""), out int length))
                cellData.ExtraOverrideLengthWeeks = length;
            else
                cellData.ExtraOverrideLengthWeeks = 0;
        }
        else
        {
            cellData.ExtraOverrideLengthWeeks = -1;
        }

        string temphours = CellInfoEditor.TempLengthInputHours.text.Replace(TMP_Specials.clear, "");
        string tempmins = CellInfoEditor.TempLengthInputMinutes.text.Replace(TMP_Specials.clear, "");
        if (DayTimeManager.TryParseLength(temphours,tempmins, out TimeSpan templen))
        {
            cellData.TempNewLength[0] = templen.Hours;
            cellData.TempNewLength[1] = templen.Minutes;
        }

        // Getting Start time.
        if (DayTimeManager.TryParseTime(CellInfoEditor.StartTimeInput.text.Replace(TMP_Specials.clear, ""), out DateTime start))
        {
            cellData.StartTime = CellInfoEditor.StartTimeInput.text.Replace(TMP_Specials.clear, "");
        }
        else
        {
            TimeSpan t = DayTimeManager.instance.GetCellStartTime(CellInfoEditor.SelectedCellColumn, CellInfoEditor.SelectedCellRow);
            cellData.StartTime = DayTimeManager.instance.FormatTime(t);
        }

        // Getting Temp Start time.
        if (DayTimeManager.TryParseTime(CellInfoEditor.TempStartTimeInput.text.Replace(TMP_Specials.clear, ""), out DateTime tempstart))
        {
            cellData.TempStartTime = CellInfoEditor.TempStartTimeInput.text.Replace(TMP_Specials.clear, "");
        }
        else
        {
            TimeSpan t = DayTimeManager.instance.GetCellStartTime(CellInfoEditor.SelectedCellColumn, CellInfoEditor.SelectedCellRow);
            cellData.TempStartTime = DayTimeManager.instance.FormatTime(t);
        }

        GUIUtility.systemCopyBuffer = JsonUtility.ToJson(cellData, false);
    }

    public void PasteCellInfo()
    {
        // Loading the data.
        string json = GUIUtility.systemCopyBuffer;

        TimetableData.ExtraCellInfoData cellData = null;

        // This is meant to prevent any weird behaviour if you paste anything other than a json file.
        try
        {
            cellData = JsonUtility.FromJson<TimetableData.ExtraCellInfoData>(json);
        }
        catch (Exception)
        {
            return;
        }
        if (cellData == null)
        {
            return;
        }

        CellInfoEditor.ChangeInfoBase(cellData.SelectedEvent);
        CellInfoEditor.EventNameOverride.text = cellData.EventNameOverride + (cellData.EventNameOverride != "" ? TMP_Specials.clear : "");
        CellInfoEditor.Info1Override.text = cellData.Info1Override + (cellData.Info1Override != "" ? TMP_Specials.clear : "");
        CellInfoEditor.Info2Override.text = cellData.Info2Override + (cellData.Info2Override != "" ? TMP_Specials.clear : "");
        CellInfoEditor.TypeOverride.value = cellData.EventTypeOverride + 1;
        CellInfoEditor.FavouriteOverride.value = cellData.OverrideFavourite;

        CellInfoEditor.OverrideTimeToggle.isOn = cellData.OverrideCommonLength;

        CellInfoEditor.LengthInputHours.text = cellData.NewLength[0].ToString() + TMP_Specials.clear;
        CellInfoEditor.LengthInputMinutes.text = cellData.NewLength[1].ToString() + TMP_Specials.clear;

        if (cellData.ExtraOverrideLengthWeeks >= 0)
        {
            CellInfoEditor.TempEventNameOverride.text = cellData.TempEventNameOverride + (cellData.TempEventNameOverride != "" ? TMP_Specials.clear : "");
            CellInfoEditor.TempInfo1Override.text = cellData.TempInfo1Override + (cellData.TempInfo1Override != "" ? TMP_Specials.clear : "");
            CellInfoEditor.TempInfo2Override.text = cellData.TempInfo2Override + (cellData.TempInfo2Override != "" ? TMP_Specials.clear : "");
            CellInfoEditor.TempTypeOverride.value = cellData.TempEventTypeOverride + 1;
            CellInfoEditor.TempFavouriteOverride.value = cellData.TempOverrideFavourite;

            CellInfoEditor.TempOverrideTimeToggle.isOn = cellData.TempOverrideCommonLength;
            CellInfoEditor.OverrideDate = DateTime.Today;


            CellInfoEditor.DelayInput.text = cellData.OverrideDelayWeeks + TMP_Specials.clear;
            CellInfoEditor.LengthInput.text = cellData.ExtraOverrideLengthWeeks + TMP_Specials.clear;

            CellInfoEditor.TempLengthInputHours.text = cellData.TempNewLength[0] + TMP_Specials.clear;
            CellInfoEditor.TempLengthInputMinutes.text = cellData.TempNewLength[1] + TMP_Specials.clear;

            CellInfoEditor.TempPropertiesLayout.SetActive(true);
            CellInfoEditor.TempPromptLayout.SetActive(false);
        }
        else
        {
            CellInfoEditor.TempPropertiesLayout.SetActive(false);
            CellInfoEditor.TempPromptLayout.SetActive(true);
        }

        CellInfoEditor.LengthInputHours.interactable = cellData.OverrideCommonLength;
        CellInfoEditor.LengthInputMinutes.interactable = cellData.OverrideCommonLength;

        CellInfoEditor.TempLengthInputHours.interactable = cellData.TempOverrideCommonLength;
        CellInfoEditor.TempLengthInputMinutes.interactable = cellData.TempOverrideCommonLength;


        CellInfoEditor.LengthInputHours.text = cellData.NewLength[0] + TMP_Specials.clear;
        CellInfoEditor.LengthInputMinutes.text = cellData.NewLength[1] + TMP_Specials.clear;

        // Getting Start time.
        if (DayTimeManager.TryParseTime(cellData.StartTime, out DateTime start))
        {
            CellInfoEditor.StartTimeInput.text = cellData.StartTime + TMP_Specials.clear;
        }
        else
        {
            TimeSpan t = DayTimeManager.instance.GetCellStartTime(CellInfoEditor.SelectedCellColumn, CellInfoEditor.SelectedCellRow);
            CellInfoEditor.StartTimeInput.text = DayTimeManager.instance.FormatTime(t) + TMP_Specials.clear;
        }

        // Getting Temp Start time.
        if (DayTimeManager.TryParseTime(cellData.TempStartTime, out DateTime tempstart))
        {
            CellInfoEditor.TempStartTimeInput.text = cellData.TempStartTime + TMP_Specials.clear;
        }
        else
        {
            TimeSpan t = DayTimeManager.instance.GetCellStartTime(CellInfoEditor.SelectedCellColumn, CellInfoEditor.SelectedCellRow);
            CellInfoEditor.TempStartTimeInput.text = DayTimeManager.instance.FormatTime(t);
        }
    }
}
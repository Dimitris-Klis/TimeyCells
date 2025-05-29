using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WeekdayEditor : MonoBehaviour
{
    public WeekDayObject WeekdayPreview;
    public TMP_InputField WeekdayName;
    public TMP_InputField StartTimeField;
    public TMP_InputField CommonLengthFieldHours;
    public TMP_InputField CommonLengthFieldMinutes;
    public Toggle[] DayToggles;
    public int WeekdayIndex;
    [Space(30)]
    [Header("Temporary Overriding")]
    public TabHandler TabHandler;
    [Space]
    public GameObject Create;
    public GameObject CreateButton;
    public GameObject ErrorText;
    public GameObject Override;
    public GameObject DeleteOverrideButton;
    [Space]
    public Slider DelaySlider;
    public Slider LengthSlider;
    public TMP_InputField DelayInput;
    public TMP_InputField LengthInput;
    [Space]
    public Toggle TempStartTimeToggle;
    public TMP_InputField TempStartTimeField;

    public Toggle TempCommonLengthToggle;
    public TMP_InputField TempCommonLengthFieldHours;
    public TMP_InputField TempCommonLengthFieldMinutes;
    DateTime OverrideDate;
    public void OpenWeekday(int index)
    {
        WeekdayIndex = index;
        
        WeekDay SelectedWeekday = DayTimeManager.instance.WeekDays[index];

        if (SelectedWeekday.OverrideExtraLengthWeeks > -1)
        {
            TabHandler.SelectTab(1);
            Override.SetActive(true);
            DeleteOverrideButton.SetActive(true);
            Create.SetActive(false);

            DelaySlider.value = SelectedWeekday.OverrideDelayWeeks;
            LengthSlider.value = SelectedWeekday.OverrideExtraLengthWeeks;
            DelayInput.text = DelaySlider.value.ToString();
            LengthInput.text = LengthSlider.value.ToString();

            TempStartTimeField.text = DayTimeManager.instance.FormatTime(SelectedWeekday.TempStartTime);
            TempCommonLengthFieldHours.text = SelectedWeekday.TempCommonLength.Hours.ToString();
            TempCommonLengthFieldMinutes.text = SelectedWeekday.TempCommonLength.Minutes.ToString();

            TempStartTimeToggle.isOn = (SelectedWeekday.OverrideMode == 1 || SelectedWeekday.OverrideMode == 3);
            TempCommonLengthToggle.isOn = (SelectedWeekday.OverrideMode == 2 || SelectedWeekday.OverrideMode == 3);
        }
        else
        {
            TabHandler.SelectTab(0);
        }
        CreateButton.SetActive(SelectedWeekday.Days != 0);
        ErrorText.SetActive(SelectedWeekday.Days == 0);

        WeekdayName.text = WeekdayPreview.WeekDayName.text = SelectedWeekday.DayName;
        StartTimeField.text = DayTimeManager.instance.FormatTime(SelectedWeekday.StartTime);
        CommonLengthFieldHours.text = SelectedWeekday.CommonLength.Hours.ToString();
        CommonLengthFieldMinutes.text = SelectedWeekday.CommonLength.Minutes.ToString();


        DayToggles[0].interactable = true;
        DayToggles[1].interactable = true;
        DayToggles[2].interactable = true;
        DayToggles[3].interactable = true;
        DayToggles[4].interactable = true;
        DayToggles[5].interactable = true;
        DayToggles[6].interactable = true;

        for (int i = 0; i < DayTimeManager.instance.WeekDays.Count; i++)
        {
            WeekDay wd = DayTimeManager.instance.WeekDays[i];
            
            uint Sun = wd.Days /  1 % 2;
            uint Mon = wd.Days /  2 % 2;
            uint Tue = wd.Days /  4 % 2;
            uint Wed = wd.Days /  8 % 2;
            uint Thu = wd.Days / 16 % 2;
            uint Fri = wd.Days / 32 % 2;
            uint Sat = wd.Days / 64 % 2;
            
            if (i == index)
            {
                DayToggles[0].isOn = Mon == 1;
                DayToggles[1].isOn = Tue == 1;
                DayToggles[2].isOn = Wed == 1;
                DayToggles[3].isOn = Thu == 1;
                DayToggles[4].isOn = Fri == 1;
                DayToggles[5].isOn = Sat == 1;
                DayToggles[6].isOn = Sun == 1;

                // Debug.Log($"{Sat}{Fri}{Thu}{Wed}{Tue}{Mon}{Sun}");
                continue;
            }
            DayToggles[0].interactable = DayToggles[0].interactable && Mon == 0;
            DayToggles[1].interactable = DayToggles[1].interactable && Tue == 0;
            DayToggles[2].interactable = DayToggles[2].interactable && Wed == 0;
            DayToggles[3].interactable = DayToggles[3].interactable && Thu == 0;
            DayToggles[4].interactable = DayToggles[4].interactable && Fri == 0;
            DayToggles[5].interactable = DayToggles[5].interactable && Sat == 0;
            DayToggles[6].interactable = DayToggles[6].interactable && Sun == 0;
        }
    }
    public void ParseStartTime(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (!DayTimeManager.TryParseTime(text, out DateTime length))
        {
            WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];
            
            if (Override.activeInHierarchy)
            {
                TempStartTimeField.text = DayTimeManager.instance.FormatTime(wd.StartTime);
            }
            else
            {
                StartTimeField.text = DayTimeManager.instance.FormatTime(wd.StartTime);
            }
        }
    }
    public void ParseMinutes(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if(int.TryParse(text, out int length))
        {
            if (length > 59) length = 59;
            if (Override.activeInHierarchy)
            {
                TempCommonLengthFieldMinutes.text = length.ToString();
            }
            else
            {
                CommonLengthFieldMinutes.text = length.ToString();
            }
            
        }
        else
        {
            WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];
            if (Override.activeInHierarchy)
            {
                TempCommonLengthFieldMinutes.text = wd.CommonLength.Minutes.ToString();
            }
            else
            {
                CommonLengthFieldMinutes.text = wd.CommonLength.Minutes.ToString();
            }
            
        }
    }
    public void ParseHours(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (int.TryParse(text, out int length))
        {
            if (length > 23) length = 23; // No event will last 24 hours!
            if (Override.activeInHierarchy)
            {
                TempCommonLengthFieldHours.text = length.ToString();
            }
            else
            {
                CommonLengthFieldHours.text = length.ToString();
            }
            
        }
        else
        {
            WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];
            if (Override.activeInHierarchy)
            {
                TempCommonLengthFieldHours.text = wd.CommonLength.Hours.ToString();
            }
            else
            {
                CommonLengthFieldHours.text = length.ToString();
            }
        }
    }

    public void SetDelay(float text)
    {
        DelayInput.text = text.ToString();
    }
    public void SetLength(float text)
    {
        LengthInput.text = text.ToString();
    }
    public void UpdateDelaySlider(string Delay)
    {
        if (int.TryParse(Delay.Replace(TMP_Specials.clear, ""), out int val))
        {
            DelaySlider.SetValueWithoutNotify(Mathf.Clamp(val, DelaySlider.minValue, DelaySlider.maxValue));
        }
        else
        {
            DelaySlider.SetValueWithoutNotify(0);
        }
    }
    public void UpdateLengthSlider(string Length)
    {
        if (int.TryParse(Length.Replace(TMP_Specials.clear, ""), out int val))
        {
            LengthSlider.SetValueWithoutNotify(Mathf.Clamp(val, LengthSlider.minValue, LengthSlider.maxValue));
        }
        else
        {
            LengthSlider.SetValueWithoutNotify(0);
        }
    }


    public void SetStartTimeInteractable(bool enabled)
    {
        TempStartTimeField.interactable = enabled;
    }
    public void SetCommonLengthInteractable(bool enabled)
    {
        TempCommonLengthFieldHours.interactable = enabled;
        TempCommonLengthFieldMinutes.interactable = enabled;
    }

    public void UpdateCreateButton()
    {
        CreateButton.SetActive(false);
        ErrorText.SetActive(true);
        for (int i = 0; i < DayToggles.Length; i++)
        {
            if (DayToggles[i].isOn)
            {
                CreateButton.SetActive(true);
                ErrorText.SetActive(false);
                return;
            }
        }
    }
    public void CreateTemp()
    {
        Override.SetActive(true);
        DeleteOverrideButton.SetActive(true);
        Create.SetActive(false);
        OverrideDate = DateTime.Today;
        

        WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];

        TempStartTimeField.text = DayTimeManager.instance.FormatTime(wd.StartTime);
        TempCommonLengthFieldMinutes.text = wd.CommonLength.Minutes.ToString();
        TempCommonLengthFieldHours.text = wd.CommonLength.Hours.ToString();
    }

    public void DeleteTemp()
    {
        Override.SetActive(false);
        DeleteOverrideButton.SetActive(false);
        Create.SetActive(true);
    }

    // There is no cancel function. Cancelling is as simple as turning off the Editor gameobject.

    public void Confirm()
    {
        SaveManager.ChangesMade();
        WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];

        wd.DayName = WeekdayName.text;

        if (DayTimeManager.TryParseTime(StartTimeField.text.Replace(TMP_Specials.clear, ""), out DateTime result))
        {
            wd.StartTime = result.TimeOfDay;
        }

        string hours = CommonLengthFieldHours.text.Replace(TMP_Specials.clear, "");
        string mins = CommonLengthFieldMinutes.text.Replace(TMP_Specials.clear, "");
        if (DayTimeManager.TryParseLength(hours, mins, out TimeSpan result2))
        {
            wd.CommonLength = result2;
        }

        int Sun = DayToggles[6].isOn ? 1 : 0;//* 1
        int Mon = (DayToggles[0].isOn ? 1 : 0) * 2;
        int Tue = (DayToggles[1].isOn ? 1 : 0) * 4;
        int Wed = (DayToggles[2].isOn ? 1 : 0) * 8;
        int Thu = (DayToggles[3].isOn ? 1 : 0) * 16;
        int Fri = (DayToggles[4].isOn ? 1 : 0) * 32;
        int Sat = (DayToggles[5].isOn ? 1 : 0) * 64;

        wd.Days = (uint)(Mon + Tue + Wed + Thu + Fri + Sat + Sun);

        if (Override.activeSelf && (TempCommonLengthToggle.isOn || TempStartTimeToggle.isOn))
        {
            if (TempStartTimeToggle.isOn) wd.OverrideMode++;
            if (TempCommonLengthToggle.isOn) wd.OverrideMode++;

            int dayOfWeek = 6; // The cell info's day of week
            for (int i = 64; i > 1; i /= 2)
            {
                if (wd.Days / i % 2 == 1) break;
                dayOfWeek--;
            }

            wd.OverrideDate = OverrideDate;
            
            if (dayOfWeek >= (int)wd.OverrideDate.DayOfWeek)
            {
                wd.OverrideLength = dayOfWeek - (int)wd.OverrideDate.DayOfWeek;
            }
            else
            {
                wd.OverrideLength = 7 - dayOfWeek + (int)wd.OverrideDate.DayOfWeek;
            }

            if (int.TryParse(DelayInput.text.Replace(TMP_Specials.clear, ""), out int delay))
                wd.OverrideDelayWeeks = delay;
            else
                wd.OverrideDelayWeeks = 0;


            if (int.TryParse(LengthInput.text.Replace(TMP_Specials.clear, ""), out int length))
                wd.OverrideExtraLengthWeeks = length;
            else
                wd.OverrideExtraLengthWeeks = 0;
        }
        else
        {
            wd.OverrideExtraLengthWeeks = -1;
        }

        gameObject.SetActive(false);
    }
}
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

    public void OpenWeekday(int index)
    {
        WeekdayIndex = index;
        
        WeekDay SelectedWeekday = DayTimeManager.instance.WeekDays[index];
        
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
            StartTimeField.text = DayTimeManager.instance.FormatTime(wd.StartTime);
        }
    }
    public void ParseCommonLength(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (!DayTimeManager.TryParseLength(text, out DateTime length))
        {
            WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];
            CommonLengthFieldHours.text = wd.CommonLength.Hours.ToString();
            CommonLengthFieldMinutes.text = wd.CommonLength.Minutes.ToString();
        }
    }
    public void ParseMinutes(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if(int.TryParse(text, out int length))
        {
            if (length > 59) length = 59;
            CommonLengthFieldMinutes.text = length.ToString();
        }
        else
        {
            WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];
            CommonLengthFieldMinutes.text = wd.CommonLength.Minutes.ToString();
        }
    }
    public void ParseHours(string text)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (int.TryParse(text, out int length))
        {
            if (length > 23) length = 23; // No event will last 24 hours!
            CommonLengthFieldHours.text = length.ToString();
        }
        else
        {
            WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];
            CommonLengthFieldHours.text = wd.CommonLength.Hours.ToString();
        }
    }

    // There is no cancel function. Cancelling is as simple as turning off the Editor gameobject.

    public void Confirm()
    {
        WeekDay wd = DayTimeManager.instance.WeekDays[WeekdayIndex];

        wd.DayName = WeekdayName.text;

        if (DayTimeManager.TryParseTime(StartTimeField.text.Replace(TMP_Specials.clear, ""), out DateTime result))
        {
            wd.StartTime = result.TimeOfDay;
        }
        string lentext = CommonLengthFieldHours.text.Replace(TMP_Specials.clear, "") + ":" + CommonLengthFieldMinutes.text.Replace(TMP_Specials.clear, "");
        if (DayTimeManager.TryParseLength(lentext, out DateTime result2))
        {
            wd.CommonLength = result2.TimeOfDay;
        }

        int Sun = DayToggles[6].isOn ? 1 : 0;//* 1
        int Mon = (DayToggles[0].isOn ? 1 : 0) * 2;
        int Tue = (DayToggles[1].isOn ? 1 : 0) * 4;
        int Wed = (DayToggles[2].isOn ? 1 : 0) * 8;
        int Thu = (DayToggles[3].isOn ? 1 : 0) * 16;
        int Fri = (DayToggles[4].isOn ? 1 : 0) * 32;
        int Sat = (DayToggles[5].isOn ? 1 : 0) * 64;

        wd.Days = (uint)(Mon + Tue + Wed + Thu + Fri + Sat + Sun);

        gameObject.SetActive(false);
    }
}
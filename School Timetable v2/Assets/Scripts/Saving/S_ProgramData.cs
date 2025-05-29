using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class S_ProgramData
{
    public List<LessonCellData> Cells;
    [System.Serializable]
    public class LessonCellData
    {
        public string LessonName;
        public string RoomIndex;
        public string TeacherName;

        //0 = Normal, 1 = Moving, 2 = Gym, 3 = Support
        public int LessonType;
        public bool Tested;
        public bool Favourite;
    }
    public bool[] _7hDays = new bool[5];
    public int[] BreakLengths_7h = new int[3];
    public int[] BreakLengths_8h = new int[3];

    public int[] StartTime = new int[2] { 7, 30 };
    public int[] EndTime = new int[2] { 13, 35 };

    public int _7hDuration = 45;
    public int _8hDuration = 40;
    public string FileName;
}

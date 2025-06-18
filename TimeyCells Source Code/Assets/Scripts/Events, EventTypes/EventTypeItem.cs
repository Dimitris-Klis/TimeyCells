using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class EventTypeItem
{
    public int ItemID;
    [Space]
    public string TypeName;
    [Space]
    public Color TextColor;
    public Color BackgroundColor;

    public EventTypeItem(TimetableData.EventTypeData data)
    {
        ItemID = data.ItemID;
        TypeName = data.TypeName;
        TextColor = new(data.TextColor[0], data.TextColor[1], data.TextColor[2], data.TextColor[3]);
        BackgroundColor = new(data.BackgroundColor[0], data.BackgroundColor[1], data.BackgroundColor[2], data.BackgroundColor[3]);
    }
    public EventTypeItem()
    {
        ItemID = 0;
        TypeName = "";
        TextColor = Color.black;
        BackgroundColor = Color.black;
    }
}

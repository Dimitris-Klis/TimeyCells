using System.Collections.Generic;
using UnityEngine;

public class TimetableGrid : MonoBehaviour
{
    public RectTransform rect;
    [Space]
    public Vector2 CellSize;
    public Vector2 Spacing;
    [Space]
    public int Rows;
    public int Columns;
    [Space]
    public bool Center;
    public TimetableChild TimetablePrefab;

    [System.Serializable]
    public class Column
    {
        public List<TimetableChild> Children = new List<TimetableChild>();
    }
    public List<Column> ColumnsList;
    //public List<TimetableChild> Children = new();

    [ContextMenu("Set it up!!!")]
    public void Setup()
    {
        if(rect == null) rect = GetComponent<RectTransform>();
        ClearAll();
        
        for (int x = 0; x < Columns; x++)
        {
            ColumnsList.Add(new Column());
            for (int y = 0; y < Rows; y++)
            {
                TimetableChild t = Instantiate(TimetablePrefab, this.transform);
                t.transform.localPosition = Vector3.zero;

                t.rect.sizeDelta = CellSize;

                Vector3 wantedpos = t.transform.localPosition;

                // Setting the offset depending on CellSize
                wantedpos.x += CellSize.x * ((float)x+.5f);
                wantedpos.y -= CellSize.y * ((float)y + .5f);

                // Setting the offset depending on Spacing
                wantedpos.x += Spacing.x * x;
                wantedpos.y -= Spacing.y * y;

                // The offset needed to center the UI (hopefully).

                t.transform.localPosition = wantedpos + GetOffset();
                //Children.Add(t);
                ColumnsList[x].Children.Add(t);
            }
        }
    }
    public Vector3 GetOffset()
    {
        if (!Center) return Vector3.zero;
        Vector3 offset = Vector3.zero;

        offset.x = Mathf.Abs(rect.sizeDelta.x - (Columns * CellSize.x + (Columns - 1) * Spacing.x)) / 2;
        offset.y = Mathf.Abs(rect.sizeDelta.y - (Rows * CellSize.y + (Rows - 1) * Spacing.y)) / -2;
        return offset;
    }
    public void AddColumn(bool isbreak)
    {
        if (Center) RemoveAllOffsets();

        Columns++;

        if (isbreak)
        {
            TimetableChild t = Instantiate(TimetablePrefab, this.transform);
            t.transform.localPosition = Vector3.zero;
            t.rect.sizeDelta = CellSize;


            Vector3 wantedpos = t.transform.localPosition;
            Vector2 wantedscale = t.rect.sizeDelta;

            wantedpos.x += ((float)Columns - .5f) * CellSize.x;
            wantedpos.x += (Columns - 1) * Spacing.x;

            // The row where the cell should be.
            float cellRowIndex = 0;
            
            if (Rows % 2 == 0)
                cellRowIndex = (Rows / 2);
            else
                cellRowIndex = (Rows / 2) + 0.5f;


            wantedpos.y -= cellRowIndex * CellSize.y;
            wantedpos.y -= (cellRowIndex - 1) * Spacing.y;
            wantedpos.y -= Spacing.y / 2;

            wantedscale.y = Rows * CellSize.y + (Rows - 1) * Spacing.y;

            t.rect.sizeDelta = wantedscale;
            t.transform.localPosition = wantedpos;
            //Children.Add(t);
            ColumnsList.Add(new());
            ColumnsList[ColumnsList.Count - 1].Children.Add(t);
        }
        if(Center) ReAddOffsets();
    }
    public void RemoveAllOffsets()
    {
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                children[j].transform.localPosition -= GetOffset();
            }
        }
    }
    public void ReAddOffsets()
    {
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                children[j].transform.localPosition += GetOffset();
            }
        }
    }
    [ContextMenu("Test Break!")]
    public void TestAddBreak() // temp
    {
        AddColumn(true);
    }
    public void ClearAll()
    {
        if (ColumnsList.Count <= 0) return;
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                if (children[j].gameObject != null)
                    DestroyImmediate(children[j].gameObject);
            }
        }
        ColumnsList.Clear();
    }
    public void RemoveColumn(int colIndex)
    {
        RemoveAllOffsets();
        if (colIndex < 0 || colIndex >= ColumnsList.Count)
        {
            Debug.LogWarning("Remove Column says: Index out of Range!");
            return;
        }
        var children = ColumnsList[colIndex].Children;
        for (int i = 0; i < children.Count; i++)
        {
            DestroyImmediate(children[i].gameObject);
        }
        children.Clear();
        ColumnsList.RemoveAt(colIndex);
        Columns--;
        ReAddOffsets();
    }
    [ContextMenu("Remove last col!")]
    public void removelastcolumntest()
    {
        RemoveColumn(ColumnsList.Count - 1);
    }

    // TO DO: ADD REMOVE COLUMN FUNCTION. REDO THE CHILDREN SYSTEM AS FOLLOWS:
    // Class Column{ List<Child> Cells }
    // List<Column> Columns;

    // * The reason for this change is so that we can make column removal easier.

    //Another TO DO: STOP USING DESTROYIMMEDIATE WHEN DONE WITH UI TESTING
}
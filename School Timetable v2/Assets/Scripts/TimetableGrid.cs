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
    public bool Center = true;
    public bool FitContent = true;
    public TimetableChild TimetablePrefab;

    Vector2 PivotFix = Vector2.up;

    Vector2 originalPivot;

    [System.Serializable]
    public class Column
    {
        public bool isBreak;
        public List<TimetableChild> Children = new List<TimetableChild>();
    }
    public List<Column> ColumnsList;
    //public List<TimetableChild> Children = new();

    [ContextMenu("Set it up!!!")]
    public void Setup()
    {
        if(rect == null) rect = GetComponent<RectTransform>();

        FitToContent();
        originalPivot = rect.pivot;
        rect.pivot = PivotFix;

        ClearAll();
        
        for (int x = 0; x < Columns; x++)
        {
            ColumnsList.Add(new Column());

            // ^1 is basically ColumnsList.Count - 1
            UpdateColumnTransform(ColumnsList[^1], ColumnsList.Count - 1);
        }
        AddAllOffsets();
        rect.pivot = new(0.5f, 0.5f);
    }
    public void ClearAll()
    {
        if (ColumnsList.Count <= 0) return;
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                if (children[j] != null)
                    DestroyImmediate(children[j].gameObject);
            }
        }
        ColumnsList.Clear();
    }
    void FitToContent() // Call this, AFTER changing row/column count.
    {
        if (!FitContent) return;
        Vector2 wantedscale;

        wantedscale.x = Columns * CellSize.x + (Columns - 1) * Spacing.x;
        wantedscale.x += 10;

        wantedscale.y = Rows * CellSize.y + (Columns - 1) * Spacing.y;
        wantedscale.y += 8;

        rect.sizeDelta = wantedscale;
    }
    public Vector3 GetOffset()
    {
        if (!Center) return Vector3.zero;
        Vector3 offset = Vector3.zero;

        offset.x = Mathf.Abs(rect.sizeDelta.x - (Columns * CellSize.x + (Columns - 1) * Spacing.x)) / 2;
        offset.y = Mathf.Abs(rect.sizeDelta.y - (Rows * CellSize.y + (Rows - 1) * Spacing.y)) / -2;
        return offset;
    }
    
    public void RemoveAllOffsets()
    {
        if (!Center) return;
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                children[j].transform.localPosition -= GetOffset();
            }
        }
    }
    public void AddAllOffsets()
    {
        if (!Center) return;
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                children[j].transform.localPosition += GetOffset();
            }
        }
    }
    public void UpdateColumnTransform(Column column,int index)
    {
        if(column.Children.Count == 0)
        {
            for (int i = 0; i < Rows; i++)
            {
                column.Children.Add(null);
            }
        }

        for (int i = 0; i < column.Children.Count; i++)
        {
            // Basic Setup
            var c = column.Children;
            if (c[i] == null)
            {
                c[i] = Instantiate(TimetablePrefab, this.transform);
                c[i].rect.sizeDelta = CellSize;
            }
            c[i].transform.localPosition = Vector3.zero;
            Vector3 wantedpos = c[i].transform.localPosition;

            // Increasing readability
            var x = index;
            var y = i;

            // Setting the offset depending on CellSize
            wantedpos.x += CellSize.x * ((float)x + .5f);
            wantedpos.y -= CellSize.y * ((float)y + .5f);

            // Setting the offset depending on Spacing
            wantedpos.x += Spacing.x * x;
            wantedpos.y -= Spacing.y * y;
            c[i].transform.localPosition = wantedpos;
        }
    }
    public void UpdateBreakTransform(int index)
    {
        if (!ColumnsList[index].isBreak) return;
        TimetableChild t = ColumnsList[index].Children[0];

        t.transform.localPosition = Vector3.zero;
        t.rect.sizeDelta = CellSize;

        Vector3 wantedpos = t.transform.localPosition;
        Vector2 wantedscale = t.rect.sizeDelta;

        wantedpos.x += ((float)index + 1 - .5f) * CellSize.x;
        wantedpos.x += (index) * Spacing.x;

        // The row where the cell should be.
        float cellRowIndex = Mathf.FloorToInt((float)Rows / 2);

        if (Rows % 2 != 0)
            cellRowIndex += 0.5f;

        wantedpos.y -= (cellRowIndex) * CellSize.y;
        wantedpos.y -= (cellRowIndex - 1) * Spacing.y;
        wantedpos.y -= Spacing.y / 2;

        wantedscale.y = Rows * CellSize.y + (Rows - 1) * Spacing.y;

        t.rect.sizeDelta = wantedscale;
        t.transform.localPosition = wantedpos;
    }
    public void UpdateAllTransforms(int start)
    {
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            if (ColumnsList[i].isBreak)
            {
                UpdateBreakTransform(i);
                continue;
            }
            UpdateColumnTransform(ColumnsList[i], i);
        }
    }
    
    public void AddBreak(int columnIndex)
    {
        
        originalPivot = rect.pivot;
        rect.pivot = PivotFix;

        RemoveAllOffsets();

        Columns++;
        FitToContent();

        TimetableChild t = Instantiate(TimetablePrefab, this.transform);
        

        ColumnsList.Insert(columnIndex, new());
        ColumnsList[columnIndex].isBreak = true;
        ColumnsList[columnIndex].Children.Add(t);

        UpdateAllTransforms(columnIndex);

        UpdateBreakTransform(columnIndex);

        AddAllOffsets();

        rect.pivot = originalPivot;
    }
    public void AddColumn(int columnIndex)
    {
        
        originalPivot = rect.pivot;
        rect.pivot = PivotFix;

        RemoveAllOffsets();

        Columns++;
        FitToContent();

        ColumnsList.Insert(columnIndex, new());
        ColumnsList[columnIndex].isBreak = false;
        
        for (int i = 0; i < Rows; i++)
        {
            ColumnsList[columnIndex].Children.Add(null);
        }
        UpdateAllTransforms(columnIndex);

        AddAllOffsets();

        rect.pivot = originalPivot;
    }
    public void RemoveColumn(int colIndex)
    {
        
        originalPivot = rect.pivot;
        rect.pivot = PivotFix;

        RemoveAllOffsets();
        if (colIndex < 0 || colIndex >= ColumnsList.Count)
        {
            Debug.LogWarning("Remove Column says: Index out of Range!");
            return;
        }
        var children = ColumnsList[colIndex].Children;
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] != null)
                DestroyImmediate(children[i].gameObject);
        }
        children.Clear();
        ColumnsList.RemoveAt(colIndex);

        Columns--;
        FitToContent();

        UpdateAllTransforms(colIndex);
        AddAllOffsets();

        rect.pivot = originalPivot;
    }

    public void AddRow(int rowIndex)
    {
        
        originalPivot = rect.pivot;
        rect.pivot = PivotFix;

        RemoveAllOffsets();

        Rows++;
        FitToContent();

        for (int i = 0; i < ColumnsList.Count; i++)
        {
            if (ColumnsList[i].isBreak) continue;
            ColumnsList[i].Children.Insert(rowIndex, null);
        }
        UpdateAllTransforms(0);
        AddAllOffsets();

        rect.pivot = originalPivot;
    }
    public void RemoveRow(int rowIndex)
    {
        originalPivot = rect.pivot;
        rect.pivot = PivotFix;

        RemoveAllOffsets();
        
        Rows--;
        FitToContent();

        for (int i = 0; i < ColumnsList.Count; i++)
        {
            if (ColumnsList[i].isBreak) continue;
            DestroyImmediate(ColumnsList[i].Children[rowIndex].gameObject);
            ColumnsList[i].Children.RemoveAt(rowIndex);
        }
        UpdateAllTransforms(0);
        AddAllOffsets();

        rect.pivot = originalPivot;
    }
    
    
    [ContextMenu("Add Break!")]
    public void TestAddBreak() // temp
    {
        AddBreak(2);
    }
    [ContextMenu("Add Column!")]
    public void TestAddCol() // temp
    {
        AddColumn(2);
    }

    [ContextMenu("Remove Column!")]
    public void removelastcolumntest()
    {
        RemoveColumn(2);
    }
    
    [ContextMenu("New Row!")]
    public void testrow()
    {
        AddRow(1);
    }

    [ContextMenu("Remove Row!")]
    public void testrow2()
    {
        RemoveRow(1);
    }

    // TO DO: STOP USING DESTROYIMMEDIATE WHEN DONE WITH UI TESTING
}
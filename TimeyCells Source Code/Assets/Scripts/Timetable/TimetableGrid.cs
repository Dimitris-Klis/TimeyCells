using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimetableGrid : MonoBehaviour
{
    [Header("Parent Transform")]
    public RectTransform rect;

    [Space(20)]
    [Header("Cell Alignment")]
    public Vector2 CellSize;
    public Vector2 Spacing;

    [Space(20)]
    [Header("Parent Properties")]
    public int Rows;
    public int Columns;
    [Space]
    public int MaxRows = 7;
    public int MaxColumns = 40;

    [Space(20)]
    public bool Center = true;
    public bool FitContent = true;
    [Space]
    public Vector2 Padding;

    [Space(20)]
    public bool DebugGrid; // When debugging, destroyImmediate is called, instead of destroy.

    [Space(20)]
    [Header("References")]
    public TimetableCell TimetableCellPrefab;

    [Space(20)]
    [Header("Adding/Removing Columns UI")]
    public Button AddColButtonPrefab;
    public Button RemoveColButtonPrefab;
    [Space]
    public Transform ColButtonParent;
    public Vector2 ColumnButtonsOffset = -Vector2.up * 20;

    [Space(20)]
    [Header("Adding/Removing Rows UI")]
    public Button AddRowButtonPrefab;
    public Button RemoveRowButtonPrefab;
    [Space]
    public Transform RowButtonParent;
    public Vector2 RowButtonsOffset = Vector2.right * 20;
    

    [System.Serializable]
    public class Column
    {
        public bool IsMultirow;
        public List<TimetableCell> Children = new List<TimetableCell>();
    }

    [Header("Timetable Content")]
    public List<Column> ColumnsList = new();

    [Header("Add/Delete Buttons")]
    public List<Button> ColumnButtons = new();
    public List<Button> RowButtons = new();


    private void Start()
    {
        DebugGrid = false;
    }




    [ContextMenu("Setup Timetable Grid!")]
    public void Setup()
    {
        if (rect == null) rect = GetComponent<RectTransform>();

        ClearAllCells();

        for (int x = 0; x < Columns; x++)
        {
            ColumnsList.Add(new Column());

            // ^1 is basically ColumnsList.Count - 1
            UpdateColumnTransform(ColumnsList[^1], ColumnsList.Count - 1);
        }

        AddAllOffsets();

        FitToContent();
    }




    //The buttons which will add/delete columns
    public void SpawnAddColumnButtons(bool rowspan)
    {
        DestroyColumnButtons();
        if (Columns >= MaxColumns) return;

        var c = ColumnsList[0].Children[0];

        for (int i = 0; i <= Columns; i++)
        {
            Vector3 buttonPos = c.transform.localPosition;
            buttonPos.y = 0;
            buttonPos.x += -(CellSize.x + Spacing.x) + (CellSize.x * i) + (Spacing.x * i) + (CellSize.x / 2) + (Spacing.x / 2);
            
            Button b = Instantiate(AddColButtonPrefab, ColButtonParent);
            b.transform.localPosition = buttonPos;
            ColumnButtons.Add(b);

            // OnClick event
            int colIndex = i;
            if (!rowspan)
            {
                b.onClick.AddListener(delegate { AddColumn(colIndex); });
            }
            else
            {
                b.onClick.AddListener(delegate { AddMultirowColumn(colIndex); });
            }
        }
    }

    public void SpawnDeleteColumnButtons()
    {
        DestroyColumnButtons();
        if (ColumnsList.Count <= 1) return;

        var c = ColumnsList[0].Children[0];

        for (int i = 0; i < Columns; i++)
        {
            Vector3 buttonPos = c.transform.localPosition;
            buttonPos.y = 0;
            buttonPos.x += (CellSize.x * i) + (Spacing.x * i);
            Button b = Instantiate(RemoveColButtonPrefab, ColButtonParent);
            b.transform.localPosition = buttonPos;
            ColumnButtons.Add(b);

            // OnClick event
            int colIndex = i;
            b.onClick.AddListener(delegate { RemoveColumn(colIndex); });
        }
    }

    //The buttons which will add/delete rows
    public void SpawnAddRowButtons()
    {
        DestroyRowButtons();
        if (Rows >= MaxRows) return;

        var c = ColumnsList[0].Children[0];
        
        for (int i = 0; i <= Rows; i++)
        {
            Vector3 buttonPos = c.transform.localPosition;
            buttonPos.y -=  -(CellSize.y + Spacing.y) + (CellSize.y*i) + (Spacing.y * i) + (CellSize.y / 2) + Spacing.y / 2 ;
            buttonPos.x = 0;

            Button b = Instantiate(AddRowButtonPrefab, RowButtonParent);
            b.transform.localPosition = buttonPos;
            RowButtons.Add(b);

            // OnClick event
            int colIndex = i;
            b.onClick.AddListener(delegate { AddRow(colIndex); });
        }
    }

    public void SpawnDeleteRowButtons()
    {
        DestroyRowButtons();
        if (Rows <= 1) return;

        var c = ColumnsList[0].Children[0];

        for (int i = 0; i < Rows; i++)
        {
            Vector3 buttonPos = c.transform.localPosition;
            buttonPos.y -= (CellSize.y * i) + (Spacing.y * i);
            buttonPos.x = 0;

            Button b = Instantiate(RemoveRowButtonPrefab, RowButtonParent);
            b.transform.localPosition = buttonPos;
            RowButtons.Add(b);

            // OnClick event
            int colIndex = i;
            b.onClick.AddListener(delegate { RemoveRow(colIndex); });
        }
    }

    public void DestroyColumnButtons()
    {
        for (int i = 0; i < ColumnButtons.Count; i++)
        {
            Destroy(ColumnButtons[i].gameObject);
        }
        ColumnButtons.Clear();
    }

    public void DestroyRowButtons()
    {
        for (int i = 0; i < RowButtons.Count; i++)
        {
            Destroy(RowButtons[i].gameObject);
        }
        RowButtons.Clear();
    }




    public void ClearAllCells()
    {
        if (ColumnsList.Count <= 0) return;
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                if (children[j] != null)
                {
                    if (DebugGrid)
                    {
                        DestroyImmediate(children[j].gameObject);
                    }
                    else
                    {
                        Destroy(children[j].gameObject);
                    }
                }
                    
            }
        }
        ColumnsList.Clear();
    }

    public void UpdateAllCells()
    {
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            for (int j = 0; j < ColumnsList[i].Children.Count; j++)
            {
                ColumnsList[i].Children[j].Info.UpdateUI();
            }
        }
    }

    void FitToContent() // Call this, AFTER changing row/column count.
    {
        if (!FitContent) return;
        Vector2 wantedscale;

        wantedscale.x = Columns * CellSize.x + (Columns - 1) * Spacing.x;
        wantedscale.x += Padding.x;

        wantedscale.y = (Rows+1) * CellSize.y + (Rows-1) * Spacing.y;
        wantedscale.y += Padding.y;

        rect.sizeDelta = wantedscale;
    }




    public Vector3 GetOffset()
    {
        if (!Center) return Vector3.zero;
        Vector3 offset = Vector3.zero;

        offset.x -= Columns * CellSize.x;
        offset.x -= (Columns-1) * Spacing.x;
        offset.x /= 2;

        offset.y += Rows * CellSize.y;
        offset.y += (Rows - 1) * Spacing.y;
        offset.y /= 2;

        return offset;
    }
    
    public void AddAllOffsets()
    {
        if (!Center) return;
        Vector3 offset = GetOffset();
        
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                children[j].transform.localPosition += offset;
            }
        }
    }

    public void RemoveAllOffsets()
    {
        if (!Center) return;
        Vector3 offset = GetOffset();

        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var children = ColumnsList[i].Children;
            for (int j = 0; j < children.Count; j++)
            {
                children[j].transform.localPosition -= offset;
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
                c[i] = Instantiate(TimetableCellPrefab, this.transform);
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

    public void UpdateMultirowTransform(int index)
    {
        if (!ColumnsList[index].IsMultirow) return;
        TimetableCell t = ColumnsList[index].Children[0];

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
            if (ColumnsList[i].IsMultirow)
            {
                UpdateMultirowTransform(i);
                continue;
            }
            UpdateColumnTransform(ColumnsList[i], i);
        }
    }




    public void AddColumn(int columnIndex)
    {
        RemoveAllOffsets();

        Columns++;
        FitToContent();

        ColumnsList.Insert(columnIndex, new());
        ColumnsList[columnIndex].IsMultirow = false;

        for (int i = 0; i < Rows; i++)
        {
            ColumnsList[columnIndex].Children.Add(null);
        }
        UpdateAllTransforms(columnIndex);

        AddAllOffsets();

        if (TimetableEditor.instance.Editing)
            SpawnAddColumnButtons(false);

        UpdateAllCells();

        DayTimeManager.instance.AddIndexLabel(columnIndex);
        DayTimeManager.instance.UpdateTimeIndexes();
    }

    public void AddMultirowColumn(int columnIndex)
    {
        RemoveAllOffsets();

        Columns++;
        FitToContent();

        TimetableCell t = Instantiate(TimetableCellPrefab, this.transform);
        

        ColumnsList.Insert(columnIndex, new());
        ColumnsList[columnIndex].IsMultirow = true;
        ColumnsList[columnIndex].Children.Add(t);

        UpdateAllTransforms(columnIndex);

        UpdateMultirowTransform(columnIndex);

        AddAllOffsets();

        if (TimetableEditor.instance.Editing)
            SpawnAddColumnButtons(true);
        UpdateAllCells();

        DayTimeManager.instance.UpdateTimeIndexes();
    }
    
    public void RemoveColumn(int columnIndex)
    {
        RemoveAllOffsets();
        if (columnIndex < 0 || columnIndex >= ColumnsList.Count)
        {
            Debug.LogWarning("Remove Column says: Index out of Range!");
            return;
        }
        var children = ColumnsList[columnIndex].Children;
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] != null)
            {
                if (DebugGrid)
                {
                    DestroyImmediate(children[i].gameObject);
                }
                else
                {
                    Destroy(children[i].gameObject);
                }
            }
        }
        children.Clear();
        ColumnsList.RemoveAt(columnIndex);

        Columns--;
        FitToContent();

        UpdateAllTransforms(columnIndex);
        AddAllOffsets();

        if (TimetableEditor.instance.Editing)
            SpawnDeleteColumnButtons();

        UpdateAllCells();
        
        DayTimeManager.instance.RemoveIndexLabel(columnIndex);
        DayTimeManager.instance.UpdateTimeIndexes();
    }

    public void SwapColumns(int IndexA, int IndexB)
    {
        Column columnA = ColumnsList[IndexA];
        ColumnsList[IndexA] = ColumnsList[IndexB];
        ColumnsList[IndexB] = columnA;

        RemoveAllOffsets();
        if (!ColumnsList[IndexA].IsMultirow)
            UpdateColumnTransform(ColumnsList[IndexA], IndexA);
        else
            UpdateMultirowTransform(IndexA);
        if (!ColumnsList[IndexB].IsMultirow)
            UpdateColumnTransform(ColumnsList[IndexB], IndexB);
        else
            UpdateMultirowTransform(IndexB);
        AddAllOffsets();

        // Swapping labels
        DayTimeManager.instance.SwapIndexLabels(IndexA, IndexB);
    }

    // This is only used for loading saves.
    public void ReplaceColumnWithMultirowAt(int index)
    {
        RemoveColumn(index);
        AddMultirowColumn(index);
    }




    public void AddRow(int rowIndex)
    {
        RemoveAllOffsets();

        Rows++;
        FitToContent();

        for (int i = 0; i < ColumnsList.Count; i++)
        {
            if (ColumnsList[i].IsMultirow) continue;
            ColumnsList[i].Children.Insert(rowIndex, null);
        }
        UpdateAllTransforms(0);
        AddAllOffsets();


        if (TimetableEditor.instance.Editing)
            SpawnAddRowButtons();
        UpdateAllCells();
        DayTimeManager.instance.AddNewWeekday(rowIndex);
    }

    public void RemoveRow(int rowIndex)
    {
        RemoveAllOffsets();
        
        Rows--;
        FitToContent();

        for (int i = 0; i < ColumnsList.Count; i++)
        {
            if (ColumnsList[i].IsMultirow) continue;
            if (DebugGrid)
            {
                DestroyImmediate(ColumnsList[i].Children[rowIndex].gameObject);
            }
            else
            {
                Destroy(ColumnsList[i].Children[rowIndex].gameObject);
            }
            ColumnsList[i].Children.RemoveAt(rowIndex);
        }
        UpdateAllTransforms(0);
        AddAllOffsets();

        if (TimetableEditor.instance.Editing)
            SpawnDeleteRowButtons();
        UpdateAllCells();
        DayTimeManager.instance.RemoveWeekday(rowIndex);
    }
    
    public void SwapRows(int IndexA, int IndexB)
    {
        RemoveAllOffsets();
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            if (ColumnsList[i].IsMultirow) continue;
            TimetableCell IndexATemp = ColumnsList[i].Children[IndexA];

            ColumnsList[i].Children[IndexA] = ColumnsList[i].Children[IndexB];
            ColumnsList[i].Children[IndexB] = IndexATemp;

            UpdateColumnTransform(ColumnsList[i], i);
        }
        AddAllOffsets();

        // Swapping weekdays
        DayTimeManager.instance.SwapWeekDays(IndexA, IndexB);
    }
}
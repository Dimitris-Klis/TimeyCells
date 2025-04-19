using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool DebugGrid; // When debugging, destroyImmediate is called, instead of destroy.
    public bool Center = true;
    public bool FitContent = true;
    public Vector2 Padding;
    [Space]
    public TimetableCell TimetablePrefab;
    public Button AddColButtonPrefab;
    public Button RemoveColButtonPrefab;
    public Vector2 ColumnButtonsOffset = -Vector2.up * 20;


    Vector2 PivotFix = Vector2.up;
    Vector2 originalPivot;

    [System.Serializable]
    public class Column
    {
        public bool isBreak;
        public List<TimetableCell> Children = new List<TimetableCell>();
    }
    public List<Column> ColumnsList = new();
    public List<Button> ColumnButtons = new();

    public void Start()
    {
        DebugGrid = false;
    }

    [ContextMenu("Set it up!!!")]
    public void Setup()
    {
        if (rect == null) rect = GetComponent<RectTransform>();

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
    //The buttons which will add columns
    public void SetupAddColumnButtons(bool colspan)
    {
        RemoveColumnButtons();
        for (int i = 0; i <= ColumnsList.Count; i++)
        {
            if (i == ColumnsList.Count)
            {
                var finalc = ColumnsList[^1].Children[0];
                Vector3 finalbuttonPos = finalc.transform.localPosition;
                finalbuttonPos.y = rect.sizeDelta.y / 2;
                finalbuttonPos.x += CellSize.x / 2 + Spacing.x / 2;

                Button finalb = Instantiate(AddColButtonPrefab, this.transform);
                finalb.transform.localPosition = finalbuttonPos + (Vector3)ColumnButtonsOffset;
                ColumnButtons.Add(finalb);

                // OnClick event
                int finalIndex = ColumnsList.Count;
                if (!colspan)
                {
                    finalb.onClick.AddListener(delegate { AddColumn(finalIndex); });
                }
                else
                {
                    finalb.onClick.AddListener(delegate { AddBreak(finalIndex); });
                }
                continue;
            }
            var c = ColumnsList[i].Children[0];
            Vector3 buttonPos = c.transform.localPosition;
            buttonPos.y = rect.sizeDelta.y / 2;
            buttonPos.x -= CellSize.x / 2 + Spacing.x / 2;
            
            Button b = Instantiate(AddColButtonPrefab, this.transform);
            b.transform.localPosition = buttonPos + (Vector3)ColumnButtonsOffset;
            ColumnButtons.Add(b);

            // OnClick event
            int colIndex = i;
            if (!colspan)
            {
                b.onClick.AddListener(delegate { AddColumn(colIndex); });
            }
            else
            {
                b.onClick.AddListener(delegate { AddBreak(colIndex); });
            }
        }
    }
    public void SetupDeleteColumnButtons()
    {
        RemoveColumnButtons();
        if (ColumnsList.Count == 1) return;
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            var c = ColumnsList[i].Children[0];
            Vector3 buttonPos = c.transform.localPosition;
            buttonPos.y = rect.sizeDelta.y / 2;

            Button b = Instantiate(RemoveColButtonPrefab, this.transform);
            b.transform.localPosition = buttonPos + (Vector3)ColumnButtonsOffset;
            ColumnButtons.Add(b);

            // OnClick event
            int colIndex = i;
            b.onClick.AddListener(delegate { RemoveColumn(colIndex); });
        }
    }
    public void RemoveColumnButtons()
    {
        for (int i = 0; i < ColumnButtons.Count; i++)
        {
            Destroy(ColumnButtons[i].gameObject);
        }
        ColumnButtons.Clear();
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
    void FitToContent() // Call this, AFTER changing row/column count.
    {
        if (!FitContent) return;
        Vector2 wantedscale;

        wantedscale.x = Columns * CellSize.x + (Columns - 1) * Spacing.x;
        wantedscale.x += Padding.x;

        wantedscale.y = Rows * CellSize.y + (Columns - 1) * Spacing.y;
        wantedscale.y += Padding.y;

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

        TimetableCell t = Instantiate(TimetablePrefab, this.transform);
        

        ColumnsList.Insert(columnIndex, new());
        ColumnsList[columnIndex].isBreak = true;
        ColumnsList[columnIndex].Children.Add(t);

        UpdateAllTransforms(columnIndex);

        UpdateBreakTransform(columnIndex);

        AddAllOffsets();

        rect.pivot = originalPivot;
        SetupAddColumnButtons(true);

        UpdateAllCells();
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
        SetupAddColumnButtons(false);

        UpdateAllCells();
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
        ColumnsList.RemoveAt(colIndex);

        Columns--;
        FitToContent();

        UpdateAllTransforms(colIndex);
        AddAllOffsets();

        rect.pivot = originalPivot;

        SetupDeleteColumnButtons();

        UpdateAllCells();
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

        rect.pivot = originalPivot;
    }
    public void UpdateAllCells()
    {
        for (int i = 0; i < ColumnsList.Count; i++)
        {
            for (int j = 0; j < ColumnsList[i].Children.Count; j++)
            {
                var c = ColumnsList[i].Children[j];
                c.Info.UpdateUI();

            }
        }
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
}
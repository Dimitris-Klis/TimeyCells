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
    public bool delprev; // A temp variable meant for testing & debugging the column adding function.
    public TimetableChild TimetablePrefab;
    public List<TimetableChild> Children = new();

    [ContextMenu("Set it up!!!")]
    public void Setup()
    {
        if(rect == null) rect = GetComponent<RectTransform>();
        ClearAll();
        
        for (int x = 0; x < Columns; x++)
        {
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
                Children.Add(t);
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
            LongColTest = t;
            Children.Add(t);
        }
        if(Center) ReAddOffsets();
    }
    public void RemoveAllOffsets()
    {
        for (int i = 0; i < Children.Count; i++)
        {
            Children[i].transform.localPosition -= GetOffset();
        }
    }
    public void ReAddOffsets()
    {
        for (int i = 0; i < Children.Count; i++)
        {
            Children[i].transform.localPosition += GetOffset();
        }
    }
    int prevcols;
    TimetableChild LongColTest;
    [ContextMenu("Test Break!")]
    public void TestAddBreak() // temp
    {
        if (delprev)
        {
            
            if (LongColTest != null)
            {
                DestroyImmediate(LongColTest.gameObject);
                Children.RemoveAt(Children.Count - 1);
                LongColTest = null;
            }
            prevcols = Columns;
        }
        
        
        AddColumn(true);
        if (delprev)
            Columns = prevcols;
    }
    public void ClearAll()
    {
        if (Children.Count > 0)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                DestroyImmediate(Children[i].gameObject);
            }
            Children.Clear();
        }
    }

    // TO DO: ADD REMOVE COLUMN FUNCTION. REDO THE CHILDREN SYSTEM AS FOLLOWS:
    // Class Column{ List<Child> Cells }
    // List<Column> Columns;

    // * The reason for this change is so that we can make column removal easier.
}
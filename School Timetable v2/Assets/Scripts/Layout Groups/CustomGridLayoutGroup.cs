using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomGridLayoutGroup : MonoBehaviour
{
    public int Rows, Columns;
    public Vector2 Spacing;
    public Vector2 Size;
    public List<int> MultirowIndexes = new();

    // HeaderSize.y: First Row Height
    // HeaderSize.x: First Column Width
    public Vector2 HeaderSize;
    [Space]
    public RectTransform SelfRect;

    [ContextMenu("UpdateLayout")]
    public void UpdateLayout()
    {
        int indexOffset = 0;
        Vector2 pivot = Vector2.zero;
        if (transform.childCount == 0) return;
        foreach (RectTransform child in transform)
        {
            int index = child.GetSiblingIndex();
            
            int col = Mathf.FloorToInt((index + indexOffset) / Rows);
            int row = (index + indexOffset) - (col * Rows);

            Vector2 wantedPos = Vector2.zero;
            child.sizeDelta = Size;
            if (col == 0)
            {
                child.sizeDelta = new(HeaderSize.x, child.sizeDelta.y);
            }
            else
            {
                child.sizeDelta = new(Size.x, child.sizeDelta.y);
            }
            if(row == 0)
            {
                child.sizeDelta = new(child.sizeDelta.x, HeaderSize.y);
            }
            else
            {
                child.sizeDelta = new(child.sizeDelta.x, Size.y);
            }
            if (index == 0)
            {
                pivot.y -= HeaderSize.y / 2;
                pivot.x += HeaderSize.x / 2;
            }
            else
            {
                
                if (col > 0 && row > 0)
                {
                    wantedPos.x += (HeaderSize.x - Size.x) / 2;
                    wantedPos.y -= (HeaderSize.y - Size.y) / 2;

                    wantedPos.x += Size.x * col + (Spacing.x * (col));
                    wantedPos.y -= Size.y * row + (Spacing.y * (row));
                }
                else
                {
                    if (col == 0 && row != 0)
                    {
                        wantedPos.y -= (HeaderSize.y - Size.y) / 2;
                        wantedPos.y -= Size.y * row + (Spacing.y * (row));
                    }
                    else if (row == 0 && col != 0)
                    {
                        wantedPos.x += Size.x * col + (Spacing.x * (col));
                        wantedPos.x += (HeaderSize.x - Size.x) / 2;
                    }
                    else
                    {
                        wantedPos = Vector2.zero;
                    }   
                }

                
            }
            //if(index == indexToDebug)
            //{
            //    Debug.Log($"Col=({index} + {indexOffset}) / {Rows}   =   {col}");
            //    Debug.Log($"Row={index}+{rowsOffset}-{col}*{Rows}   =   {row}");
                
            //}
            if (MultirowIndexes.Contains(index))
            {
                child.sizeDelta = new(child.sizeDelta.x, (Rows - 1) * Size.y + (Rows - 2) * Spacing.y);
                wantedPos.y = - (HeaderSize.y - child.sizeDelta.y) / 2 - child.sizeDelta.y - Spacing.y;

                wantedPos.x = Size.x * (col) + (Spacing.x * (col));
                wantedPos.x += (HeaderSize.x - Size.x) / 2;

                indexOffset+= Rows - row - 1;
            }
            child.localPosition = pivot + wantedPos;
        }

        // Centering

        Vector2 totalSize = Vector2.zero;
        totalSize.x -= HeaderSize.x;
        totalSize.y += HeaderSize.y;

        totalSize.x -= (Columns - 1) * Size.x;
        totalSize.y += (Rows - 1) * Size.y;

        totalSize.x -= (Columns - 1) * Spacing.x;
        totalSize.y += (Rows - 1) * Spacing.y;

        foreach (RectTransform child in transform)
        {
            child.localPosition += (Vector3)totalSize / 2;
        }

        // Fitting to content
        SelfRect.sizeDelta = new(Mathf.Abs(totalSize.x), Mathf.Abs(totalSize.y));
    }
}
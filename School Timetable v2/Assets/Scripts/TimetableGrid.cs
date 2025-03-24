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
    public TimetableChild TimetablePrefab;
    public List<TimetableChild> Children = new();

    [ContextMenu("Set it up!!!")]
    public void Setup()
    {
        if(rect == null) rect = GetComponent<RectTransform>();

        if(Children.Count > 0)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                DestroyImmediate(Children[i].gameObject);
            }
            Children.Clear();
        }
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                TimetableChild t = Instantiate(TimetablePrefab, this.transform);
                t.transform.localPosition = Vector3.zero;

                t.rect.sizeDelta = CellSize;
                
                Vector2 wantedpos = t.transform.localPosition;

                // Setting the offset depending on CellSize
                wantedpos.x += CellSize.x * ((float)x+.5f);
                wantedpos.y -= CellSize.y * ((float)y + .5f);

                // Setting the offset depending on Spacing
                wantedpos.x += Spacing.x * x;
                wantedpos.y -= Spacing.y * y;

                // The offset needed to center the UI (hopefully).
                Vector2 offset = Vector2.zero;
                Debug.Log((Columns * CellSize.x + (Columns - 1) * Spacing.x));
                offset.x = Mathf.Abs(rect.sizeDelta.x - (Columns * CellSize.x + (Columns - 1) * Spacing.x)) / 2;
                offset.y = Mathf.Abs(rect.sizeDelta.y - (Rows * CellSize.y + (Rows - 1) * Spacing.y)) / -2;
                t.transform.localPosition = wantedpos + offset;
                Children.Add(t);
            }
        }
    }
}

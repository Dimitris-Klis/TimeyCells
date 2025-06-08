using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomLayoutGroup : MonoBehaviour
{
    public enum AlignmentModes { Horizontal, Vertical}
    public AlignmentModes AlignmentMode;

    public RectTransform SelfRect;
    [Space]
    public bool AffectCellSizeX;
    public float CellSizeX;
    [Space]
    public bool AffectCellSizeY;
    public float CellSizeY;
    [Space]
    public float Spacing;
    [ContextMenu("UpdateLayout")]
    public void UpdateLayout()
    {
        float wantedPos = 0;
        RectTransform prevChild = SelfRect; // Temporary assignment to avoid errors.
        if (transform.childCount == 0) return;
        foreach (RectTransform child in transform)
        {
            Vector2 newSizeDelta = child.sizeDelta;
            int index = child.GetSiblingIndex();
            
            if (AffectCellSizeX)
            {
                newSizeDelta.x = CellSizeX;
            }
            if (AffectCellSizeY)
            {
                newSizeDelta.y = CellSizeY;
            }
            child.sizeDelta = newSizeDelta;

            if (index == 0)
            {
                if (AlignmentMode == AlignmentModes.Horizontal)
                {
                    wantedPos += child.sizeDelta.x / 2;
                }
                else
                {
                    wantedPos -= child.sizeDelta.y / 2;
                }
            }
            else
            {
                if (AlignmentMode == AlignmentModes.Horizontal)
                {
                    wantedPos += child.sizeDelta.x + (prevChild.sizeDelta.x - child.sizeDelta.x) / 2;
                }
                else
                {
                    wantedPos -= child.sizeDelta.y + (prevChild.sizeDelta.y - child.sizeDelta.y) / 2;
                }
            }

            if (AlignmentMode == AlignmentModes.Horizontal)
            {
                child.localPosition = Vector2.right * wantedPos;
            }
            else
            {
                child.localPosition = Vector2.up * wantedPos;
            }
            prevChild = child;
            if (AlignmentMode == AlignmentModes.Horizontal)
            {
                wantedPos += Spacing;
            }
            else
            {
                wantedPos -= Spacing;
            }
        }

        // Centering

        float totalSize = 0;
        foreach (RectTransform child in transform)
        {
            if (AlignmentMode == AlignmentModes.Horizontal)
                totalSize += child.sizeDelta.x;
            else
                totalSize += child.sizeDelta.y;
        }
        totalSize += Spacing * (transform.childCount - 1);

        foreach (RectTransform child in transform)
        {
            if (AlignmentMode == AlignmentModes.Horizontal)
                child.localPosition -= Vector3.right * totalSize / 2;
            else
                child.localPosition += Vector3.up * totalSize / 2;
        }
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
    [ContextMenu("TryLayoutGroup")]
    public void UpdateLayout()
    {
        Vector2 wantedPos = Vector2.zero;
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

            


            if (index != 0)
            {
                if (AlignmentMode == AlignmentModes.Horizontal)
                {
                    wantedPos.x += child.sizeDelta.x;
                    wantedPos += Vector2.right * Spacing;
                }
                else
                {
                    wantedPos.y -= child.sizeDelta.y;
                    wantedPos -= Vector2.up * Spacing;
                }
            }
            else
            {
                if (AlignmentMode == AlignmentModes.Horizontal)
                {
                    wantedPos.x += child.sizeDelta.x / 2;
                }
                else
                {
                    wantedPos.y -= child.sizeDelta.y / 2;
                }
            }
            child.localPosition = wantedPos;
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
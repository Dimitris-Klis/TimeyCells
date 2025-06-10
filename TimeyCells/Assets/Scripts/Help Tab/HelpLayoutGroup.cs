using System.Collections.Generic;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
[ExecuteAlways]
public class HelpLayoutGroup : MonoBehaviour
{
    public RectTransform SelfRect;
    [Space]
    public float Spacing;
    public float PaddingX = 5;
    public bool CenterY = true;
    public List<RectTransform> children = new();

    public void UpdateLayout()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        DelayedUpdateLayout();
    }
    public void DelayedUpdateLayout()
    {
        float wantedPos = 0;
        RectTransform prevChild = SelfRect; // Temporary assignment to avoid errors.
        if (transform.childCount == 0) return;
        foreach (RectTransform child in children)
        {
            int index = child.GetSiblingIndex();

            if (index == 0)
            {
                wantedPos -= child.sizeDelta.y / 2;
            }
            else
            {
                wantedPos -= child.sizeDelta.y + (prevChild.sizeDelta.y - child.sizeDelta.y) / 2;
            }

            
            child.localPosition = Vector2.up * wantedPos;
            
            prevChild = child;

            wantedPos -= Spacing;
            
        }

        // Centering

        float totalSize = 0;
        foreach (RectTransform child in children)
        {
            totalSize += child.sizeDelta.y;
        }
        totalSize += Spacing * (transform.childCount - 1);

        // Fit to content

        SelfRect.sizeDelta = new(SelfRect.sizeDelta.x, totalSize);

        foreach (RectTransform child in children)
        {
            if (CenterY)
                child.localPosition += Vector3.up * totalSize / 2;

            float xDiff = SelfRect.sizeDelta.x - child.sizeDelta.x;
            float extraPadding = xDiff > 0 ? PaddingX : 0;
            child.localPosition = new Vector3(-(xDiff) / 2 + extraPadding, child.localPosition.y);
        }
    }
}
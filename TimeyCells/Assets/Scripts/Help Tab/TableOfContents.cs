using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TableOfContents : MonoBehaviour
{
    public RectTransform SelfRect;
    [Space]
    [SerializeField] int H1Size = 150;
    [SerializeField] int H2Size = 120;
    [SerializeField] int H3Size = 110;
    [Space]
    public float Spacing;
    public ContentButton TabPrefab;
    public HelpSection helpSection;
    public bool CenterChildren;
    public struct HeaderShortcut
    {
        public string name;
        public int level;
        public HeaderShortcut(string _name, int _level)
        {
            name = _name;
            level = _level;
        }
    }
    public List<HeaderShortcut> headers = new List<HeaderShortcut>();
    public List<ContentButton> buttons = new List<ContentButton>();
    public void Setup()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(!Application.isPlaying)
                DestroyImmediate(buttons[i].gameObject);
            else
                Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
        int number = 1;
        for (int i = 0; i < headers.Count; i++)
        {
            ContentButton b = Instantiate(TabPrefab, transform);
            buttons.Add(b);

            string headerSize = "";
            switch (headers[i].level)
            {
                case 0:
                    headerSize = $"<size={H1Size}%>{number}. <indent=15%>";
                    // b.button.interactable = false;
                    number++;
                    break;
                case 1:
                    headerSize = $"      <size={H2Size}%>\u2022 <indent=21%>";
                    break;
                default:
                    headerSize = $"            <size={H3Size}%>\u25E6 <indent=31%>";
                    break;
            }

            b.text.text = headerSize + headers[i].name + "</size>";
        }

        UpdateLayout();
    }
    [ContextMenu("UpdateLayout")]
    public void UpdateLayout()
    {
        float wantedPos = 0;
        RectTransform prevChild = SelfRect; // Temporary assignment to avoid errors.
        if (transform.childCount == 0) return;
        foreach (ContentButton b in buttons)
        {
            Vector2 newSizeDelta = b.selfRect.sizeDelta;
            int index = b.selfRect.GetSiblingIndex();

            b.selfRect.sizeDelta = newSizeDelta;

            if (index == 0)
            {
                wantedPos -= b.selfRect.sizeDelta.y / 2;
            }
            else
            {
                wantedPos -= b.selfRect.sizeDelta.y + (prevChild.sizeDelta.y - b.selfRect.sizeDelta.y) / 2;
            }


            b.selfRect.localPosition = Vector2.up * wantedPos;
            prevChild = b.selfRect;
            wantedPos -= Spacing;
        }

        // Centering

        float totalSize = 0;
        foreach (RectTransform child in transform)
        {
            totalSize += child.sizeDelta.y;
        }
        totalSize += Spacing * (transform.childCount - 1);



        // Fitting to content
        SelfRect.sizeDelta = new(SelfRect.sizeDelta.x, Mathf.Abs(totalSize));

        if (!CenterChildren) return;
        
        foreach (RectTransform child in transform)
        {
            child.localPosition += Vector3.up * totalSize / 2;
        }
    }
}

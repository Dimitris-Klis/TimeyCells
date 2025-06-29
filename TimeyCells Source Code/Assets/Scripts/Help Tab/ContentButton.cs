using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContentButton : MonoBehaviour
{
    [Header("Self Rect")]
    public RectTransform selfRect;

    [Space(20)]
    [Header("UI Components")]
    public Button button;
    public TMP_Text text;
}
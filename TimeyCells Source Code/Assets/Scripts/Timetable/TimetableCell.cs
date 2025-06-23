using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimetableCell : MonoBehaviour
{
    [Header("Multirow")]
    public bool IsMultirow;

    [Space(20)]
    [Header("Rect References")]
    public RectTransform rect;
    public Button SelfButton;

    [Space(20)]
    [Header("UI References")]
    public Image BackgroundImage;
    public TMP_Text EventNameText;
    public TMP_Text Info1Text;
    public TMP_Text Info2Text;
    public Image FavouriteImage;

    [Space(20)]
    [Header("Cell Info")]
    public CellInfo Info;
}
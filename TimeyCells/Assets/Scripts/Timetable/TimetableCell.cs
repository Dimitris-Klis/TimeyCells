using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimetableCell : MonoBehaviour
{
    public bool IsMultirow;
    public RectTransform rect;
    [Space]
    public Image BackgroundImage;
    public TMP_Text EventNameText;
    public TMP_Text Info1Text;
    public TMP_Text Info2Text;
    public Image FavouriteImage;
    [Space]
    public CellInfo Info;
    public Button SelfButton;
}

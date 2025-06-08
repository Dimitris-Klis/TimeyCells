using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmationManager : MonoBehaviour
{
    public static ConfirmationManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject ConfirmationOverlay;
    public TMP_Text TMP_Title;
    public TMP_Text TMP_Desc;

    public Button ButtonPrefab;
    public ContentSizeFitter DescriptionTextSizeFitter;
    public CenterAndFit DescriptionParent;
    public Transform ButtonsParent;
    public List<Button> Buttons = new();
    public class ButtonPrompt
    {
        public string ButtonName;
        public UnityAction Action;

        public ButtonPrompt(string text, UnityAction action)
        {
            ButtonName = text;
            Action = action;
        }
    }

    public void ShowConfirmation(string title, string desc, params ButtonPrompt[] buttons)
    {
        ConfirmationOverlay.SetActive(true);
        TMP_Title.text = title;
        TMP_Desc.text = desc;

        for (int i = 0; i < Buttons.Count; i++)
        {
            Destroy(Buttons[i].gameObject);
        }
        Buttons.Clear();

        for (int i = 0; i < buttons.Length; i++)
        {
            Button b = Instantiate(ButtonPrefab, ButtonsParent);
            Buttons.Add(b);
            b.GetComponentInChildren<TMP_Text>().text = buttons[i].ButtonName;
            if (buttons[i].Action != null)
                b.onClick.AddListener(buttons[i].Action);
            b.onClick.AddListener(delegate { ConfirmationOverlay.SetActive(false); });
        }

        StartCoroutine(PrepareLayout());
    }
    IEnumerator PrepareLayout()
    {
        DescriptionTextSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return null;
        DescriptionTextSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        yield return null;
        DescriptionParent.UpdateLayout();
    }
}

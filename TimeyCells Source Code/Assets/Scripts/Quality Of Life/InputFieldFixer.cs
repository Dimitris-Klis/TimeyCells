using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class InputFieldFixer : MonoBehaviour
{
    public TMP_InputField TMPField;
    public uint CharacterLimit;
    bool changed;
    public void CheckIfEmpty()
    {
        if (TMPField.text == TMP_Specials.clear) TMPField.text = string.Empty;
        else
        {
            if (CharacterLimit == 0) return;
            string textWithoutClear = TMPField.text.Replace(TMP_Specials.clear, "");
            if (textWithoutClear.Length > CharacterLimit)
                TMPField.text = textWithoutClear.Remove((int)CharacterLimit, textWithoutClear.Length - (int)CharacterLimit) + TMP_Specials.clear;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TMPField.onValueChanged.AddListener(delegate {  CheckIfEmpty(); });
    }
}
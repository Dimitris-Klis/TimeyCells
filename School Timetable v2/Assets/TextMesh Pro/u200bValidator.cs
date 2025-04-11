using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "InputValidator - U200B Fix.asset", menuName = "TextMeshPro/Input Validators/U+200B Fix", order = 101)]
public class u200bValidator : TMP_InputValidator
{
    // Custom text input validation function
    public override char Validate(ref string text, ref int pos, char ch)
    {
        HideCharacter(ref text, ref pos, ch);
        return ch;
    }
    public void HideCharacter(ref string text, ref int pos, char ch)
    {
        text = text.Replace(TMP_Specials.clear, "");
        if (pos >= text.Length && text.Length > 0) pos = text.Length;

        text = text.Insert(pos, ch.ToString());
        pos += 1;
        text += TMP_Specials.clear;
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "InputValidator - HexCodeValidator.asset", menuName = "TextMeshPro/Input Validators/HexCode", order = 100)]
public class HexCodeValidator : TMP_InputValidator
{
    // Custom text input validation function
    public override char Validate(ref string text, ref int pos, char ch)
    {
        bool validLetter = (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
        bool canHashtag = ch == '#' && pos == 0;

        if ((validLetter || canHashtag) && pos <= 8)
        {
            text = text.Insert(pos, ch.ToString());
            pos += 1;
            return ch;
        }
        return (char)0;
    }
    bool ContainsHashtag(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '#') return true;
        }
        return false;
    }
}
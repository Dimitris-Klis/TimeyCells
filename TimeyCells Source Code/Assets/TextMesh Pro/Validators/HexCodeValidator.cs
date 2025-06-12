using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "InputValidator - HexCodeValidator.asset", menuName = "TextMeshPro/Input Validators/HexCode", order = 100)]
public class HexCodeValidator : u200bValidator
{
    // Custom text input validation function
    public override char Validate(ref string text, ref int pos, char ch)
    {
        bool validLetter = (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
        bool canHashtag = ch == '#' && pos == 0;

        if (ch == '>' && text[8] =='c') text = text.Remove(8);

        if ((validLetter || canHashtag) && pos <= 8)
        {
            //text = text.Insert(pos, ch.ToString());
            //pos += 1;
            HideCharacter(ref text, ref pos, ch);
            return ch;
        }
        return (char)0;
    }
}
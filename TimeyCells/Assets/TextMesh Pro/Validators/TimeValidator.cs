using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "InputValidator - TimeValidator.asset", menuName = "TextMeshPro/Input Validators/Time", order = 100)]
public class TimeValidator : u200bValidator
{
    // Custom text input validation function
    public override char Validate(ref string text, ref int pos, char ch)
    {
        bool validNumber = (ch >= '0' && ch <= '9');
        bool validLetter = ch == 'P' || ch == 'A' || ch == 'M' || ch == 'p' || ch == 'a' || ch == 'm' || ch == ':' || ch == '.' || ch == ' ';

        if (ch == '>' && text[8] == 'c') text = text.Remove(8);

        if ((validNumber || validLetter))
        {
            //text = text.Insert(pos, ch.ToString());
            //pos += 1;
            HideCharacter(ref text, ref pos, ch);
            return ch;
        }
        return (char)0;
    }
}

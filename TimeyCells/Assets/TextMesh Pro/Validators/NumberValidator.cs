using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "InputValidator - NumberValidator.asset", menuName = "TextMeshPro/Input Validators/Numbers", order = 100)]
public class NumberValidator : u200bValidator
{
    // Custom text input validation function
    public override char Validate(ref string text, ref int pos, char ch)
    {
        bool validNumber = (ch >= '0' && ch <= '9');
        if (validNumber)
        {
            HideCharacter(ref text, ref pos, ch);
            return ch;
        }
        return (char)0;
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "InputValidator - TimeValidator.asset", menuName = "TextMeshPro/Input Validators/Time", order = 100)]
public class TimeValidator : ValidatorBase
{
    // Custom text input validation function
    public override char Validate(ref string text, ref int pos, char ch)
    {
        // Unfortunately, when manually validating, we also need to check for the character limit manually :/
        if (InputField != null) if(InputField.characterLimit > 0 && text.Length >= InputField.characterLimit) return (char)0;

        bool validNumber = (ch >= '0' && ch <= '9');
        bool validLetter = ch == 'P' || ch == 'A' || ch == 'M' || ch == 'p' || ch == 'a' || ch == 'm' || ch == ':' || ch == '.' || ch == ' ';

        if (validNumber || validLetter)
        {
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
        return (char)0;
    }
}

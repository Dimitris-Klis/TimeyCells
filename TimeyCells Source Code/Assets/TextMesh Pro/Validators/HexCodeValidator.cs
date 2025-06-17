using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "InputValidator - HexCodeValidator.asset", menuName = "TextMeshPro/Input Validators/HexCode", order = 100)]
public class HexCodeValidator : ValidatorBase
{
    // Custom text input validation function
    public override char Validate(ref string text, ref int pos, char ch)
    {
        // Unfortunately, when manually validating, we also need to check for the character limit manually :/
        if (InputField != null) if (InputField.characterLimit > 0 && text.Length >= InputField.characterLimit) return (char)0;

        bool validLetter = (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
        bool canHashtag = ch == '#' && pos == 0 && (text.Length == 0 || text[0] != '#' );

        if (validLetter || canHashtag)
        {
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
        return (char)0;
    }
}
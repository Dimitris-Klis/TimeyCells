using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class ValidatorBase : TMP_InputValidator
{
    protected TMP_InputField InputField;

    public void AssignInputField(TMP_InputField _inputField)
    {
        InputField = _inputField;
    }

    public override char Validate(ref string text, ref int pos, char ch)
    {
        return (char)0;
    }
}

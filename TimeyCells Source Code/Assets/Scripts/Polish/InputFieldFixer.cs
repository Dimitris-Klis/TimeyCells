using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class InputFieldFixer : MonoBehaviour
{
    public TMP_InputField TMPField;
    [SerializeField] TMP_Text PreviewText;

    // We're removing Zero Width Spaces to fix the Roboto font displaying them as a little dot in the corner.
    public void UpdateTextPreview(string text)
    {
        // Removing the 0-width spaces
        string sanitized = text.Replace("\u200b", "");
        PreviewText.text = sanitized;
    }

    private void Start()
    {
        // We do this so that the custom validators can access the Character Limit.
        if (TMPField.inputValidator != null)
        {
            ValidatorBase b = TMPField.inputValidator as ValidatorBase;
            b.AssignInputField(TMPField);
        }
    }
}
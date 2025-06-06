using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class TMPDropdownLocalizer : MonoBehaviour
{
    public TMP_Dropdown TMP_Dropdown;
    public string[] keys;

    public void UpdateText()
    {
        if (TMP_Dropdown == null) return;
        for (int i = 0; i < keys.Length; i++)
        {
            TMP_Dropdown.options[i].text = LocalizationSystem.instance.GetText(gameObject.name, keys[i]);
        }
        TMP_Dropdown.captionText.text = TMP_Dropdown.options[TMP_Dropdown.value].text;
    }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (TMP_Dropdown == null)
                TMP_Dropdown = GetComponent<TMP_Dropdown>();
        }
    #endif
}
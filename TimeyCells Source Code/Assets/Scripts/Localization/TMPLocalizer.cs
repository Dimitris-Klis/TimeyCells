using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPLocalizer : MonoBehaviour
{
    public TMP_Text TMP_Text;
    [Space]
    public string key;
    public string extraText;



    public void UpdateText()
    {
        if (TMP_Text == null) return;
        TMP_Text.text = LocalizationSystem.instance.GetText(gameObject.name, key) + extraText;
    }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (TMP_Text == null)
                TMP_Text = GetComponent<TMP_Text>();
        }
    #endif
}

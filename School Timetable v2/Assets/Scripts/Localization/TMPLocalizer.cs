using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPLocalizer : MonoBehaviour
{
    public TMP_Text TMP_Text;
    public string key;
    public string extraText;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.01f);
        UpdateText();
    }

    public void UpdateText()
    {
        if (TMP_Text == null) return;
        TMP_Text.text = LocalizationSystem.instance.GetText(gameObject.name, key) + extraText;
    }
    public void UpdateText(string newkey)
    {
        key=newkey;

        UpdateText();
    }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (TMP_Text == null)
                TMP_Text = GetComponent<TMP_Text>();
        }
    #endif
}

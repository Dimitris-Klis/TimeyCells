using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class VersionText : MonoBehaviour
{
    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = "v" + Application.version;
    }
}
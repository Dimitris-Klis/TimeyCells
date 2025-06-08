using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(!Application.isMobilePlatform);
    }
}
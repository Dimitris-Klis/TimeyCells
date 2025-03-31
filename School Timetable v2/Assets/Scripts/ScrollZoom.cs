using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
    public Transform Table;
    public int MinScale=1, MaxScale=3;
    public float ScrollSensitivity = .25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Table.localScale += Vector3.one * ScrollSensitivity;
            if(Table.localScale.x > MaxScale)
                Table.localScale = Vector3.one * MaxScale;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            Table.localScale -= Vector3.one * ScrollSensitivity;
            if (Table.localScale.x < MinScale)
                Table.localScale = Vector3.one * MinScale;
        }
    }
}

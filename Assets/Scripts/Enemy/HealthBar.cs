using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private Transform bar;
    // Use this for initialization
    void Start()
    {
        bar = transform.Find("Bar");
    }

    // Update is called once per frame
    public void SetSize(float normalizedSize)
    {
        // Size between 0 and 1
        if (bar == null)
        {
            return;
        }
        if (normalizedSize < 0)
        {
            normalizedSize = 0;
        }
        bar.localScale = new Vector3(normalizedSize, 1f);
    }
}

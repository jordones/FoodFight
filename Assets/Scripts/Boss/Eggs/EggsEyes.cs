using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsEyes : MonoBehaviour
{

    public GameObject character = null;
    public bool active;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Character")
        {
            character = col.gameObject;
            active = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Character")
        {
            character = null;
            active = false;
        }

    }
}

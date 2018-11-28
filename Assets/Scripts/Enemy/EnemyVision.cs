using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {
    public bool active = false;
    public GameObject character = null;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Character")
        {
            active = true;
            character = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Character")
        {
            active = false;
        }
    }
}

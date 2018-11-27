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
            Debug.Log("Enemy can see character");
            active = true;
            character = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Character")
        {
            Debug.Log("Enemy can no longer see character");

            active = false;
        }
    }
}

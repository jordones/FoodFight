﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeEyes : MonoBehaviour {
	public bool active = false;
	public GameObject character = null;
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Character") {
			Debug.Log("Knife can see character");
			active = true;
			character = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Character") {
			Debug.Log("Knife can no longer see character");

			active = false;
        }
	}
}

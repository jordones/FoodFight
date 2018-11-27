using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineappleEyes : MonoBehaviour {

	// Use this for initialization
	public GameObject character = null;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnTriggerEnter2D(Collider2D col) {
		if(col.tag == "Character") {
			character = col.gameObject;
		}
	}

	public void OnTriggerExit2D(Collider2D col) {
		if(col.tag == "Character") {
			character = col.gameObject;
		}

	}
}

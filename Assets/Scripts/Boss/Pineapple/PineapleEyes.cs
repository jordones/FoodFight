using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineapleEyes : MonoBehaviour {

	// Use this for initialization
	public GameObject character = null;
	public bool active = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnTriggerEnter2D(Collider col) {
		if(col.tag == "Character") {
			this.character = col.gameObject;
			this.active = true;
		}
	}

	public void OnTriggerExit2D(Collider col) {
		if(col.tag == "Character") {
			this.character = col.gameObject;
			this.active = false;
		}

	}
}

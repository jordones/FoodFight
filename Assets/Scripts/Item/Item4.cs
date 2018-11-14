using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item4 : Item {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnPickup(Character character) {
		Vector3 newSize = character.slapFab.transform.localScale;
		newSize.x += 1f;
		character.slapFab.transform.localScale = newSize;
	}
}

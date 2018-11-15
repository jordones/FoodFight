using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item5 : Item {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnPickup(Character character) {
		character.spewSpeed += 10f;
	}
}

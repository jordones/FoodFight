using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1 : Item {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnPickup(Character character) {
        character.maxSpeed += 3f;
	}

}
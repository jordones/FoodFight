using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2 : Item {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public override void ModifyCharacter() {
		character.slapDamage *= 2;
		character.spewDamage *= 2;
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3 : Item {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public override void ModifyCharacter() {
		character.health -= 27;
	}
}
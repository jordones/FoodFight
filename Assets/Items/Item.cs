using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

	public Character character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Collision on the item");
		if (col.tag == "Character") {
			// Item effect gets added here for now it will just be removed
			character = col.gameObject.GetComponent<Character>();
			ModifyCharacter();
			Destroy(gameObject);
			
		}
	}

	public abstract void ModifyCharacter();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Collision on the item");
		if (col.tag == "Character") {
			GameObject parent = transform.parent.gameObject;

			Character character = col.gameObject.GetComponent<Character>();
			character.Pickup(gameObject);
			GetComponent<Collider2D>().enabled = false;
			Destroy(parent);
		}
	}

	public abstract void OnPickup(Character character);
}

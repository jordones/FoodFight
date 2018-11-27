using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsAttack : MonoBehaviour {

	public float duration = 1.5f;

	public int damageOnHit = 30;
	// Use this for initialization
	void Start () {
		Destroy(gameObject, duration);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Character") {
			Character character = col.GetComponent<Character>() as Character;
			character.TakeDamage(damageOnHit);
			Destroy(gameObject);
		} else if (col.tag == "Terrain") {
		    Destroy(gameObject);
		}
	}
	
}

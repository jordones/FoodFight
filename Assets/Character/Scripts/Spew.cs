using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spew : MonoBehaviour {

    public int damage = 25;


	// Use this for initialization
	void Start() {
		// Destory this thing after n seconds if it does not collide with anything
		Destroy(gameObject, 1.5f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log("SPEW HIT");

		// If it hit an ememy
		if(col.tag == "Enemy") {
			col.gameObject.GetComponent<EnemyStats>().TakeDamage(damage);
			Debug.Log("Enemy Hit");
		    Destroy(gameObject);
		} else if (col.tag != "Character") {
		    Destroy(gameObject);
		}

	} 
}

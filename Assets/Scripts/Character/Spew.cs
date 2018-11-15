using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spew : MonoBehaviour {

	public static Character playerScript;


	// Use this for initialization
	void Start() {
		// Destory this thing after n seconds if it does not collide with anything
		Destroy(gameObject, 1.5f);

	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log("SPEW HIT");
		Debug.Log(col.gameObject);

		// If it hit an ememy
		if(col.tag == "Enemy") {
			col.gameObject.GetComponent<EnemyStats>().TakeDamage(playerScript.spewDamage);
			Debug.Log("Enemy Hit");
		    Destroy(gameObject);
		} else if (col.tag != "Character" && col.tag != "Spawner") {
		    Destroy(gameObject);
		}

	} 
}

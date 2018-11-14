using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// todo: Start Animation here

		Destroy(gameObject, 0.5f);
	}

	void OnTriggeredEnter2D(Collider2D col) {
        Debug.Log("Slap Hit!");
        Debug.Log(col.tag);
		if (col.tag == "Enemy") {
			// col.gameObject.GetComponent<EnemyStats>.TakeDamage();
			Debug.Log("Slapped an Enemy!");
		}
	}
}

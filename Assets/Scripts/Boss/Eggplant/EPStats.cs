using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPStats : TypedEnemy {

    public int health = 250;
    public int attack = 45;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (health <= 0) {
            Die();
        }
    }

    public void TakeDamage (int amount) {
        health -= amount;
        Debug.Log("Enemy damage from " + (health+amount) + " to " + health);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;

        if (collider.tag == "Character") {
            collider.gameObject.GetComponent<Character>().TakeDamage(attack);
        }
    }

    private void Die () {
        Debug.Log("Enemy Death");
        Destroy(gameObject);    
        LevelManager.instance.Killed(this);
        // print("Enemy destroyed");
    }
}

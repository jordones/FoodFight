using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public int health = 100;
    public int attack = 25;
    public GameObject me;

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
    }

    void OnCollisionEnter2D(Collision2D collision) {

        Collider2D collider = collision.collider;

        if (collider.tag == "Player") {
            collider.gameObject.GetComponent<Player>().TakeDamage(attack);
        }
    }

    private void Die () {
        Destroy(me);
    }
}

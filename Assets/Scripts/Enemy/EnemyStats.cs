using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : TypedEnemy {

    public int health = 100;
    public int attack = 25;

	// Use this for initialization
	void Start () {
		
	}
	

    public void TakeDamage (int amount) {
        health -= amount;
        if (health <= 0) {
            Die();
        }
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
        LevelManager.instance.Killed(this);
        Destroy(gameObject);    
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		GUI.Label(new Rect (pos.x, Screen.height-pos.y-70, 30, 30), "" + health);
    }
}

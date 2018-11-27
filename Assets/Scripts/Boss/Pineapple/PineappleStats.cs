using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineappleStats : TypedEnemy {

	public int attack = 10;
	public int health = 100;
	
	// Use this for initialization
	void Start () {	
	}
	
    public void TakeDamage (int amount) {
        health -= amount;
		if (health  <= 0) {
			Die();
		}
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        if (collider.tag == "Character") {
            collider.gameObject.GetComponent<Character>().TakeDamage(attack);
        }
    }

    private void Die () {
        if (LevelManager.instance) {
			LevelManager.instance.Killed(this);
		}
        Destroy(gameObject);    
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		GUI.Label(new Rect (pos.x, Screen.height - pos.y - 70, 30, 30), "" + health);
    }

}

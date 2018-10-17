using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int attack = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int amount) {
        Debug.Log("Took damage of amount: " + amount);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Collider2D collider = collision.collider;

        if (collider.tag == "Enemy")
        {
            collider.gameObject.GetComponent<EnemyStats>().TakeDamage(attack);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public int health = 100;
    public int attack = 25;
    public GameObject me;

    public GameObject enemyFab;

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
        SpawnEnemy(new Vector3(1,1,0), SimpleMovement.BEHAVIOUR.AGGRO); 
        Destroy(me);    
    }

    private void SpawnEnemy(Vector3 spawnPos, SimpleMovement.BEHAVIOUR behaviour) {
        GameObject newEnemy = Instantiate(enemyFab, spawnPos, Quaternion.Euler(new Vector3 (0,0,0))) as GameObject;
        newEnemy.GetComponent<SimpleMovement>().behaviour = SimpleMovement.BEHAVIOUR.AGGRO;
        newEnemy.GetComponent<EnemyStats>().health = 100;
    }
}

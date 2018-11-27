using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public int health = 100;
    public int attack = 25;
    private bool alive = true;
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

    }
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
            animator.SetTrigger("Attack");
            collider.gameObject.GetComponent<Character>().TakeDamage(attack);
        }

    }

    private void Die () {
        if (alive)
        {
            animator.SetTrigger("Die");
            Debug.Log("Enemy Death");
            LevelManager.instance.Killed();
            Destroy(gameObject, 0.417f);
        }
        alive = false;
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		GUI.Label(new Rect (pos.x, Screen.height-pos.y-70, 30, 30), "" + health);
    }
}

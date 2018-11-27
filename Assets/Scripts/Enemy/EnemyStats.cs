using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : TypedEnemy
{

    public int maxHealth = 100;
    public int health = 100;
    public int attack = 25;
    public EnemyAnimationHelper animationHelper;

    [SerializeField] private HealthBar healthBar;


    // Use this for initialization
    void Start()
    {
        healthBar.SetSize(1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            animationHelper.TriggerDie();
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        float healthPercent = (float)health / maxHealth;
        healthBar.SetSize(healthPercent);
        Debug.Log("Enemy damage from " + (health + amount) + " to " + health);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Collider2D collider = collision.collider;

        if (collider.tag == "Character")
        {
            Debug.Log(animationHelper);
            Debug.Log("Attacking");
            animationHelper.TriggerAttack();
            collider.gameObject.GetComponent<Character>().TakeDamage(attack);
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Death");
        Destroy(gameObject, 0.5f);
        LevelManager.instance.Killed(this);
        // print("Enemy destroyed");
    }

    void OnGUI()
    {
        // Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        // GUI.Label(new Rect (pos.x, Screen.height-pos.y-70, 30, 30), "" + health);
    }
}

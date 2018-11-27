using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsBehaviour : MonoBehaviour
{
    public EggsEyes eyes;

    private int attackType;
    private float minAttackTime = 0.5f;
    private float maxAttackTime = 3f;

    private float minJumpTime = 2f;
    private float maxJumpTime = 5f;
    private float singleAttackSpeed = 10f;
    private float doubleAttackSpeed = 7f;
    private float tripleAttackSpeed = 7f;
    public Rigidbody2D projectileFab;
    public float jumpForce = 500f;
    public Rigidbody2D rb2d;
    // I want a single fast attack
    // I want a double slower attack
    // I want a triple slower attack
    // Maybe burst different types of attacks in a window
    // jumps randomly

    // Use this for initialization
    void Awake()
    {
        Attack();
        Jump();
    }

    void Attack()
    {
        float randomTime = Random.Range(minAttackTime, maxAttackTime);
        Invoke("AttackWindow", randomTime);
    }

    void AttackWindow()
    {
        attackType = Random.Range(1, 4);
        switch (attackType)
        {
            case 1:
                SingleFastAttack();
                break;
            case 2:
                DoubleSlowerAttack();
                break;
            case 3:
                TripleSlowerAttack();
                break;
        }
        Attack();
    }

    /*
        Single, Double, Triple are number of projectiles
        Fast, Slower, are the speed of the projectiles
     */
    void SingleFastAttack()
    {
        SingleAttack(singleAttackSpeed, 0);
    }

    void DoubleSlowerAttack()
    {
        SingleAttack(doubleAttackSpeed, 2);
        SingleAttack(doubleAttackSpeed, -2);
    }

    void TripleSlowerAttack()
    {
        SingleAttack(tripleAttackSpeed, 5);
        SingleAttack(tripleAttackSpeed, 0);
        SingleAttack(tripleAttackSpeed, -5);

    }

    void SingleAttack(float speed, float degOffset)
    {
        Vector3 targetPosition = eyes.character.transform.position;
        targetPosition.y += degOffset;
        Rigidbody2D spew = Instantiate(projectileFab, transform.position, transform.rotation) as Rigidbody2D;
        spew.velocity = (targetPosition - transform.position).normalized * speed;
    }

    bool CanSeeCharacter()
    {
        return eyes != null && eyes.active;
    }

    void Jump()
    {
        print("JUMPING ");
        rb2d.AddForce(new Vector2(0f, jumpForce));
        
        float randomTime = Random.Range(minJumpTime, maxJumpTime);
        Invoke("Jump", randomTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {

    public float movementSpeed = 5.0f;
    public int fieldOfVision = 10;
    public BEHAVIOUR behaviour;
    private MOVE_DIRECTION moveDirection = MOVE_DIRECTION.NONE;
    public Animator animator;

    private bool facingRight = true;

    // Use this for initialization
    void Awake () {
        animator = GetComponent<Animator>();

    }

    void Update () {

        Vector2 fieldLeft = transform.position;
        fieldLeft.x -= fieldOfVision;

        Vector2 fieldRight = transform.position;
        fieldRight.x += fieldOfVision;


        bool visionRight  = Physics2D.Linecast(transform.position, fieldRight, 1 << LayerMask.NameToLayer("Player"));
        bool visionLeft = Physics2D.Linecast(transform.position, fieldLeft, 1 << LayerMask.NameToLayer("Player"));

        if (visionRight) {
            if (behaviour == BEHAVIOUR.AGGRO) {

                moveDirection = MOVE_DIRECTION.RIGHT;
            } else {

                moveDirection = MOVE_DIRECTION.LEFT;
            }
        }
        else if (visionLeft) {

            if (behaviour == BEHAVIOUR.AGGRO) {

                moveDirection = MOVE_DIRECTION.LEFT;
            } else {

                moveDirection = MOVE_DIRECTION.RIGHT;
            }
        }
        else {
            moveDirection = MOVE_DIRECTION.NONE;
        }
    }


    // Update is called once per frame
    void FixedUpdate () {
		
        switch (moveDirection) {
            case MOVE_DIRECTION.NONE:
                animator.ResetTrigger("Walk");
                break;
            
            case MOVE_DIRECTION.LEFT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * movementSpeed * -1, GetComponent<Rigidbody2D>().velocity.y);
                animator.SetTrigger("Walk");
                if (facingRight) Flip();
                break;
            
            case MOVE_DIRECTION.RIGHT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
                animator.SetTrigger("Walk");
                if (!facingRight) Flip();
                break;
            

        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = !spriteRenderer.flipX;

    }

    // Enumeration denoting the direction in which the enemy should be fleeing
    private enum MOVE_DIRECTION
    {
        LEFT,
        RIGHT,
        NONE
    }

    public enum BEHAVIOUR {
        AGGRO,
        FLEE
    }
}

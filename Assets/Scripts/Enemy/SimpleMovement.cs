﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {

    public float movementSpeed = 2.8f;
    public float moveForce = 50f;
    public BEHAVIOUR behaviour;
    private MOVE_DIRECTION moveDirection = MOVE_DIRECTION.NONE;

    public EnemyVision enemyVision;
    public EnemyAnimationHelper animationHelper;
    private SpriteRenderer spriteRenderer;
    public bool facingRight = true;

    public float stunTime = 2f;
    private BEHAVIOUR initialBehaviour;

    void Awake() {
        initialBehaviour = behaviour;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
      
        bool visionRight = false;
        bool visionLeft = false;

        if (enemyVision.active) {
            if (enemyVision.character.transform.position.x > gameObject.transform.position.x) {
                // player is to the right
                visionRight = true;
            }
            else if (enemyVision.character.transform.position.x < gameObject.transform.position.x) {
                // player is to the left
                visionLeft = true;
            }
            else {
                // player is in same x as enemy
                visionRight = false;
                visionLeft = false;
            }
        }
        else {
            // player is in same x as enemy
            visionRight = false;
            visionLeft = false;
        }

        switch(behaviour) {
            case BEHAVIOUR.AGGRO:
                if(visionRight) {
                    moveDirection = MOVE_DIRECTION.RIGHT;
                } else if (visionLeft) {
                    moveDirection = MOVE_DIRECTION.LEFT;
                } else {
                    moveDirection = MOVE_DIRECTION.NONE;
                }
            break;
            case BEHAVIOUR.FLEE:
                if(visionRight) {
                    moveDirection = MOVE_DIRECTION.LEFT;
                } else if (visionLeft) {
                    moveDirection = MOVE_DIRECTION.RIGHT;
                } else {
                    moveDirection = MOVE_DIRECTION.NONE;
                }
            break;
            default:
            case BEHAVIOUR.NEUTRAL:
                moveDirection = MOVE_DIRECTION.NONE;
            break;
        }
    }


    // Update is called once per frame
    void FixedUpdate () {
		int dir = 0;
        switch (moveDirection) {
            case MOVE_DIRECTION.NONE:
                animationHelper.ToggleAnimation(ANIMATION_STATE.IDLE);
                break;
            
            case MOVE_DIRECTION.LEFT:
                animationHelper.ToggleAnimation(ANIMATION_STATE.WALK);
                dir = -1;
                if (facingRight) {
                    Flip();
                }
                break;
            
            case MOVE_DIRECTION.RIGHT:
                animationHelper.ToggleAnimation(ANIMATION_STATE.WALK);
                if (!facingRight) {
                    Flip();
                }
                dir = 1;
                break;
        }

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (dir * rb2d.velocity.x < movementSpeed) {
            rb2d.AddForce(Vector2.right * dir * moveForce);
        }

        // if character speed is above max character speed, set speed to max speed
        if (Mathf.Abs(rb2d.velocity.x) > movementSpeed) {
            // Mathf.Sign gets sign of number (1 or -1)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * movementSpeed, rb2d.velocity.y);
        }   	
	}

    public void Stun() {
        behaviour = BEHAVIOUR.NEUTRAL;
        Invoke("RecoverFromStun", stunTime);
    }
    private void RecoverFromStun() {
        behaviour = initialBehaviour;
    }

    void Flip()
    {
        facingRight = !facingRight;
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
        FLEE,
        NEUTRAL
    }
}

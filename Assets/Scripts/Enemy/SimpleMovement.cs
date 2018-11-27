using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {

    public float movementSpeed = 2.8f;
    public float moveForce = 50f;
    public int fieldOfVision = 10;
    public BEHAVIOUR behaviour;
    private MOVE_DIRECTION moveDirection = MOVE_DIRECTION.NONE;
    public EnemyAnimationHelper animationHelper;


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
		int dir = 0;
        switch (moveDirection) {
            case MOVE_DIRECTION.NONE:
                Debug.Log("Idling");
                animationHelper.ToggleAnimation(ANIMATION_STATE.IDLE);
                break;
            
            case MOVE_DIRECTION.LEFT:
                Debug.Log("Walk Left");
                animationHelper.ToggleAnimation(ANIMATION_STATE.WALK);
                dir = -1;
                break;
            
            case MOVE_DIRECTION.RIGHT:
                Debug.Log("Walk Right");
                animationHelper.ToggleAnimation(ANIMATION_STATE.WALK);
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

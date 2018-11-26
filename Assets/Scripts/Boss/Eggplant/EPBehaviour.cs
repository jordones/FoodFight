using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPBehaviour : MonoBehaviour {

    public float movementSpeed = 2.8f;
    public float moveForce = 50f;
    private MOVE_DIRECTION moveDirection = MOVE_DIRECTION.NONE;
	private bool active = false;
	private GameObject character = null;

	// Use this for initialization
	void Awake () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Character") {
			active = true;
			character = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Character") {
			active = false;
			moveDirection = MOVE_DIRECTION.NONE;
        }
    }

    void Update () {

    }


    // Update is called once per frame
    void FixedUpdate () {
		if (active) {
			int dir = 0;
			if (character.transform.position.x < gameObject.transform.position.x) {
				moveDirection = MOVE_DIRECTION.LEFT;
			} else {
				moveDirection = MOVE_DIRECTION.RIGHT;
			}
			switch (moveDirection) {
				case MOVE_DIRECTION.NONE:
						break;
				
				case MOVE_DIRECTION.LEFT:
					dir = -1;
					break;
				
				case MOVE_DIRECTION.RIGHT:
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
	}

    // Enumeration denoting the direction in which the enemy should be fleeing
    private enum MOVE_DIRECTION
    {
        LEFT,
        RIGHT,
        NONE
    }
}

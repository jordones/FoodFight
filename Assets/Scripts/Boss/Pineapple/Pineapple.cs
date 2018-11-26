﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : TypedEnemy {
    private static float HIDING_DELTA_Y = 10f;

    public bool debugState;

    // The time the boss waits before chossing to appear after it has chosen where it will appear
	public float hiddenTime = 3f;

    // The time the boss waits before it chooses to eith hide or spawn enemies
	public float appearedTime = 3f;
	
    // The rate at which the boss spawns enemies
	public float enemySpawnRate = 1f;
	public float appearingTime = 0.05f;
	public float hidingTime = 0.05f;
	public int attack = 10;
	public int health = 100;

    // Should be the character
	public GameObject character;
	
	// The enemy that the boss will spawn 
	public GameObject spawnFab;

    // The distance from either side of the character that the boss will 
	// come up from under the ground and spawn enemies
	public float appearRange = 10f;

    // Max number of enemies to spawn in a spawn cycle
	public int maxEnemiesToSpawn = 2;


    // The range to the left and to the right of the initial spawn location that the boss will interact with the character
	public float battleRange = 10f;
	// The bossess X coordinate on spawn 
	private float initialX;

	private PineappleState currentState;
	// wheather or not an enemy will spawn next time the boss appears
	private bool spawnEnemy = false;
	// Whether or not the character is in the bossess battleRange
	private bool targetInRange;
	
	private bool facingRight = false;
	private Rigidbody2D rb2d;
	private BoxCollider2D bc2d;
	private bool isMoving = false;
	private float groundY = 0;



	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		bc2d = GetComponent<BoxCollider2D>();
		initialX = gameObject.transform.position.x;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 50, 1  << LayerMask.NameToLayer("Ground"));
		if(hit) {
			groundY = transform.position.y - hit.distance;
		} else {
			groundY = transform.position.y - HIDING_DELTA_Y;
		}
		
		gameObject.transform.position = new Vector2(initialX, groundY - HIDING_DELTA_Y);
		StartCoroutine("BattlePlayer", 0.0f);		
	}

	void FixedUpdate() {
		bool targetToRight = gameObject.transform.position.x < character.gameObject.transform.position.x;
		if (targetToRight && !facingRight){
			Flip();
		} else if (!targetToRight && facingRight) {
			Flip();
		}
	}

	void Flip() {
		facingRight = !facingRight;
	}

	protected IEnumerator SmoothMovement(Vector3 end, float moveTime){
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		float inverseMoveTime = 1/moveTime;

        IgnoreGroundCollisions();
		isMoving = true;
		while (sqrRemainingDistance > float.Epsilon) {
			// Move a point to a new point in a stright line
			Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
			rb2d.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
		isMoving = false;
		AllowGroundCollisions();
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

	private IEnumerator BattlePlayer() {

		float appearX = initialX;
		
        while (true) {
			targetInRange = initialX + battleRange >= character.gameObject.transform.position.x
		                && initialX - battleRange <= character.gameObject.transform.position.x;
					
			if (!targetInRange) {
				if(currentState != PineappleState.HIDDEN || currentState != PineappleState.HIDING) {
				    ChangeState(PineappleState.HIDING);
				}
			}
			
			switch(currentState) {
				case PineappleState.HIDDEN:
					// Choose Behaviour: Spawn an enemy or dont
					spawnEnemy = 1 == Random.Range(0,2) ? true : false;
					
					// If the boss chose to spawn an enemy when it appears
					if (spawnEnemy) {
						appearX = SpawnEnemies_AppearX();
					} else {
						appearX = character.transform.position.x;
					}
		
					// show indicator to where boss will come up?, then wait
					yield return new WaitForSeconds(hiddenTime);
					ChangeState(PineappleState.APPEARING);

				break;
				case PineappleState.APPEARING:
					// Instantly move the bosses x position
					// This is so when smooth movment happens, the boss come up from the ground directly under the
					// the chosen position
					transform.position = (new Vector3(appearX, transform.position.y, 0));
					yield return null;
					// Tell the boss to move upwards, then wait
					StartCoroutine(SmoothMovement(new Vector2(appearX, groundY + bc2d.bounds.extents.y), appearingTime));
					yield return new WaitForSeconds(appearingTime);
					ChangeState(PineappleState.APPEARED);
				break;
				case PineappleState.APPEARED:
					if (spawnEnemy) {
						ChangeState(PineappleState.SPAWNING_ENEMY);
					 } else {
                        ChangeState(PineappleState.HIDING);
					 } 
					spawnEnemy = false;
					yield return new WaitForSeconds(appearedTime);
					
				break;
				case PineappleState.SPAWNING_ENEMY:
					int numSpawn = Random.Range(0, maxEnemiesToSpawn+1);
					for (int i = 0; i < numSpawn; i++) {
						// X position for spawning is either on to the left or the right of the boss
						// and just outside the boss's collider
						float spawnX = facingRight ? transform.position.x + 0.1f + bc2d.bounds.extents.x
													: transform.position.x - 0.1f - bc2d.bounds.extents.x;
						Vector3 spawnPos = new Vector3(spawnX, transform.position.y, 0);

						// Instansitate an enemy
					   Instantiate(spawnFab, spawnPos, Quaternion.Euler(new Vector3 (0, 0, 0)));
					   yield return new WaitForSeconds(enemySpawnRate);
					}
					ChangeState(PineappleState.HIDING);
				break;

				case PineappleState.HIDING:
					// Tell the boss to hide, reposition the boss to it's
					StartCoroutine(SmoothMovement(new Vector2(transform.position.x, groundY-HIDING_DELTA_Y), hidingTime));
					yield return new WaitForSeconds(hidingTime);
					ChangeState(PineappleState.HIDDEN);

				break;
			}
		yield return null;
		}

	}
	private void ChangeState(PineappleState state) {
		if (debugState) {
			Debug.Log("Pineapple State: Changing from " + getStateString(currentState) + " to " + getStateString(state));
		}
		currentState = state;

	}
	// Get a position either to the left or to the right of the player
	// that is appearRange units away. The boss will only appear using this pattern if it
	// will be spawning enemies after it appears
    private float SpawnEnemies_AppearX() {
		int sign = 1 == Random.Range(0, 2) ? -1 : 1;
		float targetX = character.transform.position.x;
		float targetY = character.transform.position.y;
		float xPos = targetX + appearRange*sign;


        // Raycast from the character to the chosen position. If there is a wall in the way,
		// change the chosen position to be on the side of the wall that is in the level bounds.
		Vector2 sourceCast = new Vector2(targetX, targetY);
		Vector2 destCast = new Vector2(xPos, targetY);
		RaycastHit2D hit = Physics2D.Raycast(sourceCast, destCast, (1 << LayerMask.NameToLayer("Ground")) | 1 << LayerMask.NameToLayer("Wall"));

		if (hit) {
			xPos = hit.transform.position.x - hit.collider.bounds.extents.x - bc2d.bounds.extents.x;
		}
		return xPos;
	}
	private void IgnoreGroundCollisions() {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Ground"), true);
	}
	private void AllowGroundCollisions() {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Ground"),false);
	}


	private string getStateString(PineappleState state) {
		string name = "UNKNOWN";
			switch(state) {
				case PineappleState.HIDDEN:
				    name = "Hidden";
				break;
				case PineappleState.APPEARING:
				    name = "Appearing";
				break;
				case PineappleState.APPEARED:
				    name = "Appeared";
				break;
				case PineappleState.SPAWNING_ENEMY:
				    name = "Spawning_Enemy";
				break;
				case PineappleState.HIDING:
				    name = "Hiding";
				break;
			}
			return name;
	}
}

enum PineappleState {
	HIDDEN,
	APPEARED,
	HIDING,
	APPEARING,
	SPAWNING_ENEMY
}

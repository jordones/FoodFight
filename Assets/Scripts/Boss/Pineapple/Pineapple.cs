using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : TypedEnemy {

	public int damage = 10;
	public int health = 100;

	public GameObject mainTarget; 

    // The distance from either side of the mainTarget(character) that the boss will 
	// come up from under the ground and spawn enemies
	public float appearRange = 10f;

    // Max number of enemies to spawn in a spawn cycle
	public int maxEnemiesToSpawn = 2;


    // The range to the left and to the right of the initial spawn location that the boss will interact with the mainTarget(character)
	public float battleRange = 10f;
	// The bossess X coordinate on spawn 
	private float initialX;

	private PineappleState currentState;
	// wheather or not an enemy will spawn next time the boss appears
	private bool spawnEnemy = false;
	// Whether or not the mainTarget is in the bossess battleRange
	private bool targetInRange;

	public GameObject hiddenPlatformFab;
	private Vector3 hiddenPos;
	private int platformDeltaY = 100;
	private bool facingRight = false;


	// Use this for initialization
	void Start () {
		initialX = gameObject.transform.position.x;
		Vector3 platPos = new Vector3(initialX, gameObject.transform.position.y-platformDeltaY,0);
		GameObject hiddenPlatform = Instantiate(hiddenPlatformFab, platPos, Quaternion.Euler(new Vector3 (0,0,0))) as GameObject;
		hiddenPos = new Vector3 (initialX,hiddenPlatform.transform.position.y+1, 0);
		
		gameObject.transform.position = hiddenPos;
		StartCoroutine("BattlePlayer", 0.0f);
		IgnoreGroundCollisions();
		
	}
	
	// Update is called once per frame
	void Update () {

	}


	private IEnumerator BattlePlayer() {

		float appearX = initialX;
		
        while (true) {
			targetInRange = initialX + battleRange <= mainTarget.gameObject.transform.position.x
		                || initialX - battleRange >= mainTarget.gameObject.transform.position.x;

			if (!targetInRange) {
				currentState = PineappleState.HIDING;
			}
			

			switch(currentState) {
				case PineappleState.HIDDEN:
					// Choose Behaviour
					spawnEnemy = 1 == Random.Range(0,2) ? true : false;
					
					// If the boss chose to spawn an enemy when it appears
					if (spawnEnemy) {
						appearX = SpawnEnemies_AppearX();
					} else {
						appearX = mainTarget.transform.position.x;
					}
					currentState = PineappleState.APPEARING;
					// show indicator to where boss will come up?, then wait
					yield return new WaitForSeconds(1.5f);

				break;
				case PineappleState.APPEARING:
					// Tell the boss to move upwards, then wait
					IgnoreGroundCollisions();
					yield return new WaitForSeconds(1.5f);
					AllowGroundCollisions();
					currentState = PineappleState.APPEARED;
				break;
				case PineappleState.APPEARED:
					currentState = spawnEnemy ? PineappleState.SPAWNING_ENEMY : PineappleState.HIDING;
					spawnEnemy = false;
					yield return new WaitForSeconds(1.5f);
					
				break;
				case PineappleState.SPAWNING_ENEMY:
					// do stuff to spawn an emeny
					// change state
					int numSpawn = Random.Range(0,maxEnemiesToSpawn+1);
					for (int i = 0; i < numSpawn; i++) {

					    
					   yield return new WaitForSeconds(1.5f);
					}

				break;

				case PineappleState.HIDING:
					// Tell the boss to hide, reposition the boss to it's
					IgnoreGroundCollisions();
					yield return new WaitForSeconds(1.5f);
					AllowGroundCollisions();
					currentState = PineappleState.HIDDEN;

				break;
			}
		yield return null;
		}

	}

	// Get a position either to the left or to the right of the player
	// that is appearRange units away
    private float SpawnEnemies_AppearX() {
		int sign = 1 == Random.Range(0,1) ? -1 : 1;
		return mainTarget.transform.position.x + appearRange*sign;
	}
	private void IgnoreGroundCollisions() {
        Physics2D.IgnoreLayerCollision(12,10, true);
	}
	private void AllowGroundCollisions() {
        Physics2D.IgnoreLayerCollision(12,10,false);
	}
}

enum PineappleState {
	HIDDEN,
	APPEARED,
	HIDING,
	APPEARING,
	SPAWNING_ENEMY
}

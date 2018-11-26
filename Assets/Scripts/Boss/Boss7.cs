using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss7 : TypedEnemy {

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

	private Boss7State currentState;
	// wheather or not an enemy will spawn next time the boss appears
	private bool spawnEnemy = false;
	// Whether or not the mainTarget is in the bossess battleRange
	private bool targetInRange;




	// Use this for initialization
	void Start () {
		initialX = gameObject.transform.position.x;
		StartCoroutine("BattlePlayer", 0.0f);
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
				currentState = Boss7State.HIDING;
			}
			

			switch(currentState) {
				case Boss7State.HIDDEN:
					// Choose Behaviour
					spawnEnemy = 1 == Random.Range(0,1) ? true : false;
					
					// If the boss chose to spawn an enemy when it appears
					if (spawnEnemy) {
						appearX = SpawnEnemies_AppearX();
					} else {
						appearX = mainTarget.transform.position.x;
					}
					currentState = Boss7State.APPEARING;
					// show indicator to where boss will come up?, then wait
					yield return new WaitForSeconds(1.5f);

				break;
				case Boss7State.APPEARING:
					// Tell the boss to move upwards, then wait
					yield return new WaitForSeconds(1.5f);
					currentState = Boss7State.APPEARED;
				break;
				case Boss7State.APPEARED:
					currentState = spawnEnemy ? Boss7State.SPAWNING_ENEMY : Boss7State.HIDING;
					spawnEnemy = false;
					yield return new WaitForSeconds(1.5f);
					
				break;
				case Boss7State.SPAWNING_ENEMY:
					// do stuff to spawn an emeny
					// change state
					yield return new WaitForSeconds(1.5f);

				break;

				case Boss7State.HIDING:
					// Tell the boss to hide, reposition the boss to it's
					yield return new WaitForSeconds(1.5f);
					currentState = Boss7State.HIDDEN;

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
}

enum Boss7State {
	HIDDEN,
	APPEARED,
	HIDING,
	APPEARING,
	SPAWNING_ENEMY
}

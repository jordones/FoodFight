using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public bool active = true;
    public float spawnTime = 3f;
    public GameObject[] enemyTypes;    // Number of each different enemy type  
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {
            StartSpawner();
        } else {
            StopSpawner();
        }
    }

    void StartSpawner() {
        // Run the Spawn method every 3 seconds starting now
        if (!active) {
            InvokeRepeating("Spawn", 0.0f, spawnTime);
            active = true;
        }
    }

    void StopSpawner() {
        CancelInvoke();
        active = false;
    }

	void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Character entred spawner radius");
		if (col.tag == "Character") {
            StartSpawner();
        }
    }

    void OnTriggerExit2(Collider2D col) {
        Debug.Log("Character left spawner radius");
		if (col.tag == "Character") {
            StopSpawner();
        }
    }

    void Spawn()
    {
        GameObject enemy = getRandomEnemy();
        Debug.Log("Enemy spawned is: " + enemy);

        Instantiate(enemy, transform.position, transform.rotation);
    }

    GameObject getRandomEnemy()
    {
        int enemyTypeIndex = Random.Range(0, enemyTypes.Length);
        return enemyTypes[enemyTypeIndex];
    }
}

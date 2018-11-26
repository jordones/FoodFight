using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    private bool running = false;
    public bool active = false;
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

    public void StartSpawner() {
        // Run the Spawn method every 3 seconds starting now
        if (!running) {
            InvokeRepeating("Spawn", 0.0f, spawnTime);
            running = true;
        }
    }

    public void StopSpawner() {
        CancelInvoke();
        running = false;
    }

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Character") {
            Debug.Log("Character entred spawner radius");
            //StartSpawner();
            active = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Character") {
            Debug.Log("Character left spawner radius");
            //StopSpawner();
            active = false;
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

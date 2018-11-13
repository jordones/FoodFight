using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public float spawnTime = 3f;
    public GameObject[] enemyTypes;    // Number of each different enemy type  
    void Start()
    {
        // Run the Spawn method ever spawn time in 3 seconds every 3 seconds
        Spawn();
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public float spawnTime = 3f;
    private int numEnemyTypes = 2;    // Number of each different enemy type  
    void Start()
    {
        // Run the Spawn method ever spawn time in 3 seconds every 3 seconds
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Spawn()
    {
        GameObject enemy = getRandomEnemy();
        Instantiate(enemy, transform.position, transform.rotation);
    }

    GameObject getRandomEnemy()
    {
        int enemyType = Random.Range(0, numEnemyTypes);
        GameObject enemy = null;
        switch (enemyType)
        {
            case 0:
                enemy = Resources.Load("Enemy") as GameObject;
                break;
            case 1:
                enemy = Resources.Load("Enemy2") as GameObject;
                break;
        }
        return enemy;
    }
}

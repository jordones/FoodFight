using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public float spawnTime = 3f;
    public string[] enemyTypes;    // Number of each different enemy type  
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
        int enemyTypeIndex = Random.Range(0, enemyTypes.Length);
        string enemyType = enemyTypes[enemyTypeIndex];
        return Resources.Load(enemyType) as GameObject;
    }
}

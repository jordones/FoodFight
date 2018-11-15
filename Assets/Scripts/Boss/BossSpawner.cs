using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour,  OnLevelGoal {

    public GameObject[] bossTypes;    // Number of each different boss type  
    void Start()
    {
		LevelManager.instance.subscribeToGoal(this);
    }

    void Spawn()
    {
        GameObject boss = getRandomBoss();
        Debug.Log("Boss spawned is: " + boss);

        Instantiate(boss, transform.position, transform.rotation);
    }

    GameObject getRandomBoss()
    {
        int bossTypeIndex = Random.Range(0, bossTypes.Length);
        return bossTypes[bossTypeIndex];
    }

	public void OnLevelGoal() {
		Spawn();
	}
}

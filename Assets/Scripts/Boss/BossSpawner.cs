using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour,  OnLevelGoal {

    public GameObject[] bossTypes;    // Number of each different boss type  
    public GameObject boss;
    void Start()
    {
		LevelManager.instance.subscribeToGoal(this);
    }

    void Spawn()
    {
        GameObject bossFab = getRandomBoss();
        Debug.Log("Boss spawned is: " + boss);

        boss = Instantiate(bossFab, transform.position, transform.rotation);
    }

    GameObject getRandomBoss()
    {
        int bossTypeIndex = Random.Range(0, bossTypes.Length);
        return bossTypes[bossTypeIndex];
    }

	public void OnLevelGoal() {
		Spawn();
        Debug.Log(BossIndicator.instance);
        BossIndicator.instance.boss = boss;
	}
}

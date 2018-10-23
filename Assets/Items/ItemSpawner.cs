using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public ItemPool itemPool;
	// Use this for initialization
	void Awake () {
	}
	
    void Start() {
        Debug.Log("Item Spawn: Should only occur once");
        Spawn(GetSpawnableItem());
    }
	

    private GameObject GetSpawnableItem() {
        Debug.Log("Loading Item to Spawn");
        // This currently just returns a statically set resource
        // In future, it should ask itempool for a random object to load
        // e.g.

        return itemPool.GetRandomItem();
    }

    private void Spawn(GameObject item) {
        Debug.Log("Spawning Item to Screen");
        Instantiate(item, transform.position, transform.rotation);
    }
}

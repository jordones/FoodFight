using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	}
	
    void Start() {
        Debug.Log("Item Spawn: Should only occur once");
        StartCoroutine(Spawn(GetSpawnableItem()));
    }

    private GameObject GetSpawnableItem() {
        Debug.Log("Loading Item to Spawn");
        // This currently just returns a statically set resource
        // In future, it should ask itempool for a random object to load
        // e.g.

        return ItemPool.instance.GetRandomItem();
    }

    private IEnumerator Spawn(GameObject item) {
        yield return new WaitUntil(() => ItemPool.instance != null && ItemPool.instance.ready); //Wait until ItemPool is ready
        Debug.Log("Spawning Item to Screen");
        Instantiate(item, transform.position, transform.rotation);
    }
}

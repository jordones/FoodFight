using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	}
	
    void Start() {
        Debug.Log("Item Spawn: Should only occur once");
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        yield return new WaitUntil(() => ItemPool.instance != null && ItemPool.instance.ready); //Wait until ItemPool is ready
        GameObject item = ItemPool.instance.GetRandomItem();
        Debug.Log("Spawning Item to Screen");
        Instantiate(item, transform.position, transform.rotation);
    }
}

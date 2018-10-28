using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemPool: MonoBehaviour {
    public bool ready;
    public GameObject[] items;
    public List<int> pool = new List<int>();
    public static ItemPool instance = null;

    void Awake() {
        // Singleton instance; ensures that only one ItemPool can exist at a time
        if (instance == null) {
            instance = this;
            StartCoroutine(InitPool());

            ready = true;
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator InitPool()
    {
        Debug.Log("Waiting...");
        yield return new WaitUntil(() => UserManager.instance != null && UserManager.instance.ready); //Wait until UserManager is initialized
        Debug.Log("Moving on...");
        pool = UserManager.instance.items;
    }
    void Start() {
        pool.Add(0);
    }

    public GameObject GetRandomItem() {
        return items[pool[Random.Range(0, pool.Count)]];
    }
}

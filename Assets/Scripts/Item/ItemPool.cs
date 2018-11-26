using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

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
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitPool());
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator InitPool()
    {
        yield return new WaitUntil(() => UserManager.instance != null && UserManager.instance.ready); //Wait until UserManager is initialized
        pool = UserManager.instance.items;
        ready = true;

    }

    public GameObject GetRandomItem() {
        int selected = UnityEngine.Random.Range(0, pool.Count);
        return items[pool[selected]];
    }
}

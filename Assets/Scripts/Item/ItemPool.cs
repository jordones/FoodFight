using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public IEnumerator SyncUnlockedToUser()
    {
        yield return new WaitUntil(() => UserManager.instance != null && UserManager.instance.ready); //Wait until UserManager is initialized
        UserManager.instance.items = pool;
    }

    public GameObject GetRandomItem() {
        int selected = UnityEngine.Random.Range(0, pool.Count);
        return items[pool[selected]];
    }

    public List<int> GetRandomLockedIds(int n) {
        System.Random rnd = new System.Random();

        IEnumerable<int> r = Enumerable.Range(0, items.Length);
        List<int> potential = new List<int>();
        foreach (int i in r) {
            potential.Add(i);
        }
        foreach (int i in pool) {
            potential.Remove(i);
        }

        return potential.OrderBy(x => rnd.Next()).Take(n).ToList();
    }

    public List<GameObject> GetRandomLockedItems(int n) {
        return GetRandomLockedIds(n).Select(x => items[x]).ToList();
    }

    public void UnlockItem(int id) {
        if (id > 0 && id < items.Length && !pool.Contains(id)) {
            pool.Add(id);
        }
    }
}

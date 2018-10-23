using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool: MonoBehaviour {

    public string[] pool;
    public static ItemPool instance = null;


    void Awake() {
        // Singleton instance; ensures that only one ItemPool can exist at a time
        if (instance == null) {

            instance = new ItemPool();
        } else {
            instance = this;
        }

    }

    void Start() {
        // Probably grab items from the DB?
    }

    public GameObject GetRandomItem() {
        // Hard coded for now.. need to randomly return an item from the pool
        int myRand = Random.Range(1, 3);
        string randomItemName = "Item" + myRand.ToString();
        return Resources.Load(randomItemName) as GameObject;
    }
}

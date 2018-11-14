using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour {

	List<Item> items = new List<Item>();
	// Use this for initialization
	void Awake () {
	}
	
    void Start() {
		foreach (Transform itemStand in transform)
		{
        	StartCoroutine(Spawn(itemStand));
		}
    }

	void Update() {
		// print(items.Count);
		foreach (Item item in items)
		{
			if (item.pickedUp) {
				destoryAllItems();
			}
		}
	}

	private void destoryAllItems() {
		foreach (Item item in items)
		{
			Destroy(item.transform.parent);
		}
	}

    private IEnumerator Spawn(Transform itemStand) {
        yield return new WaitUntil(() => ItemPool.instance != null && ItemPool.instance.ready); //Wait until ItemPool is ready
        GameObject item = ItemPool.instance.GetRandomItem();
		print(item.GetComponentsInChildren<Item>()[0]);
		items.Add(item.GetComponentsInChildren<Item>()[0]) ;
		Vector3 newPosition = itemStand.position;
		newPosition.y += 0.7f;
        Instantiate(item, newPosition, itemStand.rotation);
    }
}

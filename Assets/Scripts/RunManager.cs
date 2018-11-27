using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunManager : MonoBehaviour {

	public static RunManager instance = null;

	// Use this for initialization
	void Start () {
		
	}
	
	void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
	}

	public void RunComplete() {
		List<int> unlocked = ItemPool.instance.GetRandomLockedIds(3);

		// Pass in new items to success screen
		RunCompleted.instance.UpdateUnlockedItems(
			unlocked.Select(x => ItemPool.instance.items[x].GetComponent<SpriteRenderer>().sprite).ToList()
		);

		// Give them to itempool
		foreach (int id in unlocked) {
			ItemPool.instance.UnlockItem(id);
		}
		// Give them to usermanager
		StartCoroutine(ItemPool.instance.SyncUnlockedToUser());

		// Cleanup
		CleanupPersistentObjects();
	}

	public void RunFailed() {
		// Cleanup
		CleanupPersistentObjects();
	}

	public void CleanupPersistentObjects() {
		if (Character.instance != null) {
			Destroy(Character.instance.gameObject);
		}
		if (UIManager.instance != null) {
			Destroy(UIManager.instance.gameObject);
		}
	}
}

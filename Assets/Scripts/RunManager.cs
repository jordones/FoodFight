using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour {

	public static RunManager instance = null;

	// Use this for initialization
	void Start () {
		
	}
	
	void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
	}

	public void RunComplete() {
		// Pick new items to unlock
		// Unlock them
		// Bring up RunCompleted scene
		// Pass in new items
		CleanupPersistentObjects();

	}

	public void RunFailed() {
		// Bring up RunFailed scene
		CleanupPersistentObjects();
	}

	public void CleanupPersistentObjects() {
		// Destroy character
		// Destroy UI
		// 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

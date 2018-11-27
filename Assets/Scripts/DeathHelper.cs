using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHelper : MonoBehaviour {

	public static DeathHelper instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
            RunManager.instance.RunFailed();
        } else {
            Destroy(gameObject);
        }
    }
}

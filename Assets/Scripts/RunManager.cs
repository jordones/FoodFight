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
	
	// Update is called once per frame
	void Update () {
		
	}
}

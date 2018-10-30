using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logout : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LogUserOut() {
		UserManager.instance.Logout();
	}
}

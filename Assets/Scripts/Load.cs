using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadUser() {
		Debug.Log("Attempting load...");
		StartCoroutine(UserManager.instance.LoadFirebaseAsync());
	}
}
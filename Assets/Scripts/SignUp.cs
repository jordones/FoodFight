﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class SignUp : MonoBehaviour {

	public InputField emailField;
	public InputField passwordField;

	public GameObject mainMenu;
	public GameObject loginMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SignUserUp() { 
		UserManager.instance.SignUp(emailField.text, passwordField.text);
		mainMenu.SetActive(true);
		loginMenu.SetActive(false);
	}
}
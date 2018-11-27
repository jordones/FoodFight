using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayHP : MonoBehaviour {

	// Use this for initialization
	private Character character;
	private Text textbox;
	void Awake () {
		// Init character controller to get hp
		character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
		// Init textbox to display the health
		textbox = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		int hp = character.getHealth();
		textbox.text = "HP: " + hp;
	}
}

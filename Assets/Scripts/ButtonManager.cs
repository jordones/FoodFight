using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class ButtonManager : MonoBehaviour {

	public Button save;
	public Button load;
	public Button exit;
	public Button login;
	public Button logout;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (UserManager.instance.loggedIn()) {
			save.gameObject.SetActive(true);
			load.gameObject.SetActive(true);
			logout.gameObject.SetActive(true);
			login.gameObject.SetActive(false);

			Vector3 pos = exit.transform.position;
			pos.y = -300f;
			exit.transform.position = pos;
		} else {
			save.gameObject.SetActive(false);
			load.gameObject.SetActive(false);
			logout.gameObject.SetActive(false);
			login.gameObject.SetActive(true);

			Vector3 pos = exit.transform.position;
			pos.y = -150f;
			exit.transform.position = pos;
		}
	}
}

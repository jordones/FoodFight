using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance = null;
	private int killed {get; set;} = 0;
	public int goal = 10;
	public string next = "MainMenu";

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
		if (killed == goal) {
			StartCoroutine(EndLevel());
		}
	}

	void OnGUI () {
		GUI.Label(new Rect (Screen.width - 150,0,100,50), "Kills: " + killed + "/" + goal);
	}

	public IEnumerator EndLevel() {
		yield return StartCoroutine(LoadScene.AsyncLoadScene(next));
	}

	public void Killed() {
		killed++;
	}
}

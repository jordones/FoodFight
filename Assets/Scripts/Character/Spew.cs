using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spew : MonoBehaviour {

	public static Character playerScript;

	public int maxCollisions = 1;
	private int collisionCount = 0;
	public AudioSource audioSource;
	public AudioClip [] spewHitClips;


	// Use this for initialization
	void Start() {
        playerScript = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();

		// Destory this thing after n seconds if it does not collide with anything
		Destroy(gameObject, 1.5f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (collisionCount >= maxCollisions) {
			return;
		}

		// If it hit an ememy
		if(col.tag == "Enemy") {
			col.gameObject.GetComponent<EnemyStats>().TakeDamage(playerScript.spewDamage);
			Debug.Log("Enemy Hit");

		    AudioClip clip = spewHitClips[Random.Range(0,spewHitClips.Length)];
			audioSource.PlayOneShot(clip);
			GetComponent<Renderer>().enabled = false;
			if (collisionCount <= maxCollisions) {
			    Destroy(gameObject, clip.length);
			}
		} else if (col.tag == "Terrain") {
			Debug.Log("Terrain Hit");
		    Destroy(gameObject);
		}

	} 
}

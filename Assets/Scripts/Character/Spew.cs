using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spew : MonoBehaviour {

	public static Character playerScript;

	public AudioSource audioSource;
	public AudioClip [] spewHitClips;

	private bool queuedToDestroy = false;


	// Use this for initialization
	void Start() {
        playerScript = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();

		// Destory this thing after n seconds if it does not collide with anything
		Destroy(gameObject, 1.5f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (queuedToDestroy) {
			return;
		}

		// If it hit an ememy
		if(col.tag == "Enemy") {
			col.gameObject.GetComponent<EnemyStats>().TakeDamage(playerScript.spewDamage);
			Debug.Log("Enemy Hit");

		    AudioClip clip = spewHitClips[Random.Range(0,spewHitClips.Length)];
			audioSource.PlayOneShot(clip);
			
			if (!queuedToDestroy) {
				queuedToDestroy = true;
			    GetComponent<Renderer>().enabled = false;
			    Destroy(gameObject, clip.length);
			}
		} else if (col.tag == "Terrain") {
			Debug.Log("Terrain Hit");
		     if (!queuedToDestroy) {
				 Destroy(gameObject);
			 }
		}

	} 
}

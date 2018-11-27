using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehaviour : MonoBehaviour {

	public KnifeEyes eyes = null;
	public int radius = 10;

	private float t = 0;

	private static float dt = 1f;
	private bool stabbing = false;
	

	// Use this for initialization
	void Awake () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Character") {
			Debug.Log(col.GetType());
        }
    }

    void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Character") {
			Debug.Log(col.GetType());
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
		if (eyes != null && eyes.active) {
			if (!stabbing) {
				Transform target = eyes.character.transform;

				float dx = radius*Mathf.Cos(t);
				float dy = radius*Mathf.Sin(t);

				Vector3 delta = new Vector3(dx, dy, 0);
				// The step size is equal to speed times frame time.
				float speed = 10;
				float step = speed * Time.deltaTime;

        		// Move our position a step closer to the target.
        		transform.position = Vector3.MoveTowards(transform.position, target.position + delta, step);
				// transform.position = target.position + current;

				Vector3 r = transform.eulerAngles;
				r.z = ((t+(Mathf.PI/2)) % (2*Mathf.PI))*(180/Mathf.PI);
				transform.eulerAngles = r;

				float d = dt*Time.deltaTime;
				t += d;
				t = t % (2*Mathf.PI);
			} else {
				// Stab towards character
			}
		}
	}
}

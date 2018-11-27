using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehaviour : MonoBehaviour {

	public KnifeEyes eyes = null;
	public EnemyStats stats = null;
	public int radius = 10;

	public static int minStabBase = 5;
	public static int maxStabBase = 10;

	private int minStabTime = minStabBase;
	private int maxStabTime = maxStabBase;

	private float t = 0;

	private static float dt = 1f;
	private bool stabbing = false;

	// Use this for initialization
	void Awake () {
		float randomTime = Random.Range(minStabTime, maxStabTime);
		Invoke("StabAttack", randomTime);
	}

	bool CanSeeCharacter() {
		return eyes != null && eyes.active;
	}

	
	void StabAttack()
	{
		float randomTime = Random.Range(minStabTime, maxStabTime);
		StartStab();
		Invoke("StabAttack", randomTime);
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

	void StartStab() {
		if (CanSeeCharacter()) {
			Debug.Log("Starting to stab");
			stabbing = true;
			t = (t + Mathf.PI) % (2*Mathf.PI);
		}
	}

    // Update is called once per frame
    void FixedUpdate () {
		if (stats != null) {
			minStabTime = Mathf.Max((minStabBase * (stats.health/stats.maxHealth)), 1);
			maxStabTime = Mathf.Max(maxStabBase * (stats.health/stats.maxHealth), 5);
		}

		if (CanSeeCharacter()) {
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
				Transform target = eyes.character.transform;

				float dx = radius*Mathf.Cos(t);
				float dy = radius*Mathf.Sin(t);

				Vector3 delta = new Vector3(dx, dy, 0);
				// The step size is equal to speed times frame time.

				float speed = 20;
				float step = speed * Time.deltaTime;

        		// Move our position a step closer to the target.
				transform.position = Vector3.MoveTowards(transform.position, target.position + delta, step);
				float distance = Vector3.Distance(transform.position, target.position + delta);

				Debug.Log("" + distance);

				if (distance < 0.1) {
					Debug.Log("Done stabbing");
					stabbing = false;
				}
			}
		}
	}
}

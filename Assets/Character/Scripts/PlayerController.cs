using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	
	public int health = 100;
    public int attack = 50;
	public int speed = 5;
	public float spewSpeed = 20f;
	public int spewDamage = 20;
	public float slapRange = 2f;
    public GameObject me;


    private float moveForce = 51f;
	public float maxSpeed = 3f;
	public float jumpForce = 5000f;
	private bool jump = false;
	public bool slapping = false;
	public bool slapDebounce = false;
	private bool grounded = true;

	public Transform groundCheck;

	private Rigidbody2D rb2d;
	public Rigidbody2D spewFab; 
	public GameObject slapFab; 
	public GameObject possessFab;

	private bool facingRight = true;

	// Use this for initialization
	void Awake () {
		// anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		groundCheck = transform.Find("groundCheck");
	}
	
	// Update is called once per frame
	void Update () {

		// Check if the character is on the ground
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1  << LayerMask.NameToLayer("Ground"));

		// Jump, only if the character is on the ground
		if (Input.GetButtonDown("Jump") && grounded) {
			jump = true;
		}
		if (Input.GetButtonDown("Spew")) {
			handleSpew();
		} else if (Input.GetButtonDown("Slap") && !slapping) {
			handleSlap();		
		} else if (Input.GetButtonDown("Possess")) {
			handlePossess();
		}
		handleSlapCollosion(); 
    }

	void handlePossess() {
		Vector2 direction = facingRight ? Vector2.right : Vector2.left;
		float distance = possessFab.GetComponent<Renderer>().bounds.size.x;
		RaycastHit2D possesHit = Physics2D.Raycast(transform.position, direction, distance, 1  << LayerMask.NameToLayer("Enemies"));
		if (possesHit.collider != null) {
			GameObject enemyHit = possesHit.collider.gameObject;
			Vector3 tmp = transform.position;
			transform.position = enemyHit.transform.position;
			enemyHit.transform.position = tmp;
			// swap hp
		}
	}

	void handleSpew() {
		if (facingRight) {
			Rigidbody2D spew = Instantiate(spewFab, transform.position, Quaternion.Euler(new Vector3 (0,0,0))) as Rigidbody2D;
			spew.velocity = new Vector2(spewSpeed,0);
		} else {
			Rigidbody2D spew = Instantiate(spewFab, transform.position, Quaternion.Euler(new Vector3 (0,0,180f))) as Rigidbody2D;
			spew.velocity = new Vector2(-spewSpeed,0);
		}
	}

	void handleSlap() {
		slapping = true;
		GameObject slap;
		Vector3 slapPos = transform.position;
		if (facingRight) {
			slapPos.x += slapFab.transform.localScale.x/2;
			slap = Instantiate(slapFab, slapPos, Quaternion.Euler(new Vector3 (0,0,0))) as GameObject;
			slap.transform.parent = gameObject.transform;
		} else {
			slapPos.x -= slapFab.transform.localScale.x/2;
			slap = Instantiate(slapFab, slapPos, Quaternion.Euler(new Vector3 (0,0,180f))) as GameObject;
			slap.transform.parent = gameObject.transform;
		}
		StartCoroutine(DestroySlap(slap));
	}

	void handleSlapCollosion() {
		if (slapping && !slapDebounce) {
		slapDebounce = true;
		Vector3 slapPos = transform.position;

		// Added 0.01f to extend hitbox slightly past visual slap box
		if (facingRight) {
			slapPos.x += slapFab.transform.localScale.x + 0.1f;
		} else {
			slapPos.x -= slapFab.transform.localScale.x + 0.1f;
		}
		RaycastHit2D slapHit = Physics2D.Linecast(transform.position, slapPos, 1  << LayerMask.NameToLayer("Enemies"));
		if (slapHit.collider != null) {
			Debug.Log("Enemy Smack! (POW)");
			Debug.Log(slapHit.collider.gameObject.tag);
			// slapHit.collider.gameObject.TakeDamage(attack);
		}
		StartCoroutine(LinecastSlapDebounce());
		
	}
	}
	void FixedUpdate() {
        // horizontal movement
		float h = Input.GetAxis("Horizontal");
		// anim.SetFloat("Speed", Mathf.Abs(h));

		if (h * rb2d.velocity.x < maxSpeed) {
			rb2d.AddForce(Vector2.right * h * moveForce);
		}

        // if character speed is above max character speed, set speed to max speed
		if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) {
			// Mathf.Sign gets sign of number (1 or -1)
			rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		}

		// flip the sprite based on the position of the character
		if (h > 0 && !facingRight){
			Flip();
		} else if (h < 0 && facingRight) {
			Flip();
		}

		if(jump) {
			// anim.SetTrigger("Jump");
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}

	void Flip() {
		facingRight = !facingRight;
	}

    public void TakeDamage (int amount) {
        health -= amount;
		if (health <= 0) {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {

        Collider2D collider = collision.collider;

        // if (collider.tag == "Player") {
        //     collider.gameObject.GetComponent<Player>().TakeDamage(attack);
        // }
    }

    private void Die () {
        Destroy(me);
    }

	private IEnumerator DestroySlap(GameObject slap) {
		yield return new WaitForSeconds(0.25f);
		Destroy(slap);
		slapping = false;
	}
	private IEnumerator LinecastSlapDebounce() {
		yield return new WaitForSeconds(0.25f);
		slapDebounce = false;
	}
}

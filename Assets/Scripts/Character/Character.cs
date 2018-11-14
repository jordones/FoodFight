using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {


	public int health = 100;
	public int healthMinus = 10;
	public int speed = 5;
	public float spewSpeed = 20f;
	public int spewDamage = 20;
	public int slapDamage = 50;
	public float slapRange = 2f;
	public float possessRange = 6f;
	
    public float moveForce = 51f;
	public float maxSpeed = 3f;
	public float jumpForce = 5000f;
	private bool jump = false;
	public bool slapping = false;
	public bool slapDebounce = false;
	private bool grounded = true;

	public List<Item> inventory = new List<Item>();

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

    void Start() {
		Spew.playerScript = this;
		Debug.Log(""+Spew.playerScript);
		InvokeRepeating("drainHealth", 2, 2f);
	} 
	
	// Update is called once per frame
	void Update () {

		// Check if the character is on the ground
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, (1  << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Enemies")) );

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
		RaycastHit2D possesHit = Physics2D.Raycast(transform.position, direction, possessRange, 1  << LayerMask.NameToLayer("Possessable"));
		if (possesHit.collider != null) {
			GameObject enemyHit = possesHit.collider.transform.parent.gameObject;
			Vector3 tmp = transform.position;
			transform.position = enemyHit.transform.position;
			enemyHit.transform.position = tmp;
			// swap hp
			int tempHp = enemyHit.GetComponent<EnemyStats>().health;
			enemyHit.GetComponent<EnemyStats>().health = health;
			health = tempHp;
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
		RaycastHit2D[] slapHits = Physics2D.LinecastAll(transform.position, slapPos, 1  << LayerMask.NameToLayer("Enemies"));

        foreach (var slapHit in slapHits)
		{
			if (slapHit.collider != null) {
				Debug.Log("Enemy Smack! (POW)");
				Debug.Log(slapHit.collider.gameObject.name);
				slapHit.collider.gameObject.GetComponent<EnemyStats>().TakeDamage(slapDamage);
			}	
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
		Debug.Log(health);
		if (health <= 0) {
            Die();
        }
    }

    private void Die () {
		Debug.Log("Character Death");
		StartCoroutine(LoadScene.AsyncLoadScene("MainMenu"));
        Destroy(gameObject);
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

	public int getHealth() {
		return health;
	}

	void OnGUI () {
		Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		GUI.Label(new Rect (pos.x, Screen.height-pos.y-70, 30, 30), "" + health);
		// GUI.Label(new Rect (Screen.width - 150,50,100,50), "HP: " + health);
		GUI.Label(new Rect (Screen.width - 150,50,100,50), "Slap Damage: " + slapDamage);
		GUI.Label(new Rect (Screen.width - 150,100,100,50), "Spew Damage: " + spewDamage);
		GUI.Label(new Rect (Screen.width - 150,150,100,50), "Speed: " + maxSpeed);
	}

	void drainHealth() {
		TakeDamage(healthMinus);
	}

	public void Pickup(GameObject itemObject) {
		Item item = itemObject.GetComponent<Item>();
		inventory.Add(item);
		itemObject.transform.parent = gameObject.transform;
		item.OnPickup(this);
	}
}

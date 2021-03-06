﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, OnLevelGoal
{

    public static Character instance = null;

    public int maxHealth = 100;
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

    public float slapLength = 3.0f;

    private DisplayItems itemsUI;

    public List<Item> inventory = new List<Item>();

    private Rigidbody2D rb2d;
    public Rigidbody2D spewFab;
    public GameObject slapFab;
    public GameObject possessFab;

    private GameObject slap;

    public AudioSource spewSound;
    public AudioSource slapSound;

    private bool facingRight = true;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake()
    {
        // anim = GetComponent<Animator>();
		slapDebounce = false;
        if (instance == null)
        {
            instance = this;
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();   
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Spew.playerScript = this;
        LevelManager.instance.subscribeToGoal(this);
        Debug.Log("" + Spew.playerScript);
        InvokeRepeating("drainHealth", 2, 2f);
        itemsUI = GameObject.FindGameObjectWithTag("UI Item").GetComponent<DisplayItems>();

    }

    // Update is called once per frame
    void Update()
    {
        // Jump, only if the character is on the ground
        if (Input.GetButtonDown("Jump") && grounded)
        {
            animator.SetTrigger("Jump");
            jump = true;
        }
        if (Input.GetButtonDown("Spew"))
        {
            animator.SetTrigger("Spew");
            handleSpew();
        }
        else if (Input.GetButtonDown("Slap") && !slapping)
        {
            handleSlap();
        }
        else if (Input.GetButtonDown("Possess"))
        {
            animator.SetTrigger("Posess");
            handlePossess();
        }
        handleSlapCollosion();
    }

    void handlePossess()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D possesHit = Physics2D.Raycast(transform.position, direction, possessRange, 1 << LayerMask.NameToLayer("Possessable"));
        if (possesHit.collider != null)
        {
            GameObject enemyHit = possesHit.collider.transform.parent.gameObject;
            Vector3 tmp = transform.position;
            transform.position = enemyHit.transform.position;
            enemyHit.transform.position = tmp;
            // swap hp
            EnemyStats enemy = enemyHit.GetComponent<EnemyStats>();
            int tempHp = enemy.health;
            enemy.health = health;
            health = tempHp;

            enemyHit.GetComponent<SimpleMovement>().Stun();
        }
    }

    void handleSpew()
    {
        if (facingRight)
        {
            Rigidbody2D spew = Instantiate(spewFab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
            spew.velocity = new Vector2(spewSpeed, 0);
        }
        else
        {
            Rigidbody2D spew = Instantiate(spewFab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
            spew.velocity = new Vector2(-spewSpeed, 0);
        }
        spewSound.Play();
    }

    void handleSlap()
    {
        slapping = true;
        Vector3 slapPos = transform.position;
        int dir = 0;
        if (facingRight) {
            dir = 1;
            slap = Instantiate(slapFab, slapPos, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        } else {
            dir = -1;
            slap = Instantiate(slapFab, slapPos, Quaternion.Euler(new Vector3(0, 0, 180f))) as GameObject;
        }
        slap.transform.parent = gameObject.transform;

        Vector3 lslapPos = slap.transform.localPosition;
        lslapPos.x += dir*slapLength / 2;
        slap.transform.localPosition = lslapPos;

        Vector3 newSize = slap.transform.localScale;
        newSize.x = slapLength;
        slap.transform.localScale = newSize;
        slapSound.Play();

        StartCoroutine(DestroySlap(slap));
    }

    void handleSlapCollosion()
    {
        if (slapping && !slapDebounce)
        {
            slapDebounce = true;
            Vector3 slapPos = transform.position;

            // Added 0.01f to extend hitbox slightly past visual slap box
            if (facingRight)
            {
                slapPos.x += slap.transform.lossyScale.x + 0.1f;
            }
            else
            {
                slapPos.x -= slap.transform.lossyScale.x + 0.1f;
            }
            RaycastHit2D[] slapHits = Physics2D.LinecastAll(transform.position, slapPos, 1 << LayerMask.NameToLayer("Enemies"));

            foreach (var slapHit in slapHits)
            {
                if (slapHit.collider != null)
                {
                    Debug.Log("Enemy Smack! (POW)");
                    Debug.Log(slapHit.collider.gameObject.name);
                    slapHit.collider.gameObject.GetComponent<EnemyStats>().TakeDamage(slapDamage);
                }
            }

            StartCoroutine(LinecastSlapDebounce());
        }
    }
    void FixedUpdate()
    {
        // horizontal movement
        float h = Input.GetAxis("Horizontal");
        // anim.SetFloat("Speed", Mathf.Abs(h));

        if (h == 0) {
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Idle");
        }
        else {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Walk");
        }

        if (h * rb2d.velocity.x < maxSpeed)
        {
            rb2d.AddForce(Vector2.right * h * moveForce);
        }

        // if character speed is above max character speed, set speed to max speed
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            // Mathf.Sign gets sign of number (1 or -1)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }

        // flip the sprite based on the position of the character
        if (h > 0 && !facingRight)
        {
            Flip();
        }
        else if (h < 0 && facingRight)
        {
            Flip();
        }

        if (jump)
        {
            // anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
            grounded = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Character Death");
        StartCoroutine(LoadScene.AsyncLoadScene("Death"));
        Destroy(gameObject);
    }

    private IEnumerator DestroySlap(GameObject slap)
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(slap);
        slapping = false;
    }
    private IEnumerator LinecastSlapDebounce()
    {
        yield return new WaitForSeconds(0.25f);
        slapDebounce = false;
    }

    public int getHealth()
    {
        return health;
    }

    // void OnGUI () {
    // 	Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

    // 	GUI.Label(new Rect (pos.x, Screen.height-pos.y-70, 30, 30), "" + health);
    // 	// GUI.Label(new Rect (Screen.width - 150,50,100,50), "HP: " + health);
    // 	GUI.Label(new Rect (Screen.width - 150,50,100,50), "Slap Damage: " + slapDamage);
    // 	GUI.Label(new Rect (Screen.width - 150,100,100,50), "Spew Damage: " + spewDamage);
    // 	GUI.Label(new Rect (Screen.width - 150,150,100,50), "Speed: " + maxSpeed);
    // }

    void drainHealth()
    {
        TakeDamage(healthMinus);
    }

    public void Pickup(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        itemsUI.AddItemToDisplay(item); // This needs to because because the parent of the item changes.
        inventory.Add(item);
        itemObject.transform.parent = gameObject.transform;
        item.OnPickup(this);
    }

    public void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
                grounded = true;
        } else if (col.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            // Prevent infintily jumping if an enemy is on your head
            if (transform.position.y > col.transform.position.y)
            {
                grounded = true;
            }
        }
    }

    public void OnLevelGoal()
    {
        // 	gameObject.transform.position = new Vector3 (bossSpawnX,bossSpawnY, 0);
    }
}

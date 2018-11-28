using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIndicator : MonoBehaviour
{
    public GameObject boss;
    public static BossIndicator instance = null;
    public SpriteRenderer arrowRenderer = null;
    // Use this for initialization
    void Start()
    {
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (boss != null)
        {
			arrowRenderer.enabled = true;
            if (transform.position.x < boss.transform.position.x)
            {
                // point right
                Vector3 r = transform.eulerAngles;
                r.z = 180f;
                transform.eulerAngles = r;
            }
            else
            {
                // point left
				Vector3 r = transform.eulerAngles;
                r.z = 0f;
                transform.eulerAngles = r;
            }
        } else {
			arrowRenderer.enabled = false;
		}
    }

    // Update is called once per frame
}

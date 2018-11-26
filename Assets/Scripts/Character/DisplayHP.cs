using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayHP : MonoBehaviour
{
    private Character character;

    private Transform bar;
    // Use this for initialization
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
        bar = transform.Find("Bar");
    }

    void Update()
    {
		float healthPercent = (float)character.getHealth()/character.maxHealth;
		// Debug.Log(character.getHealth());
		SetSize(healthPercent);
    }

    // Update is called once per frame
    public void SetSize(float normalizedSize)
    {
        // Size between 0 and 1
        if (normalizedSize < 0)
        {
            normalizedSize = 0;
        }
        bar.localScale = new Vector3(normalizedSize, 1f);
    }
}

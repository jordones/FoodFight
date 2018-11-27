using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsBehaviour : MonoBehaviour
{

    private int attackType;
    private float minAttackTime;
    private float maxAttackTime;
    private bool attacking = false;

    // Use this for initialization
    void Awake()
    {
        float randomTime = Random.Range(minAttackTime, maxAttackTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attacking)
        {
            switch (attackType)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
    }


}

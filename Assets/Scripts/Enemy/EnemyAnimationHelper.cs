using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHelper : MonoBehaviour {

    private Animator animator;
    private ANIMATION_STATE animationState= ANIMATION_STATE.IDLE;

	// Use this for initialization
	void Awake () {
        animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ToggleAnimation (ANIMATION_STATE newState) {
        if (newState == ANIMATION_STATE.IDLE)
        {
            animationState = ANIMATION_STATE.IDLE;
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Idle");
        }
        else if (newState == ANIMATION_STATE.WALK)
        {
            animationState = ANIMATION_STATE.IDLE;
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Walk");
        }
    }

    public void TriggerAttack () {
        animator.SetTrigger("Attack");

    }

    public void TriggerDie () {
        animator.SetTrigger("Die");

    }


}

public enum ANIMATION_STATE
{
    IDLE,
    WALK
}
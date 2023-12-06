using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private string currentState;

    private PlayerController playerController;

    public static class States
    {
        public static readonly string Idle = "Idle";

        public static readonly string Throw = "Throw";
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        playerController = GetComponent<PlayerController>();

        ChangeAnimationState(States.Idle);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        anim.Play(newState);

        currentState = newState;
    }

    void LateUpdate()
    {
        if (currentState == States.Throw)
        {
            // Checks if throw animation is complete
            if (!(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
            {
                // Tells the PlayerController script that the throw animation is complete
                playerController.FinishedThrow();

                ChangeAnimationState(States.Idle);
            }
        }
    }

    // ChangeToThrowState is called by the PlayerController Script whenever the player throws
    public void ChangeToThrowState()
    {
        ChangeAnimationState(States.Throw);
    }

}

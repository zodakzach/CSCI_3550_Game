using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerAnimation class responsible for managing player animations
public class PlayerAnimation : MonoBehaviour
{
    // Reference to the Animator component
    private Animator anim;

    // Current state of the animation
    private string currentState;

    // Reference to the PlayerController script
    private PlayerController playerController;

    // Animation states as constants
    public static class States
    {
        public static readonly string Idle = "Idle";
        public static readonly string Throw = "Throw";
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to this GameObject
        anim = GetComponent<Animator>();

        // Get the PlayerController component attached to this GameObject
        playerController = GetComponent<PlayerController>();

        // Set the initial animation state to Idle
        ChangeAnimationState(States.Idle);
    }

    // Function to change the animation state
    private void ChangeAnimationState(string newState)
    {
        // If the new state is the same as the current state, do nothing
        if (currentState == newState) return;

        // Play the animation for the new state
        anim.Play(newState);

        // Update the current state
        currentState = newState;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Check if the current animation state is Throw
        if (currentState == States.Throw)
        {
            // Checks if the throw animation is complete
            if (!(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
            {
                // Tell the PlayerController script that the throw animation is complete
                playerController.FinishedThrow();

                // Change the animation state back to Idle
                ChangeAnimationState(States.Idle);
            }
        }
    }

    // ChangeToThrowState is called by the PlayerController Script whenever the player throws
    public void ChangeToThrowState()
    {
        // Change the animation state to Throw
        ChangeAnimationState(States.Throw);
    }
}

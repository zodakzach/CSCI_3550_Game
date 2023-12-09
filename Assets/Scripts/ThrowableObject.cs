using UnityEngine;
using System;
using Cinemachine;

public class ThrowableObject : MonoBehaviour
{
    // Rigidbody component for physics simulation
    private Rigidbody2D rb;

    // Flag to track if the object has been thrown
    private bool thrown = false;

    // Event triggered when the object is destroyed
    public EventHandler OnDestroyed;

    // Cinemachine Virtual Camera for camera manipulation
    private CinemachineVirtualCamera cam;

    // TrailRenderer for visual effect trail
    private TrailRenderer trail;

    // Called when the script instance is being loaded
    private void OnEnable()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Reset thrown flag and configure trail
        thrown = false;
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
    }

    // Method to throw the object with specified force and direction
    public void ThrowObject(float force, Vector2 direction, CinemachineVirtualCamera cam)
    {
        // Check if Rigidbody2D exists and the object has not been thrown yet
        if (rb != null && !thrown)
        {
            // Assign the provided CinemachineVirtualCamera
            this.cam = cam;

            // Enable Rigidbody physics and apply force
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero; // Reset velocity before applying force
            rb.AddForce(force * direction, ForceMode2D.Impulse);

            // Enable trail emission if TrailRenderer exists
            if (trail != null)
            {
                trail.emitting = true;
            }

            // Mark the object as thrown
            thrown = true;
        }
        else
        {
            // Log a warning if Rigidbody is null or the object has already been thrown
            Debug.LogWarning("Rigidbody is null or the object has already been thrown.");
        }
    }

    // Called when this object collides with another 2D collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object has been thrown
        if (thrown)
        {
            // If a CinemachineVirtualCamera is assigned, make it follow the collision transform
            if (cam != null)
            {
                cam.Follow = collision.transform;
            }

            // Deactivate the game object
            gameObject.SetActive(false);

            // Attempt to get a DamageableObject component from the collided object
            DamageableObject obj = collision.gameObject.GetComponent<DamageableObject>();

            // If DamageableObject is found, apply damage
            if (obj != null)
            {
                obj.Damage();
            }

            // Trigger the OnDestroyed event with empty EventArgs
            OnDestroyed(this, EventArgs.Empty);
        }
    }
}

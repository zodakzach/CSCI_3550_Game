using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Rigidbody2D rb;

    public void ThrowObject(float force, Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(force * direction, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}

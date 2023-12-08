using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThrowableObject : MonoBehaviour
{
    private Rigidbody2D rb;

    private CinemachineVirtualCamera cam;

    private Vector2 camStartPos;

    private Transform originalFollow;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetFollowCam(CinemachineVirtualCamera cam)
    {
        this.cam = cam;
        originalFollow = cam.Follow;
        camStartPos = this.cam.transform.position;
        this.cam.Follow = transform;
    }

    public void ThrowObject(float force, Vector2 direction)
    {
        rb.AddForce(force * direction, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        DamageableObject obj = collision.gameObject.GetComponent<DamageableObject>();

        if ( obj != null)
        {
            obj.Damage();
        }

        ResetCam();
    }

    private void ResetCam()
    {
        cam.Follow = originalFollow;
        cam.transform.position = camStartPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Slider ThrowPowerMeter;

    [SerializeField] private Transform boulderSpawnPoint;

    [SerializeField] private GameObject boulderPrefab;

    [SerializeField] private float force;

    [SerializeField] private CinemachineVirtualCamera cam;

    private DirectionalLineController directionalLine;

    private PlayerAnimation anim;

    private bool powerMeterOn;

    private bool reverse;

    private int numOfThrows;

    [SerializeField] private int maxThrows = 5;

    private bool throwing;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        numOfThrows = 0;
        powerMeterOn = false;
        reverse = false;
        throwing = false;
        directionalLine = GetComponentInChildren<DirectionalLineController>(false);
    }

    private void FixedUpdate()
    {
        if (!powerMeterOn || throwing){ return; }

        if (ThrowPowerMeter.value == 0)
        {
            ThrowPowerMeter.value = 1;
        }

        if (ThrowPowerMeter.value >= ThrowPowerMeter.maxValue)
        {
            reverse = true;
        }
        else if (ThrowPowerMeter.value < 1)
        {
            reverse = false;
        }

        if ((ThrowPowerMeter.value < ThrowPowerMeter.maxValue) && !reverse)
        {
            ThrowPowerMeter.value *= 1.1f;
        }
        else if ((ThrowPowerMeter.value <= ThrowPowerMeter.maxValue) && reverse)
        {
            ThrowPowerMeter.value /= 1.1f;
        }
}

    // Throw is called by the PlayerInput Component attached to the object as a unity event when the user triggers the 'Throw' input action
    public void Throw(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartPowerMeter();
        }
        else if (context.canceled)
        {
            if (powerMeterOn)
            {
                StopPowerMeter();
            }
        }
    }

    private void StartPowerMeter()
    {
        if (throwing) { return; }

        powerMeterOn = true;
    }

    private void StopPowerMeter()
    {
        powerMeterOn = false;

        throwing = true;

        anim.ChangeToThrowState();

    }

    private void ResetPowerMeter()
    {
        ThrowPowerMeter.value = 0;
    }

    // FinishedThrow is called by the PlayerAnimation Script after the throw animation is finished
    public void FinishedThrow()
    {
        throwing = false;
        numOfThrows++;

        LaunchBoulder();
    }

    private void LaunchBoulder()
    {
        GameObject newBoulder = Instantiate(boulderPrefab, boulderSpawnPoint.position, Quaternion.identity);

        ThrowableObject boulder = newBoulder.GetComponent<ThrowableObject>();

        if (boulder != null)
        {
            boulder.SetFollowCam(cam);
            boulder.ThrowObject((ThrowPowerMeter.value * force), directionalLine.Direction.normalized);
        }

    }
}

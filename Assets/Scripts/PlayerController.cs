using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Slider ThrowPowerMeter;

    private bool powerMeterOn;

    private bool reverse;

    // Start is called before the first frame update
    void Start()
    {
        powerMeterOn = false;
        reverse = false;
    }

    private void FixedUpdate()
    {
        if (!powerMeterOn){ return; }

        if (ThrowPowerMeter.value == ThrowPowerMeter.maxValue)
        {
            reverse = true;
        }
        else if (ThrowPowerMeter.value == 0)
        {
            reverse = false;
        }

        if ((ThrowPowerMeter.value < ThrowPowerMeter.maxValue) && !reverse)
        {
            ThrowPowerMeter.value++;
        }
        else if ((ThrowPowerMeter.value <= ThrowPowerMeter.maxValue) && reverse)
        {
            ThrowPowerMeter.value--;
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Performed");
            StartPowerMeter();
        }
        else if (context.canceled)
        {
            Debug.Log("Canceled");
            StopPowerMeter();
        }
    }

    private void StartPowerMeter()
    {
        powerMeterOn = true;
    }

    private void StopPowerMeter()
    {
        powerMeterOn = false;
    }
}

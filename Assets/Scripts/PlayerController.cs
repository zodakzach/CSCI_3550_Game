using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Slider representing the power meter for the throw
    [SerializeField] private Slider ThrowPowerMeter;

    // Spawn point for the boulders
    [SerializeField] private Transform boulderSpawnPoint;

    // List of throwable objects (boulders)
    [SerializeField] private List<ThrowableObject> boulders;

    // Force applied to the boulders when thrown
    [SerializeField] private float force;

    // Cinemachine virtual camera for dynamic camera control
    [SerializeField] private CinemachineVirtualCamera cam;

    // GameOver screen UI element
    [SerializeField] private GameOver gameOverScreen;

    // Tower object representing the player's base
    [SerializeField] private Tower tower;

    [SerializeField] private GameObject instructionsBackground;

    // DirectionalLineController for visualizing the throw direction
    private DirectionalLineController directionalLine;

    // Current throw direction
    private Vector2 direction;

    // PlayerAnimation script for managing player animations
    private PlayerAnimation anim;

    // Flag to indicate if the power meter is active
    private bool powerMeterOn;

    // Flag to reverse the power meter value
    private bool reverse;

    // Number of available boulders
    private int numOfBoulders;

    // Flag to indicate if the player can throw
    private bool canThrow;

    // Original position to reset the camera
    private Vector2 camStartPos;

    // Original follow target for the camera
    private Transform originalFollow;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize components and variables
        anim = GetComponent<PlayerAnimation>();
        powerMeterOn = false;
        reverse = false;
        canThrow = true;
        directionalLine = GetComponentInChildren<DirectionalLineController>(false);

        numOfBoulders = boulders.Count;

        originalFollow = cam.Follow;
        camStartPos = this.cam.transform.position;

        // Attach event handlers for boulder destruction
        for (int i = 0; i < numOfBoulders; i++)
        {
            boulders[i].OnDestroyed += Boulders_OnDestroyed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Escape key input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Call the Quit method to exit the game
            gameOverScreen.Quit();
        }
    }

    // FixedUpdate is called at a fixed interval
    private void FixedUpdate()
    {
        // Update the power meter
        UpdatePowerMeter();
    }

    // Method to handle the 'Throw' input action
    public void Throw(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Start the power meter when the 'Throw' action is performed
            StartPowerMeter();
        }
        else if (context.canceled && powerMeterOn)
        {
            // Stop the power meter when the 'Throw' action is released
            StopPowerMeter();
        }
    }

    // Method called after the throw animation is finished
    public void FinishedThrow()
    {
        if (instructionsBackground.activeInHierarchy == true)
        {
            instructionsBackground.SetActive(false);
        }

        if (numOfBoulders > 0)
        {
            // Launch the next available boulder
            LaunchBoulder(boulders.Find(obj => obj.gameObject.activeSelf && obj.enabled));
        }
    }

    // Event handler for boulder destruction
    private void Boulders_OnDestroyed(object sender, EventArgs e)
    {
        // Check for game end conditions and perform a delayed action
        CheckEndGame();
        StartCoroutine(DelayedAction());
    }

    // Method to update the power meter
    private void UpdatePowerMeter()
    {
        if (!powerMeterOn || !canThrow) { return; }

        // Ensure the power meter value is within bounds
        if (ThrowPowerMeter.value == 0)
        {
            ThrowPowerMeter.value = 1;
        }

        // Adjust the power meter value based on the direction
        if (ThrowPowerMeter.value >= ThrowPowerMeter.maxValue)
        {
            reverse = true;
        }
        else if (ThrowPowerMeter.value < 1)
        {
            reverse = false;
        }

        // Update the power meter value based on the direction
        if ((ThrowPowerMeter.value < ThrowPowerMeter.maxValue) && !reverse)
        {
            ThrowPowerMeter.value *= 1.1f;
        }
        else if ((ThrowPowerMeter.value <= ThrowPowerMeter.maxValue) && reverse)
        {
            ThrowPowerMeter.value /= 1.1f;
        }
    }

    // Method to start the power meter
    private void StartPowerMeter()
    {
        if (!canThrow) { return; }

        powerMeterOn = true;
    }

    // Method to stop the power meter and initiate the throw
    private void StopPowerMeter()
    {
        powerMeterOn = false;
        canThrow = false;
        direction = directionalLine.Direction.normalized;
        directionalLine.gameObject.SetActive(false);

        // Change to the throw animation state
        anim.ChangeToThrowState();
    }

    // Method to reset the power meter
    private void ResetPowerMeter()
    {
        ThrowPowerMeter.value = 0;
        directionalLine.gameObject.SetActive(true);
    }

    // Method to launch a boulder
    private void LaunchBoulder(ThrowableObject boulder)
    {
        if (boulder != null)
        {
            // Deactivate the boulder, reposition it, and activate it
            boulder.gameObject.SetActive(false);
            boulder.gameObject.transform.position = boulderSpawnPoint.position;
            boulder.gameObject.SetActive(true);

            // Set up the camera to follow the launched boulder
            cam.Follow = boulder.gameObject.transform;

            // Throw the boulder with calculated force and direction
            boulder.ThrowObject((ThrowPowerMeter.value * force), direction, cam);
            numOfBoulders--;
        }
    }

    // Method to reset the camera
    private void ResetCam()
    {
        cam.Follow = originalFollow;
        cam.transform.position = camStartPos;
    }

    // Method to check for game end conditions
    private void CheckEndGame()
    {
        if (tower.Health > 0 && numOfBoulders == 0)
        {
            // Set up the game over screen for a loss
            gameOverScreen.SetUpScreen(false);
        }
        else if (tower.Health <= 0)
        {
            // Set up the game over screen for a win
            gameOverScreen.SetUpScreen(true);
        }
    }

    // Coroutine for delayed actions
    private IEnumerator DelayedAction()
    {
        // Delay for 1.25 seconds
        yield return new WaitForSeconds(1.25f);

        // Perform the delayed actions: allow throwing, reset the camera, and reset the power meter
        canThrow = true;
        ResetCam();
        ResetPowerMeter();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    // List of DamageableObject instances composing the tower
    [SerializeField] private List<DamageableObject> tower;

    // Total health of the tower
    [SerializeField] private int health;

    // Slider representing the health bar of the tower
    [SerializeField] private Slider healthBar;

    // Property to access the current health of the tower
    public int Health
    {
        get { return health; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the maximum value and initial value of the health bar
        healthBar.maxValue = health;
        healthBar.value = health;

        // Attach event handlers to the OnDamaged event of each DamageableObject in the tower
        if (tower.Count > 0)
        {
            for (int i = 0; i < tower.Count; i++)
            {
                tower[i].OnDamaged += Tower_OnDamaged;
            }
        }
    }

    // Event handler for the OnDamaged event of DamageableObjects in the tower
    private void Tower_OnDamaged(object sender, DamageableObject.OnDamagedArgs e)
    {
        // Decrease the tower's health and health bar value based on the damage received
        health -= e.DamageAmount;
        healthBar.value -= e.DamageAmount;
    }
}

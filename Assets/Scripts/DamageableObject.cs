using UnityEngine;
using System;

public class DamageableObject : MonoBehaviour
{
    // Amount of damage this object will take each time it is Damaged
    [SerializeField] private int damageAmount;

    // Event triggered when the object is damaged
    public event EventHandler<OnDamagedArgs> OnDamaged;

    // EventArgs class to hold information about the damage event
    public class OnDamagedArgs : EventArgs
    {
        public int DamageAmount;
    }

    // Method to simulate damage to the object
    public void Damage()
    {
        // Trigger the OnDamaged event with information about the damage amount
        OnDamaged(this, new OnDamagedArgs { DamageAmount = damageAmount });
    }
}

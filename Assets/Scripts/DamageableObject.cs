using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageableObject : MonoBehaviour
{

    [SerializeField] private int damageAmount;

    public event EventHandler<OnDamagedArgs> OnDamaged;

    public class OnDamagedArgs : EventArgs
    {
        public int DamageAmount;
    }

    public void Damage()
    {
        OnDamaged(this, new OnDamagedArgs { DamageAmount = damageAmount });
    }

}

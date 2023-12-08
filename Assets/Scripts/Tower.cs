using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private List<DamageableObject> tower;

    [SerializeField] private int health;

    // Start is called before the first frame update
    void Start()
    {
        if (tower.Count > 0)
        {
            for (int i = 0; i < tower.Count; i++)
            {
                tower[i].OnDamaged += Tower_OnDamaged;
            }
        }
    }

    private void Tower_OnDamaged(object sender, DamageableObject.OnDamagedArgs e)
    {
        health -= e.DamageAmount;

        if (health <= 0)
        {
            DestroyTower();
        }
    }

    private void DestroyTower()
    {
        Debug.Log("Win");
    }
}

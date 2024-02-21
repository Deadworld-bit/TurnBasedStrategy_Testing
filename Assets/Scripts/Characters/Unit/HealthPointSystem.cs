using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPointSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public event EventHandler OnUnitDown;
    public event EventHandler OnDamaged;

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);
        
        if (health == 0)
        {
            UnitDown();
        }
        
        Debug.Log(health);
    }

    private void UnitDown()
    {
        OnUnitDown?.Invoke(this,EventArgs.Empty);
    }

    public float getHealthPercentage()
    {
        return (float)health / healthMax;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPointSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public event EventHandler OnUnitDown;

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

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
}
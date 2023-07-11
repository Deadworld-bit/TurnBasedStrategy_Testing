using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }
    }
}
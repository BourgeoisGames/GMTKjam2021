using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maximumHealth = 1.0f; // Maximum health

    private float currentHealth;

    private void Start()
    {
        currentHealth = maximumHealth;
    }

    void take_damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }
}

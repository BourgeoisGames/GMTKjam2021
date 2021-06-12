using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Maximum health
    public float maximumHealth = 5.0f;
    // Game over script
    public GameOver gameOver;
    // Health UI
    public HealthUI healthUI;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maximumHealth;
    }

    public void take_damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameOver.game_over();
        }
    }

    private void Update()
    {
        healthUI.set_health(currentHealth / maximumHealth);
    }
}

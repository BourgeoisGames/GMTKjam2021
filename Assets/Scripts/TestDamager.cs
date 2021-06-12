using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private void Update()
    {
        playerHealth.take_damage(Time.deltaTime);
    }
}

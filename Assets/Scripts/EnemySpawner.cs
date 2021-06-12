using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Interval before the first wave spawns
    public float firstSpawnTime = 3.0f;
    // Interval between enemy waves spawning
    public float enemySpawnTime = 10.0f;
    // List of positions in which to spawn enemy waves
    // This should be given as the position of the origin of the enemy objects that spawn
    public Vector3[] enemySpawnLocations;
    // The enemy to spawn
    public GameObject enemyPrefab;
    // Number of enemies in a wave
    public int enemiesPerWave = 5;

    // Time until the next spawn
    private float spawnTimer;

    private void Start()
    {
        spawnTimer = firstSpawnTime;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0.0f)
        {
            // Pick a random spawn point
            int spawnPointIndex = Random.Range(0, enemySpawnLocations.Length);
            Vector3 spawnPoint = enemySpawnLocations[spawnPointIndex];
            // Spawn the next wave of enemies
            for (int i = 0; i < enemiesPerWave; i++)
            {
                Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            }

            spawnTimer += enemySpawnTime;
        }
    }
}

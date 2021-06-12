using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Player controller
    public PlayerController playerController;
    public PlayerScore scorekeeper;
    // Interval before the first wave spawns
    public float firstSpawnTime = 3.0f;
    // Interval between enemy waves spawning
    public float enemySpawnTime = 10.0f;
    // List of positions in which to spawn enemy waves
    // This should be given as the position of the origin of the enemy objects that spawn
    private Vector3[] enemySpawnLocations;
    // The enemy to spawn
    public GameObject enemyPrefab;
    // Number of enemies in a wave
    public int enemiesPerWave = 5;

    // Time until the next spawn
    private float spawnTimer;

    private string spawn_tag = "EnemySpawnPoint";

    private void Start()
    {
        spawnTimer = firstSpawnTime;
        populate_spawn_points();
    }

    void populate_spawn_points()
    {
        GameObject[] spawn_points = GameObject.FindGameObjectsWithTag(spawn_tag);
        ArrayList spawn_list = new ArrayList();
        foreach(GameObject spawn_point in spawn_points){
            spawn_list.Add(spawn_point.transform.position);
        }
        enemySpawnLocations = (Vector3[])spawn_list.ToArray(typeof(Vector3));

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
                GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                EnemyController enemyController = enemySpawned.GetComponent<EnemyController>();
                if (enemyController == null)
                {
                    Debug.LogError("THE ENEMY PREFAB HAS NO CONTROLLER SCRIPT -- YOU SUCK ASS");
                }
                else
                {
                    enemyController.set_target(playerController);
                    enemyController.set_scorekeeper(scorekeeper);
                }
            }

            spawnTimer += enemySpawnTime;
        }
    }
}

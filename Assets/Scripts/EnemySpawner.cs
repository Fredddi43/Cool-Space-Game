/*
 * File: EnemySpawner.cs
 * Description: Manager class to handle enemy spawning.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;

// EnemySpawner class, inherits from MonoBehaviour
public class EnemySpawner : MonoBehaviour
{
    // Variable definitions
    public GameObject[] enemyPrefabs;  // Array to store enemy prefabs
    public float spawnInterval;   // Time between spawns
    private float timer = 0f; // Timer to track spawn intervals. Actual spawn interval is set in the GameConfig, dont try to change it here

    // Screen boundaries, used to determine spawn positions
    private Vector2 spawnTopRight;
    private Vector2 spawnBottomLeft;

    // Start method, called before the first frame update
    void Start()
    {
        spawnInterval = GameConfigManager.instance.gameConfig.enemySpawnInterval; // Get the spawn interval from the GameConfig instance
        // Get the world coordinates for the screen boundaries
        spawnTopRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        spawnBottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }
    // Update method, called once per frame
    void Update()
    {
        // Increment the timer, if we exceed the spawnIntervall, spawn
        // an enemy and reset the timer
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    // SpawnEnemy method, spawns an enemy at a random vertical position on the right side of the screen
    void SpawnEnemy()
    {
        // Randomly select an enemy type based on your array of enemy prefabs.
        int index = Random.Range(0, enemyPrefabs.Length);
        string enemyTag = "Enemy" + index; // Constructs "Enemy0" or "Enemy1"

        // Determine a spawn position on the right side of the screen at a random vertical position.
        float spawnY = Random.Range(spawnBottomLeft.y, spawnTopRight.y);
        Vector2 spawnPosition = new Vector2(spawnTopRight.x + 1f, spawnY);

        // Spawn the enemy from the corresponding pool.
        GameObject enemy = ObjectPooler.instance.SpawnFromPool(enemyTag, spawnPosition, Quaternion.identity);
        enemy.layer = LayerMask.NameToLayer("Enemy"); // Set the layer to "Enemy"
    }
}

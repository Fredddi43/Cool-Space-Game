/*
 * File: Enemy.cs
 * Description: Enemy class defines enemy types, behaviour and collision handling.
 *              Implements object pooling via IPooledObject.
 *              Fires an event on destruction to decouple score updates.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyType
{
    RedSquare,     // default speed
    YellowSquare,  // double speed
    Meteor,        // splits into 2 SmallMeteor at 45 degrees north and south
    SmallMeteor    // default speed, does not split further
}

public class Enemy : MonoBehaviour, IPooledObject
{
    public float speed;  // Default speed
    public Sprite[] enemySprites; // Array of possible sprites for this enemy

    private SpriteRenderer _spriteRenderer; // Reference to the SpriteRenderer component
    public EnemyType enemyType;
    public Vector2 moveDirection = Vector2.left; // Default movement direction
    private Rigidbody2D _rigidbody;
    private float lifeTime; // Lifetime of the enemy, to avoid memory leaks
    private Coroutine lifetimeCoroutine; // Coroutine to handle enemy lifetime
    public GameObject[] powerUpPrefabs; // Array of possible powerups to drop
    public GameObject[] explosionPrefabs; // Array of possible explosion animation prefabs 
    // (set these to be in the same order as the sprites or it will play the wrong animation)
    private int chosenSpriteIndex = 0; // Store the chosen sprite since we chose them at random within an enemy class

    // Awake method, called when the script instance is being loaded
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        speed = GameConfigManager.instance.gameConfig.enemyDefaultSpeed; // Get the enemy speed from the GameConfig instance
        lifeTime = GameConfigManager.instance.gameConfig.enemyLifetime; // Get the enemy lifetime from the GameConfig instance
    }
    
    // OnObjectSpawn method is called by the ObjectPooler when the enemy is reused.
    public void OnObjectSpawn()
    {
        // Restart lifetime coroutine
        if (lifetimeCoroutine != null)
            StopCoroutine(lifetimeCoroutine);
        lifetimeCoroutine = StartCoroutine(DisableAfterTime(lifeTime));

        // Randomly select a sprite from the array if it exists
        if (enemySprites != null && enemySprites.Length > 0)
        {
            chosenSpriteIndex = Random.Range(0, enemySprites.Length);
            _spriteRenderer.sprite = enemySprites[chosenSpriteIndex];
        }
        // Start enemy movement
        SetTrajectory(moveDirection);
    }
    
    // Fallback for non-pooled objects: if not pooled, call OnObjectSpawn in Start.
    void Start()
    {
        if (gameObject.activeInHierarchy)
            OnObjectSpawn();
    }
    
    // Coroutine to disable enemy after its lifetime expires.
    private IEnumerator DisableAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    
    // Start movement of the instance in the given direction.
    // Note: We removed the immediate deactivation to let the enemy live for its lifetime.
    private void SetTrajectory(Vector2 direction)
    {
        // YellowSquare enemies move twice as fast
        if (enemyType == EnemyType.YellowSquare)
        {
            // since we set the speed from config earlier, we can just
            // multiply it here. We grab the Multiplier from the
            // GameConfigManager directly since it's only used once in
            // the entire file.
            _rigidbody.linearVelocity = direction.normalized * speed * GameConfigManager.instance.gameConfig.yellowSquareSpeedMultiplier;
        }
        else
        {
            _rigidbody.linearVelocity = direction.normalized * speed;
        }
    }
    
    // Collision detection with the player's bullets, all other collisions are ignored.
    // Player collisions are handled in the PlayerController.cs script.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only trigger upon bullet collision
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Play our explosion sound sfx and animation
            AudioManager.instance.PlaySFX(AudioManager.instance.explosionSound);
            if (explosionPrefabs != null && explosionPrefabs.Length > 0)
            {
                int explosionIndex = chosenSpriteIndex;
                if (explosionIndex >= explosionPrefabs.Length)
                    explosionIndex = explosionPrefabs.Length - 1;

                Instantiate(explosionPrefabs[explosionIndex], transform.position, Quaternion.identity);
            }
            // 20% chance to drop a powerup, taken from the GameConfigManager
            if (Random.value < GameConfigManager.instance.gameConfig.powerupDropChance && powerUpPrefabs.Length > 0)
            {
                // Randomly pick one of the powerups, spawn it and play the spawn sfx
                int randomIndex = Random.Range(0, powerUpPrefabs.Length);
                Instantiate(powerUpPrefabs[randomIndex], transform.position, Quaternion.identity);
                AudioManager.instance.PlaySFX(AudioManager.instance.powerupSpawnSound);
            }
            // If the enemy is a meteor, split it into two smaller meteors
            if (enemyType == EnemyType.Meteor)
            {
                Instantiate(explosionPrefabs[0], transform.position, Quaternion.identity);
                SpawnSmallMeteors();
            }
            // Return the enemy to the pool
            gameObject.SetActive(false);
            // Fire the enemy-destruction event to award 100 points.
            GameEvents.EnemyDestroyed(100);
        }
    }
    
    // Split the meteor into two smaller meteors.
    private void SpawnSmallMeteors()
    {
        // Instantiate two clones at the same position as this asteroid.
        GameObject smallAsteroidUp = Instantiate(gameObject, transform.position, Quaternion.identity);
        GameObject smallAsteroidDown = Instantiate(gameObject, transform.position, Quaternion.identity);

        // Get their Enemy scripts so we can adjust their properties.
        Enemy enemyUp = smallAsteroidUp.GetComponent<Enemy>();
        Enemy enemyDown = smallAsteroidDown.GetComponent<Enemy>();

        // Change their type so they won't split further.
        enemyUp.enemyType = EnemyType.SmallMeteor;
        enemyDown.enemyType = EnemyType.SmallMeteor;

        // Scale them down.
        smallAsteroidUp.transform.localScale *= 0.5f;
        smallAsteroidDown.transform.localScale *= 0.5f;

        // Set movement directions:
        // For a base movement of left (180°), adding 45° upward gives 135°, and downward gives 225°.
        float angleUp = 135f * Mathf.Deg2Rad;
        float angleDown = 225f * Mathf.Deg2Rad;
        enemyUp.moveDirection = new Vector2(Mathf.Cos(angleUp), Mathf.Sin(angleUp)).normalized;
        enemyDown.moveDirection = new Vector2(Mathf.Cos(angleDown), Mathf.Sin(angleDown)).normalized;
    }
}

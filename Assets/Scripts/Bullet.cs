/*
 * File: Bullet.cs
 * Description: Bullet class is responsible for the bullet behavior,
 * mostly just a handler framework for bullet prefab.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;
using System.Collections;

// Bullet class, inherits from MonoBehaviour
public class Bullet : MonoBehaviour, IPooledObject
{
    // Variable definitions
    private float speed; // The speed of the bullet is 10 units per second
    public static float lifeTime; // The bullets lifetime is 10s
    private Rigidbody2D _rigidbody; // Reference to the Rigidbody2D component
    public GameObject missileExplosionPrefab; // Reference to the missile explosion prefab
    private Coroutine disableCoroutine; // Coroutine reference for disabling the bullet

    // Awake method, called when the script instance is being loaded
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        speed = GameConfigManager.instance.gameConfig.bulletSpeed; // Get the speed from the GameConfigManager
        lifeTime = GameConfigManager.instance.gameConfig.bulletLifetime; // Get the lifetime from the GameConfigManager
    }

    // on spawn of the bullet, give it a disable coroutine so we return it
    // to the pool after a certain amount of time (this is set in the GameConfig)
    public void OnObjectSpawn() {
        if (disableCoroutine != null)
            StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(DisableAfterTime(lifeTime));
    }

    private IEnumerator DisableAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // wait for the end of the bullets lifetime
        gameObject.SetActive(false); // return it to the pool
    }
    
    // Fire method, fires the bullet in a given direction
    public void Fire(Vector2 direction)
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.shootSound);
        _rigidbody.linearVelocity = direction.normalized * speed;
        // Calculate the angle based on the direction, then add an
        // offset of -90 degrees since our sprite is rotated
        float angleInDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angleInDegrees += -90; // Adjust so the sprite appears horizontal
        transform.rotation = Quaternion.Euler(0, 0, angleInDegrees);
    }

    // OnTriggerEnter2D method, called when the bullet collides with
    // an enemy. All other collisions are ignored.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyShipRedSquare") || collision.CompareTag("EnemyShipYellowSquare") || collision.CompareTag("Meteor"))
        {
            Instantiate(missileExplosionPrefab, transform.position, Quaternion.identity); // Play the xplosion animation
            gameObject.SetActive(false); // Return the bullet to the pool
        }
    }
}

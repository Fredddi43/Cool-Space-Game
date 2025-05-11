/*
 * File: PowerUp.cs
 * Description: Defines the types and behaviour of the powerups
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;

// Define an enum for the powerup types
public enum PowerUpType
{
    Score,
    Invincibility,
    WeaponLevelUpgrade
}
// PowerUp class, inherits from MonoBehaviour
public class PowerUp : MonoBehaviour
{
    // Variable definitions
    private float lifeTime = 10f; // Lifetime of the powerup, to avoid memory leaks
    public PowerUpType powerUpType;

    // Start method, called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the powerup after its lifetime to avoide memory leaks
    }
    // Collision handling, we only care about player collisions and ignore all others
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // play powerup pickup sound
            AudioManager.instance.PlaySFX(AudioManager.instance.powerupPickupSound);
            PlayerController player = collision.GetComponent<PlayerController>();

            switch (powerUpType)
            {
                case PowerUpType.Score:
                    // Add 50 points (doesn't need the PlayerController)
                    ScoreManager.instance.AddScore(50);
                    break;

                case PowerUpType.Invincibility:
                    // Make the player invincible for 5 seconds
                    if (player != null)
                    {
                        player.StartInvincibility(GameConfigManager.instance.gameConfig.invincibilityDuration);
                    }
                    break;

                case PowerUpType.WeaponLevelUpgrade:
                    // Upgrade the player's weapon level
                    if (player != null)
                    {
                        player.UpgradeWeaponLevel();
                    }
                    break;
            }

            // Destroy the power-up after applying its effect
            Destroy(gameObject);
        }
    }
}

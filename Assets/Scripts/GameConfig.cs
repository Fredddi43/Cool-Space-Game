/*
 * File: GameConfig.cs
 * Description: Config file for game settings.
 * Author: Friedemann Lipphardt
 * Date: 2025-29-03
 */

// Imports
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]

// GameConfig class, inherits from ScriptableObject
public class GameConfig : ScriptableObject
{
    [Header("Player Settings")]
    public float playerSpeed = 10f;
    public float invincibilityDuration = 5f;
    
    [Header("Bullet Settings")]
    public float bulletSpeed = 10f;
    public float bulletLifetime = 10f;
    
    [Header("Enemy Settings")]
    public float enemyDefaultSpeed = 2f;
    public float enemyLifetime = 20f;
    public float yellowSquareSpeedMultiplier = 2f;
    public float enemySpawnInterval = 2f;
    public float powerupDropChance = 0.20f;
    
    [Header("Explosion Settings")]
    public float explosionLifetime = 1f;
}

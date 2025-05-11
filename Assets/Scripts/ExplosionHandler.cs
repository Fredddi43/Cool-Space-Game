/*
 * File: ExplosionHandler.cs
 * Description: Small helper class to handle explosion animations.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;

// Explosion class, inherits from MonoBehaviour
public class Explosion : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, GameConfigManager.instance.gameConfig.explosionLifetime); // play the animation then destroy it
    }
}

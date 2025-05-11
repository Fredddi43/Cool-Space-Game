/*
 * File: GameEvents.cs
 * Description: Centralized static class for game events.
 * Author: Friedemann Lipphardt
 * Date: 2025-29-03
 */

// Imports
using System;

public static class GameEvents
{
    // Event fired when an enemy is destroyed.
    // Passes the score award as an integer parameter.
    public static event Action<int> OnEnemyDestroyed;

    // Call this method to fire the enemy destruction event.
    public static void EnemyDestroyed(int scoreAward)
    {
        OnEnemyDestroyed?.Invoke(scoreAward);
    }
}

// We could implement other events here in the future, but for now
// enemy destruction is the only one that made sense.
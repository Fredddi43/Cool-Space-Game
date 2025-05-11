/*
 * File: GameEventsTests.cs
 * Description: Unit tests for the GameEvents static class. Verifies that the enemy-destruction event
 * is fired correctly with the expected score award. Simple example to
 * trial the Unity testing framework.
 * Author: Friedemann Lipphardt
 * Date: 2025-01-04
 */

// Imports
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class GameEventsTests
{
    private int eventScore = 0;

    [SetUp]
    public void Setup()
    {
        // Subscribe to the enemy destruction event.
        GameEvents.OnEnemyDestroyed += OnEnemyDestroyedHandler;
    }

    [TearDown]
    public void Teardown()
    {
        // Unsubscribe to prevent side effects between tests.
        GameEvents.OnEnemyDestroyed -= OnEnemyDestroyedHandler;
    }

    // Handler to capture the score award from the event.
    private void OnEnemyDestroyedHandler(int scoreAward)
    {
        eventScore = scoreAward;
    }

    [Test]
    public void GameEvents_EnemyDestroyed_FiresEvent()
    {
        // Fire the enemy-destruction event with a score award of 200.
        GameEvents.EnemyDestroyed(200);
        // Verify that the event handler received the correct value.
        Assert.AreEqual(200, eventScore);
    }
}

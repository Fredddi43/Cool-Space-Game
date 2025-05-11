/*
 * File: ScoreManager.cs
 * Description: ScoreManager class, handles scoring and updates the UI.
 *              Subscribes to enemy destruction events to update the score.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance for easy access

    public int score = 0; // Start score is 0
    public TMP_Text scoreText; // Reference to the UI Text component

    // Event fired when the score changes, passing the new score as parameter.
    public static event Action<int> OnScoreChanged;

    private void Awake()
    {
        // Singleton pattern ensures only one ScoreManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subscribe to both scene load and enemy destruction events.
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEvents.OnEnemyDestroyed += HandleEnemyDestroyed;
    }
    
    private void OnDisable()
    {
        // Unsubscribe from events to prevent memory leaks.
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }
    
    // Called when a new scene is loaded; update the scoreText reference if in GameScene.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            StartCoroutine(UpdateScoreTextReference());
        }
    }

    // Coroutine to update the scoreText reference after a frame
    // delay. Was necessary to ensure the UI Text is available in the
    // scene, as the scene transition from MainMenu to GameScene
    // caused the UI Text to not be available immediately.
    private IEnumerator UpdateScoreTextReference()
    {
        yield return new WaitForEndOfFrame();
        TMP_Text newScoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        if (newScoreText != null)
        {
            scoreText = newScoreText;
            UpdateScoreText();
        }
        else
        {
            Debug.LogWarning("ScoreText object not found in GameScene. Ensure the UI Text is named 'ScoreText'.");
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    private void Start()
    {
        UpdateScoreText();
    }

    // This method is called when an enemy is destroyed.
    private void HandleEnemyDestroyed(int scoreAward)
    {
        AddScore(scoreAward);
    }

    // AddScore method, adds the given amount to the score and updates the UI.
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        OnScoreChanged?.Invoke(score);
    }

    // UpdateScoreText method, updates the score text in the UI.
    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}

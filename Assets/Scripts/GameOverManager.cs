/*
 * File: GameOverManager.cs
 * Description: GameOverManager class is responsible for handling 
 * game over state. Instanced class.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    // Variable definitions
    // Singleton instanced class
    public static GameOverManager instance;
    public GameObject gameOverScreen; // Reference to the game over screen
    public TMP_Text finalScoreText; // Reference to the final score text

    // Awake method, called when the script instance is being loaded
    private void Awake()
    {
        // Implement singleton pattern for easy access
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // TriggerGameOver method, called when the game is over
    public void TriggerGameOver()
    {
        // Play explosion and game over sound effects
        AudioManager.instance.PlaySFX(AudioManager.instance.explosionSound);
        AudioManager.instance.PlaySFX(AudioManager.instance.gameOverSound);
        // Display the final score on the game over screen
        finalScoreText.text = "Final Score: " + ScoreManager.instance.score;
        gameOverScreen.SetActive(true); // Show the game over screen
        // Use the state machine to handle game over state
        GameManager.instance.SetState(GameState.GameOver);

        // Disable player input to prevent further movement/shooting
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.enabled = false;
        }
    }

    // Update method, called once per frame
    private void Update()
    {
        // If the game is over and the player presses Space, restart the scene.
        if (GameManager.instance.currentState == GameState.GameOver && Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.RestartGame();
        }
    }
}

/*
 * File: GameManager.cs
 * Description: GameManager handles overall game state transitions using a finite state machine.
 *              It manages states like MainMenu, Playing, Paused, and GameOver.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Our range of game states.
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

// GameManager class, inherits from MonoBehaviour
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Current game state.
    public GameState currentState = GameState.MainMenu;

    // Event fired whenever the game state changes.
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        // Implement singleton pattern and persist across scenes.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager alive across scene loads.
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance.
        }

         // Ensure an AudioManager is present (this is relevant if we
         // start directly on the GameScene since AudioManager is
         // usually owned by MainMenu)
        if (AudioManager.instance == null)
        {
            GameObject audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
                Debug.Log("AudioManager instantiated from Resources.");
            }
            else
            {
                Debug.LogError("AudioManagerPrefab not found in Resources!");
            }
        }
    }

    // Method to set the game state.
    public void SetState(GameState newState)
    {
        if (newState != currentState) // game state has changed
        {
            currentState = newState;
            OnGameStateChanged?.Invoke(currentState); // Notify listeners of the state change.

            // Execute additional logic based on state change.
            switch (currentState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1;
                    // Load the MainMenu scene.
                    SceneManager.LoadScene("MainMenu");
                    break;
                case GameState.Playing:
                    Time.timeScale = 1;
                    // Load the gameplay scene
                    SceneManager.LoadScene("GameScene");
                    break;
                case GameState.Paused:
                    Time.timeScale = 0;
                    // Pause game logic. (Not currently used.)
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0;
                    // Show game over overlays (handled by GameOverManager).
                    break;
            }
        }
    }

    // Example method to toggle pause state. (Not currently used.)
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
            SetState(GameState.Paused);
        else if (currentState == GameState.Paused)
            SetState(GameState.Playing);
    }

    // Method to restart the game.
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetState(GameState.Playing);
        // Reset the score before restarting
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetScore();
        }
    }
}

/*
 * File: AudioManager.cs
 * Description: AudioManager class is responsible for playing 
 * background music and sound effects. Instanced class.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Variable definitions
    // Singleton instanced class
    public static AudioManager instance;

    public AudioSource musicSource;      // For background music
    public AudioSource sfxSource;        // For one-shot SFX

    // Audio clips we use in the game
    public AudioClip backgroundMusic;
    public AudioClip mainMenuMusic;
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioClip powerupSpawnSound;
    public AudioClip powerupPickupSound;
    public AudioClip gameOverSound;

    // Awake method, called when the script instance is being loaded
    private void Awake()
    {
        // If an instance already exists and it's not this, destroy the new one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    // OnEnable method, subscribe to sceneLoaded events
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // OnDisable method, unsubscribe from sceneLoaded events
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a new scene is loaded. Switches music based on scene name.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we're in the MainMenu scene, use mainMenuMusic.
        if (scene.name == "MainMenu")
        {
            if (mainMenuMusic != null)
            {
                musicSource.clip = mainMenuMusic;
                musicSource.loop = true;
                musicSource.volume = 0.4f;
                musicSource.Play();
            }
        }
        else
        {
            // Otherwise, play the standard background music.
            if (backgroundMusic != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.loop = true;
                musicSource.volume = 0.4f;
                musicSource.Play();
            }
        }
    }

    // Start method, called before the first frame update
    private void Start()
    {
        // get our active scene to decide what music to play
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu") // we start either with the main menu music
        {
            if (mainMenuMusic != null)
            {
                musicSource.clip = mainMenuMusic;
                musicSource.loop = true;
                musicSource.volume = 0.4f;
                musicSource.Play();
            }
        }
        else // or directly with the game audio
        {
            if (backgroundMusic != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.loop = true;
                musicSource.volume = 0.4f;
                musicSource.Play();
            }
        }
    }


    // Play SFX method, plays a one-shot sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}

/*
 * File: PlayerController.cs
 * Description: PlayerController class is responsible for handling 
 * player movement, collision and shooting. Also plays movement audio.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

// PlayerController class, inherits from MonoBehaviour
public class PlayerController : MonoBehaviour
{
    // Variable definitions

    // Get the bottom left and top right corners of the screen, used
    // for boundary clamping
    private static Vector2 bottomLeft;
    private static Vector2 topRight;

    public int weaponLevel = 1; // Default weapon level
    private float speed; // Default speed
    public Bullet bulletPrefab; // Define our bullet prefab
    public bool isInvincible = false; // Invincibility, relevant for power-up
    public AudioSource movementAudioSource; // audioSource for ship's movement audio    
    public AudioClip movementSound; // movement audio clip
    private Animator _animator; // animator for ships animation
    private Vector2 spriteExtents; // half width and half height of the player's sprite

    // Start method, called before the first frame update
    void Start()
    {
        // Initialize variables
        topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _animator = GetComponent<Animator>();
        speed = GameConfigManager.instance.gameConfig.playerSpeed; // Get the player speed from the GameConfig instance

        // Cache the sprite extents
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteExtents = sr.bounds.extents;
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is over, stop the movement audio, otherwise it will keep playing
        if (GameManager.instance.currentState == GameState.GameOver)
        {
            if (movementAudioSource.isPlaying)
                movementAudioSource.Stop();
            return;
        }
        // Move the ship
        MoveShip();
        // If the ship is moving, play the movement audio
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) // ship is moving
        {
            if (!movementAudioSource.isPlaying)
            {
                movementAudioSource.clip = movementSound;
                movementAudioSource.loop = true;
                movementAudioSource.Play();
            }

            // set the movement animation in the animator
            _animator.SetBool("isMoving", true);
        }
        else // ship is not moving
        {
            movementAudioSource.Stop();
        }
        // shoot on spacebar press
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (GameManager.instance.currentState == GameState.Playing)
            {
                Shoot();
            }
        }
    }

    // Ship movement function, WASD or arrow keys move the ship
    private void MoveShip()
    {
        // Get the horizontal and vertical inputs
        // this is set in unity project settings
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Get the current position of the ship
        Vector2 currentPosition = transform.position;

        // Calculate the new position, multiplying by Time.deltaTime for frame independence.
        // Adjust boundaries by adding/subtracting the sprite extents so that the whole sprite stays on-screen.
        float clampedX = Mathf.Clamp(
            currentPosition.x + horizontalInput * speed * Time.deltaTime,
            bottomLeft.x + spriteExtents.x,
            topRight.x - spriteExtents.x);
        float clampedY = Mathf.Clamp(
            currentPosition.y + verticalInput * speed * Time.deltaTime,
            bottomLeft.y + spriteExtents.y,
            topRight.y - spriteExtents.y);

        // Update the position of the ship
        transform.position = new Vector2(clampedX, clampedY);
    }

    // Fire function, space bar fires the weapon, weapon level 1-3
    // determines the kind of weapon fired.
    // Level 1: Single Shot
    // Level 2: Split Shot
    // Level 3: Single + Split Shot
    private void Shoot()
    {
        _animator.SetTrigger("shoot");
        switch (weaponLevel)
        {
            case 1:
                FireSingleShot(transform.position);
                break;
            case 2:
                FireSplitShot(transform.position);
                break;
            case 3:
                FireSplitShot(transform.position);
                FireSingleShot(transform.position);
                break;
        }
    }

    // fire a single bullet for mode 1 & 3
    private void FireSingleShot(Vector2 currentPosition)
    {
        // Grab a new bullet from the object pooler
        GameObject bulletObj = ObjectPooler.instance.SpawnFromPool("Bullet", currentPosition, bulletPrefab.transform.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        // Fire the bullet
        bullet.Fire(Vector2.right);
    }

    // fire 2 split bullets for mode 2 & 3
    private void FireSplitShot(Vector2 currentPosition)
    {
        // Two bullets at 30° up and 30° down relative to right.
        float angle = 30f * Mathf.Deg2Rad;
        // Calculate direction for 30° upward bullet
        Vector2 directionUp = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        // Calculate direction for 30° downward bullet
        Vector2 directionDown = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle)).normalized;

        // shoot two bullets, one each in our new directions
        GameObject bulletObjUp = ObjectPooler.instance.SpawnFromPool("Bullet", currentPosition, bulletPrefab.transform.rotation);
        Bullet bulletUp = bulletObjUp.GetComponent<Bullet>();
        bulletUp.Fire(directionUp);

        GameObject bulletObjDown = ObjectPooler.instance.SpawnFromPool("Bullet", currentPosition, bulletPrefab.transform.rotation);
        Bullet bulletDown = bulletObjDown.GetComponent<Bullet>();
        bulletDown.Fire(directionDown);
    }

    // StartInvincibility method, makes the player invincible for a
    // given duration, used for power-up
    public void StartInvincibility(float duration)
    {
        StartCoroutine(InvincibilityRoutine(duration));
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float flashInterval = 0.2f; // Adjust flash speed as desired
        float timer = 0f;

        while (timer < duration)
        {
            sr.enabled = !sr.enabled; // Toggle visibility
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        // Ensure sprite is visible at the end
        sr.enabled = true;
        isInvincible = false;
    }

    // UpgradeWeaponLevel method, upgrades the player's weapon level,
    // used for power-up, max level 3, starts at 1
    public void UpgradeWeaponLevel()
    {
        // Upgrade the player's weapon level
        if (weaponLevel < 3) { weaponLevel++; }
    }

    // OnTriggerEnter2D method, called when the player collides with
    // an object. Only handles enemy collisions. Power-up collisions
    // are handled in the PowerUp class. Bullet collisions are ignored.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if we collide with an enemy, and we are not invincible, trigger game over
        if ((collision.CompareTag("EnemyShipRedSquare") || collision.CompareTag("EnemyShipYellowSquare") || collision.CompareTag("Meteor")) && !isInvincible)
        {
            GameOverManager.instance.TriggerGameOver();
        }
    }
}

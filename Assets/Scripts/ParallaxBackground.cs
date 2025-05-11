/*
 * File: ParallaxBackground.cs
 * Description: Draws our parallax background
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

// Imports
using UnityEngine;

// ParallaxLayer class, inherits from MonoBehaviour
public class ParallaxLayer : MonoBehaviour
{
    // Variable definitions
    public float scrollSpeed = 0.1f; // Speed of the scrolling
    private Vector2 startPos; // Initial position of the background
    private float layerWidth; // Width of the sprite in world units

    // Start method, called before the first frame update
    void Start()
    {
        // Save the initial position
        startPos = transform.position;
        // Get the width of the sprite in world units
        layerWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    // Update method, called once per frame
    void Update()
    {
        // Move the background to the left
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // When the background has moved left by its full width,
        // reposition it to the right of its starting position
        if (transform.position.x <= startPos.x - layerWidth)
        {
            transform.position = new Vector2(startPos.x + layerWidth, transform.position.y);
        }
    }
}

/*
 * File: MainMenuController.cs
 * Description: Handler class for the main menu.
 *              Triggers state changes via GameManager.
 * Author: Friedemann Lipphardt
 * Date: 2025-15-03
 */

using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // This method is triggered by the start game button on the main menu.
    public void StartGame()
    {
        // Load the game scene and set the game state to Playing.
        GameManager.instance.SetState(GameState.Playing);
    }
}

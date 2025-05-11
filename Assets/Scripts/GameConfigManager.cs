/*
 * File: GameConfig.cs
 * Description: Config file Manager. Instanced singleton class, used
 * to reference variables in the config from other classes.
 * Author: Friedemann Lipphardt
 * Date: 2025-29-03
 */

// Imports
using UnityEngine;

// GameConfigManager class, inherits from MonoBehaviour
public class GameConfigManager : MonoBehaviour
{
    public static GameConfigManager instance;
    public GameConfig gameConfig;

    // This class doesn't do a lot except act as a handler for the
    // GameConfig file. It is a singleton class, so it can be referenced
    // from other classes, and therefore it checks wether it already exists.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

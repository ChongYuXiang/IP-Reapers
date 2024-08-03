/* Author: Chong Yu Xiang  
 * Filename: Game Manager
 * Descriptions: For gameobjects to persist through scenes and to keep track of score and health
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int currentHealth = 100;
    public int currentFuel = 0;
    private bool keyGot = false;

    public static GameManager instance;
    public GameObject sceneManager;

    // Dont destroy on load
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Call to decrease health
    public void DecreaseHealth(int HealthToRemove)
    {
        currentHealth -= HealthToRemove;

        // Player dies
        if (currentHealth <= 0)
        {
            // Game over scene
            sceneManager = GameObject.Find("SceneManager");
            sceneManager.SendMessage("ChangeScene", 5);
            Destroy(gameObject);
        }
    }
}

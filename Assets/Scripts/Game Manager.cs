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
    public bool keyGot = false;
    public bool axeCollected = false;
    public bool shotgunCollected = false;
    public bool rifleCollected = false;
    public int medkitsCollected = 0;

    public static GameManager instance;
    public GameObject sceneChanger;

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

    // Call to adjust health
    public void AdjustHealth(int HealthToChange)
    {
        currentHealth += HealthToChange;
        Debug.Log(currentHealth);

        // Player dies
        if (currentHealth <= 0)
        {
            // Game over scene
            sceneChanger = GameObject.Find("Scene Changer");
            sceneChanger.SendMessage("ChangeScene", 5);
            Debug.Log("scene changed");
            Destroy(gameObject);
        }
    }

    // To save inventory between scenes
    public void pickupAxe()
    {
        axeCollected = true;
    }
    public void pickupShotgun()
    {
        shotgunCollected = true;
    }
    public void pickupRifle()
    {
        rifleCollected = true;
    }
    public void pickupMedkit(int change)
    {
        medkitsCollected += change;
    }
}

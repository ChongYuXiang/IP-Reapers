/* Author: Chong Yu Xiang  
 * Filename: Game Manager
 * Descriptions: For gameobjects to persist through scenes and to keep track of score and health
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public int currentHealth = 100;

    public int currentFuel = 0;
    public int currentKey = 0;
    public bool completed = false;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI keyText;

    public bool axeCollected = false;
    public bool shotgunCollected = false;
    public bool rifleCollected = false;
    public int medkitsCollected = 0;

    public int shotgunAmmo = 6;
    public int rifleAmmo = 30;
    public TextMeshProUGUI ammoText;

    public static GameManager instance;
    public GameObject sceneChanger;
    public Image healthbar;

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
        if (HealthToChange < 0)
        {
        }

        // Player dies
        if (currentHealth <= 0)
        {
            // Health does not go below 0
            currentHealth = 0;
            // Game over scene
            sceneChanger = GameObject.Find("SceneChanger");
            sceneChanger.SendMessage("ChangeScene", 5);
            Debug.Log("scene changed");
            Destroy(gameObject);
        }
        // Player heals over max health
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }
        healthbarUpdate();
        Debug.Log(currentHealth);
    }

    private void healthbarUpdate()
    {
        healthbar.fillAmount = (float)currentHealth / 100f;
        if (currentHealth < 30)
        {
            healthbar.color = Color.red;
        }
        else if (currentHealth < 60)
        {
            healthbar.color = Color.yellow;
        }
        else
        {
            healthbar.color = Color.green;
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
        updateAmmo("medkit");
    }
    public void pickupFuel()
    {
        currentFuel++;
        fuelText.text = currentFuel.ToString();
    }
    public void pickupKey()
    {
        currentKey = 1;
        keyText.text = currentKey.ToString();
    }
    public void completionCheck()
    {
        if (currentKey == 1 && currentFuel == 14)
        {
            completed = true;
        }
    }

    public void rifleShot(string action)
    {
        if (action == "shoot")
        {
            rifleAmmo--;
            updateAmmo("rifle");
        }
        if (action == "reload")
        {
            rifleAmmo = 30;
            updateAmmo("rifle");
        }
    }
    public void shotgunShot(string action)
    {
        if (action == "shoot")
        {
            shotgunAmmo--;
            updateAmmo("shotgun");
        }
        if (action == "reload")
        {
            shotgunAmmo = 6;
            updateAmmo("shotgun");
        }
    }

    public void updateAmmo(string weapon)
    {
        if (weapon == "na")
        {
            ammoText.text = "";
        }
        if (weapon == "shotgun")
        {
            ammoText.text = shotgunAmmo + "/6";
        }
        if (weapon == "rifle")
        {
            ammoText.text = rifleAmmo + "/30";
        }
        if (weapon == "medkit")
        {
            ammoText.text = medkitsCollected.ToString();
        }
    }
}

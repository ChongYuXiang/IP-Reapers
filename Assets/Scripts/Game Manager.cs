/* Author: Chong Yu Xiang and Tan Chun Boon, Caleb Matthew
 * Filename: Game Manager
 * Descriptions: For gameobjects to persist through scenes and to keep track of health inventory and UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public int currentHealth = 100;

    public int currentFuel = 0;
    public int currentKey = 0;
    public bool completed = false;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI keyText;

    public bool axeCollected = false;
    public GameObject axeUI;
    public bool shotgunCollected = false;
    public GameObject shotgunUI;
    public bool rifleCollected = false;
    public GameObject rifleUI;
    public int medkitsCollected = 0;
    public GameObject medkitsUI;

    public int shotgunAmmo = 6;
    public int rifleAmmo = 30;
    public TextMeshProUGUI ammoText;

    public static GameManager instance;
    public GameObject sceneChanger;
    public Image healthbar;

    private void Awake()
    {
        // Dont destroy on load
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
        // Player dies
        if (currentHealth <= 0)
        {
            // Health does not go below 0
            currentHealth = 0;
            // Game over scene
            sceneChanger = GameObject.Find("SceneChanger");
            sceneChanger.SendMessage("ChangeScene", 5);
            Destroy(gameObject);
        }
        // Player heals over max health
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }
        healthbarUpdate();
    }

    private void healthbarUpdate()
    {
        // Match healthbar visual to health
        healthbar.fillAmount = (float)currentHealth / 100f;

        // Change healthbar to red if low health
        if (currentHealth < 30)
        {
            healthbar.color = Color.red;
        }
        // Change healthbar to yellow of medium health
        else if (currentHealth < 60)
        {
            healthbar.color = Color.yellow;
        }
        // Change healthbar to green at high health
        else
        {
            healthbar.color = Color.green;
        }
    }

    // Update axe status and UI
    public void pickupAxe()
    {
        axeCollected = true;
        axeUI.SetActive(true);
    }
    // Update shotgun status and UI
    public void pickupShotgun()
    {
        shotgunCollected = true;
        shotgunUI.SetActive(true);
    }
    // Update rifle status and UI
    public void pickupRifle()
    {
        rifleCollected = true;
        rifleUI.SetActive(true);
    }
    // Update medkit amount and UI
    public void pickupMedkit(int change, bool weaponHeld)
    {
        medkitsCollected += change;
        if (medkitsCollected > 0)
        {
            // Show medkit UI if in inventory
            medkitsUI.SetActive(true);
        }
        else
        {
            // Hide medkit UI if none remaining
            medkitsUI.SetActive(false);
        }
        // Only show medkit ammo when not holding another item
        if (!weaponHeld)
        {
            updateAmmo("medkit");
        }
    }
    // Update fuel amount and UI
    public void pickupFuel()
    {
        currentFuel++;
        fuelText.text = currentFuel.ToString();
    }
    // Update key amount and UI
    public void pickupKey()
    {
        currentKey = 1;
        keyText.text = currentKey.ToString();
    }
    // Check if player has all resources needed to win the game
    public void completionCheck()
    {
        if (currentKey == 1 && currentFuel == 12)
        {
            completed = true;
        }
    }

    // Rifle shooting or reloading based in input
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

    // Shotgun shooting or reloading based in input
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

    // Update ammo count and UI based on weapon that is input
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

    // Update highlighted UI in inventory
    public void updateColors(string weapon, bool active)
    {
        if (weapon == "axe")
        {
            if (active)
            {
                // Highlight axe, unhighlight others
                axeUI.GetComponent<Image>().color = Color.green;
                shotgunUI.GetComponent<Image>().color = Color.white;
                rifleUI.GetComponent<Image>().color = Color.white;
                medkitsUI.GetComponent<Image>().color = Color.white;
            }
            else
            {
                axeUI.GetComponent<Image>().color = Color.white;
            }
        }
        if (weapon == "shotgun")
        {
            if (active)
            {
                // Highlight shotgun, unhighlight others
                shotgunUI.GetComponent<Image>().color = Color.green;
                axeUI.GetComponent<Image>().color = Color.white;
                rifleUI.GetComponent<Image>().color = Color.white;
                medkitsUI.GetComponent<Image>().color = Color.white;
            }
            else
            {
                shotgunUI.GetComponent <Image>().color = Color.white;
            }
        }
        if (weapon == "rifle")
        {
            if (active)
            {
                // Highlight rifle, unhighlight others
                rifleUI.GetComponent<Image>().color = Color.green;
                axeUI.GetComponent<Image>().color = Color.white;
                shotgunUI.GetComponent<Image>().color = Color.white;
                medkitsUI.GetComponent<Image>().color = Color.white;
            }
            else
            {
                rifleUI.GetComponent<Image>().color = Color.white;
            }
        }
        if (weapon == "medkit")
        {
            if (active)
            {
                // Highlight medkit, unhighlight others
                medkitsUI.GetComponent<Image>().color = Color.green;
                rifleUI.GetComponent<Image>().color = Color.white;
                axeUI.GetComponent<Image>().color = Color.white;
                shotgunUI.GetComponent<Image>().color = Color.white;
            }
            else
            {
                medkitsUI.GetComponent<Image>().color = Color.white;
            }
        }
        if (weapon == "all")
        {

            // Unhighlight all
            medkitsUI.GetComponent<Image>().color = Color.white;
            rifleUI.GetComponent<Image>().color = Color.white;
            axeUI.GetComponent<Image>().color = Color.white;
            shotgunUI.GetComponent<Image>().color = Color.white;
        }
    }
}

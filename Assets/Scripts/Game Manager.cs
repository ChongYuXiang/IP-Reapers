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
        axeUI.SetActive(true);
    }
    public void pickupShotgun()
    {
        shotgunCollected = true;
        shotgunUI.SetActive(true);
    }
    public void pickupRifle()
    {
        rifleCollected = true;
        rifleUI.SetActive(true);
    }
    public void pickupMedkit(int change)
    {
        medkitsCollected += change;
        updateAmmo("medkit");
        if (medkitsCollected > 0)
        {
            medkitsUI.SetActive(true);
        }
        else
        {
            medkitsUI.SetActive(false);
        }
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
        if (currentKey == 1 && currentFuel == 15)
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

    public void updateColors(string weapon, bool active)
    {
        if (weapon == "axe")
        {
            if (active)
            {
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
    }
}

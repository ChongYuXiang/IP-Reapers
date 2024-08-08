/* Author: Chong Yu Xiang  
 * Filename: Player
 * Descriptions: Player raycast
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerCamera;
    public float interactionDistance;

    public GameObject sceneChanger;
    public GameObject gameManager;
    public TextMeshProUGUI description;

    public GameObject flashlight;

    public GameObject axe;
    private bool axeCollected = false;

    public GameObject shotgun;
    private bool shotgunCollected = false;

    public GameObject rifle;
    private bool rifleCollected = false;

    public GameObject medkit;
    private int medkitsCollected = 0;

    private void Update()
    {
        // Update GameManager in case of scene change
        gameManager = GameObject.Find("GameManager");

        // Draw debug interaction raycast
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);

        // Detect using interaction raycast
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitInteract, interactionDistance))
        {

            // Ray hits player house exit
            if (hitInteract.transform.name == "doorStart")
            {
                // Activate and change prompt
                description.text = "Press E to exit";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Change scene to town
                    sceneChanger.SendMessage("ChangeScene", 2);
                }
            }

            // Ray hits player house entrance
            else if (hitInteract.transform.name == "doorRestart")
            {
                // Activate and change prompt
                description.text = "Press E to restart game";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    sceneChanger.SendMessage("ChangeScene", 0);
                }
            }

            // Ray hits police station entrance
            else if (hitInteract.transform.name == "wallDoubleDoorA")
            {
                // Activate and change prompt
                description.text = "Press E to enter";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Change scene to town
                    sceneChanger.SendMessage("ChangeScene", 3);
                }
            }

            // Ray hits police station exit
            else if (hitInteract.transform.name == "wallDoubleDoorB")
            {
                // Activate and change prompt
                description.text = "Press E to exit";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Change scene to town
                    sceneChanger.SendMessage("ChangeScene", 2);
                }
            }

            // Ray hits door
            else if (hitInteract.transform.name == "door")
            {
                // Activate and change prompt
                description.text = "Press E to interact";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Change scene to town
                    hitInteract.transform.gameObject.SendMessage("Change");
                }
            }

            // Ray hits axe
            else if (hitInteract.transform.name == "AxePickup")
            {
                // Activate and change prompt
                description.text = "Press E to pick up axe";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Add to inventory
                    Destroy(hitInteract.transform.gameObject);
                    gameManager.SendMessage("pickupAxe");
                }
            }
            // Ray hits shotgun
            else if (hitInteract.transform.name == "ShotgunPickup")
            {
                // Activate and change prompt
                description.text = "Press E to pick up shotgun";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Add to inventory
                    Destroy(hitInteract.transform.gameObject);
                    gameManager.SendMessage("pickupShotgun");
                }
            }
            // Ray hits rifle
            else if (hitInteract.transform.name == "RiflePickup")
            {
                // Activate and change prompt
                description.text = "Press E to pick up AS-86";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Add to inventory
                    Destroy(hitInteract.transform.gameObject);
                    gameManager.SendMessage("pickupRifle");
                }
            }
            // Ray hits medkit
            else if (hitInteract.transform.name == "MedkitPickup")
            {
                // Activate and change prompt
                description.text = "Press E to pick up medkit";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Add to inventory
                    Destroy(hitInteract.transform.gameObject);
                    gameManager.SendMessage("pickupMedkit", 1);
                }
            }

            // Not looking at important object
            else
            {
                // Deactivate prompt
                description.gameObject.SetActive(false);
            }
        }
        // Not looking at any object
        else
        {
            // Deactivate prompt
            description.gameObject.SetActive(false);
        }


        // Toggle flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeInHierarchy);
        }



        //Check if player has axe
        axeCollected = gameManager.GetComponent<GameManager>().axeCollected;
        if (axeCollected)
        {
            //If player presses 1
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Equip axe
                rifle.SetActive(false);
                shotgun.SetActive(false);
                medkit.SetActive(false);
                axe.SetActive(!axe.activeInHierarchy);
            }
        }
        //Check if player has shotgun
        shotgunCollected = gameManager.GetComponent<GameManager>().shotgunCollected;
        if (shotgunCollected)
        {
            //If player presses 2
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Equip shotgun
                rifle.SetActive(false);
                axe.SetActive(false);
                medkit.SetActive(false);
                shotgun.SetActive(!shotgun.activeInHierarchy);
            }
        }
        //Check if player has rifle
        rifleCollected = gameManager.GetComponent<GameManager>().rifleCollected;
        if (rifleCollected)
        {
            //If player presses 3
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //Equip rifle
                shotgun.SetActive(false);
                axe.SetActive(false);
                medkit.SetActive(false);
                rifle.SetActive(!rifle.activeInHierarchy);
            }
        }
        //Check if player has medkit
        medkitsCollected = gameManager.GetComponent<GameManager>().medkitsCollected;
        if (medkitsCollected >= 1)
        {
            //If player presses 4
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //Equip medkit
                shotgun.SetActive(false);
                axe.SetActive(false);
                rifle.SetActive(false);
                medkit.SetActive(!rifle.activeInHierarchy);
            }
        }

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitEnemy, 15))
        {

            if (hitEnemy.transform.CompareTag("Enemy") && rifle.activeSelf == true)
            {
                // Click to interact
                if (Input.GetButtonDown("Fire1"))
                {
                    // Damage enemy
                    hitEnemy.transform.gameObject.SendMessage("damage");
                }
            }
        }

        if (medkit.activeSelf == true)
        {
            // Click to use
            if (Input.GetButtonDown("Fire1"))
            {
                // Heal player
                gameManager.GetComponent<GameManager>().AdjustHealth(30);

                // Update medkit amount
                gameManager.SendMessage("pickupMedkit", -1);
                medkitsCollected = gameManager.GetComponent<GameManager>().medkitsCollected;

                if (medkitsCollected <= 0)
                {
                    medkit.SetActive(false);
                }
            }
        }
    }
}

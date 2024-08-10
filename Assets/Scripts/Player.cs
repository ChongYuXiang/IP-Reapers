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
    public GameObject shotgun;
    public GameObject rifle;
    public GameObject medkit;
    private int medkitsCollected = 0;

    private bool reloading = false;

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
            else if (hitInteract.transform.name == "doorHome")
            {
                // Activate and change prompt
                description.text = "Press E to enter";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    sceneChanger.SendMessage("ChangeScene", 1);
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
                    description.gameObject.SetActive(false);
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
                    description.gameObject.SetActive(false);
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
                    description.gameObject.SetActive(false);
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
                    description.gameObject.SetActive(false);
                }
            }

            // Ray hits fuel
            else if (hitInteract.transform.name == "FuelCanister")
            {
                // Activate and change prompt
                description.text = "Press E to pick up fuel";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Add to inventory
                    Destroy(hitInteract.transform.gameObject);
                    gameManager.SendMessage("pickupFuel", 1);
                    description.gameObject.SetActive(false);
                }
            }
            // Ray hits car key
            else if (hitInteract.transform.name == "CarKey")
            {
                // Activate and change prompt
                description.text = "Press E to pick up car key";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Add to inventory
                    Destroy(hitInteract.transform.gameObject);
                    gameManager.SendMessage("pickupKey");
                    description.gameObject.SetActive(false);
                }
            }

            // Ray hits police car
            else if (hitInteract.transform.name == "carPolice")
            {
                gameManager.SendMessage("completionCheck");

                if (gameManager.GetComponent<GameManager>().completed)
                {
                    // Activate and change prompt
                    description.text = "Press E to escape";
                    description.gameObject.SetActive(true);

                    // Press E to interact
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Change scene to victory
                        sceneChanger.SendMessage("ChangeScene", 4);
                        Destroy(gameManager);
                    }
                }
                else
                {
                    // Activate and change prompt
                    description.text = "You cannot use this car yet\n" + gameManager.GetComponent<GameManager>().currentFuel + "/15 fuel, " + gameManager.GetComponent<GameManager>().currentKey + "/1 key";
                    description.gameObject.SetActive(true);
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
        if (gameManager.GetComponent<GameManager>().axeCollected)
        {
            //If player presses 1
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Equip axe
                rifle.SetActive(false);
                shotgun.SetActive(false);
                medkit.SetActive(false);
                axe.SetActive(!axe.activeInHierarchy);
                gameManager.GetComponent<GameManager>().updateColors("axe", axe.activeInHierarchy);

                //Activate axe functions
                StartCoroutine(axeAttack());
            }
        }
        //Check if player has shotgun
        if (gameManager.GetComponent<GameManager>().shotgunCollected)
        {
            //If player presses 2
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Equip shotgun
                rifle.SetActive(false);
                axe.SetActive(false);
                medkit.SetActive(false);
                shotgun.SetActive(!shotgun.activeInHierarchy);
                gameManager.GetComponent<GameManager>().updateColors("shotgun", shotgun.activeInHierarchy);
                gameManager.GetComponent<GameManager>().updateAmmo("shotgun");

                //Activate shotgun functions
                StartCoroutine(shotgunAttack());
            }
            if (gameManager.GetComponent<GameManager>().rifleAmmo != 6 && shotgun.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    StartCoroutine(shotgunReload());
                }
            }
        }
        //Check if player has rifle
        if (gameManager.GetComponent<GameManager>().rifleCollected)
        {
            //If player presses 3
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //Equip rifle
                shotgun.SetActive(false);
                axe.SetActive(false);
                medkit.SetActive(false);
                rifle.SetActive(!rifle.activeInHierarchy);
                gameManager.GetComponent<GameManager>().updateColors("rifle", rifle.activeInHierarchy);
                gameManager.GetComponent<GameManager>().updateAmmo("rifle");

                //Activate rifle functions
                StartCoroutine(rifleAttack());
            }
            if (gameManager.GetComponent<GameManager>().rifleAmmo != 30 && rifle.activeSelf) 
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    StartCoroutine(rifleReload());
                }
            }
        }
        //Check if player has medkit
        if (gameManager.GetComponent<GameManager>().medkitsCollected >= 1)
        {
            //If player presses 4
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //Equip medkit
                shotgun.SetActive(false);
                axe.SetActive(false);
                rifle.SetActive(false);
                medkit.SetActive(!medkit.activeInHierarchy);
                gameManager.GetComponent<GameManager>().updateColors("medkit", medkit.activeInHierarchy);
                gameManager.GetComponent<GameManager>().updateAmmo("medkit");
            }
        }

        // Use medkit
        if (medkit.activeSelf == true)
        {
            // If clicking and health is not already max
            if (Input.GetButtonDown("Fire1") && gameManager.GetComponent<GameManager>().currentHealth != 100)
            {
                // Heal player
                gameManager.GetComponent<GameManager>().AdjustHealth(60);

                // Update medkit amount
                gameManager.SendMessage("pickupMedkit", -1);
                medkitsCollected = gameManager.GetComponent<GameManager>().medkitsCollected;
                gameManager.GetComponent<GameManager>().updateAmmo("medkit");

                // Unequip if no medkits left
                if (medkitsCollected <= 0)
                {
                    medkit.SetActive(false);
                }
            }
        }

        // Remove ammo text when not holding a gun
        if (rifle.activeSelf == false && shotgun.activeSelf == false && medkit.activeSelf == false)
        {
            gameManager.GetComponent<GameManager>().updateAmmo("na");
        }

    }

    // Rifle functions
    IEnumerator rifleAttack()
    {
        // While rifle equipped
        while (rifle.activeSelf == true)
        {
            // Rifle has ammo
            if (gameManager.GetComponent<GameManager>().rifleAmmo > 0)
            {
                // When click
                if (Input.GetButton("Fire1") && !reloading)
                {
                    if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit rifleHit, 25))
                    {
                        if (rifleHit.transform.gameObject.CompareTag("Enemy"))
                        {
                            rifleHit.transform.gameObject.SendMessage("damage", 35);
                        }
                    }
                    gameManager.GetComponent<GameManager>().rifleShot("shoot");
                    yield return new WaitForSeconds(0.16f);
                }
            }
            yield return new WaitForEndOfFrame();
        } 
    }
    IEnumerator rifleReload()
    {
        reloading = true;
        yield return new WaitForSeconds(0.8f);
        if (rifle.activeSelf)
        {
            gameManager.GetComponent<GameManager>().rifleShot("reload");
        }
        yield return new WaitForSeconds(0.2f);
        reloading = false;
    }

    // Shotgun functions
    IEnumerator shotgunAttack()
    {
        // While rifle equipped
        while (shotgun.activeSelf == true)
        {
            // Rifle has ammo
            if (gameManager.GetComponent<GameManager>().shotgunAmmo > 0)
            {
                // When click
                if (Input.GetButton("Fire1") && !reloading)
                {
                    if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit shotgunHit, 10))
                    {
                        if (shotgunHit.transform.gameObject.CompareTag("Enemy"))
                        {
                            shotgunHit.transform.gameObject.SendMessage("damage", 100);
                        }
                    }
                    gameManager.GetComponent<GameManager>().shotgunShot("shoot");
                    yield return new WaitForSeconds(1f);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator shotgunReload()
    {
        reloading = true;
        yield return new WaitForSeconds(0.8f);
        if (shotgun.activeSelf)
        {
            gameManager.GetComponent<GameManager>().shotgunShot("reload");
        }
        yield return new WaitForSeconds(0.2f);
        reloading = false;
    }

    // Axe function
    IEnumerator axeAttack()
    {
        // While rifle equipped
        while (axe.activeSelf == true)
        {
            // When click
            if (Input.GetButton("Fire1"))
            {
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit axeHit, 3))
                {
                    if (axeHit.transform.gameObject.CompareTag("Enemy"))
                    {
                        axeHit.transform.gameObject.SendMessage("damage", 200);
                    }
                }
                yield return new WaitForSeconds(0.75f);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}

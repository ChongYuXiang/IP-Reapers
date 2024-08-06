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
    [SerializeField]
    Transform playerCamera;

    [SerializeField]
    float interactionDistance;

    [SerializeField]
    GameObject sceneManager;

    [SerializeField]
    TextMeshProUGUI description;

    [SerializeField]
    GameObject flashlight;

    [SerializeField]
    GameObject rifle;

    [SerializeField]
    GameObject shotgun;

    [SerializeField]
    GameObject axe;

    private void Update()
    {
        // Draw debug interaction raycast
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);

        // Detect using interaction raycast
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitInteract, interactionDistance))
        {

            // Ray hits player house exit
            if (hitInteract.transform.name == "doorP")
            {
                // Activate and change prompt
                description.text = "Press E to exit";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Change scene to town
                    sceneManager.SendMessage("ChangeScene", 2);
                }
            }

            // Ray hits player house entrance
            else if (hitInteract.transform.name == "doorP2")
            {
                // Activate and change prompt
                description.text = "Press E to enter";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Change scene to town
                    sceneManager.SendMessage("ChangeScene", 1);
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
                    sceneManager.SendMessage("ChangeScene", 3);
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
                    sceneManager.SendMessage("ChangeScene", 2);
                }
            }

            // Ray hits door
            else if (hitInteract.transform.name == "door")
            {
                // Activate and change prompt
                description.text = "Press E to open";
                description.gameObject.SetActive(true);

                // Press E to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Change scene to town
                    hitInteract.transform.gameObject.SendMessage("Change");
                    description.gameObject.SetActive(false);
                }
            }

            // Not looking at important object
            else
            {
                // Deactivate prompt
                description.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            shotgun.SetActive(false);
            axe.SetActive(false);
            rifle.SetActive(!rifle.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rifle.SetActive(false);
            axe.SetActive(false);
            shotgun.SetActive(!shotgun.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            rifle.SetActive(false);
            shotgun.SetActive(false);
            axe.SetActive(!axe.activeInHierarchy);
        }

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitEnemy, 10))
        {

            if (hitEnemy.transform.CompareTag("Enemy") && rifle.activeSelf == true)
            {
                // Activate and change prompt
                description.text = "Shoot!";
                description.gameObject.SetActive(true);

                // Click to interact
                if (Input.GetButtonDown("Fire1"))
                {
                    // Damage enemy
                    hitEnemy.transform.gameObject.SendMessage("damage");
                }
            }
        }
    }
}

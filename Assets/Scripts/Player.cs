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

    private void Update()
    {
        // Draw debug raycast
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);

        // Detect using raycast
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactionDistance))
        {
            // Ray hits player house door
            if (hit.transform.name == "doorP")
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
    }
}

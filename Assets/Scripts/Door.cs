/* Author: Tan Chun Boon, Caleb Matthew
 * Filename: Door
 * Descriptions: Opening and closing doors
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    public NavMeshObstacle EntranceForAI;

    private bool opened = false;
    private bool turning = false;
    private float openDuration = 0.8f;
    private float currentDuration;

    private Vector3 startRotation;
    private Vector3 targetRotation;

    private void Update()
    {
        if (turning)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / openDuration;
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t); // Turn door
            gameObject.GetComponent<MeshCollider>().enabled = false; // Turn off collider when turning

            if (currentDuration >= openDuration) // If door is done turning
            {
                currentDuration = 0f;
                turning = false; // Stop turning
                transform.eulerAngles = targetRotation; // Snap to specific rotation
                opened = !opened; // Switch state
            }
        }
        else
        {
            // Turn on collider when stationary
            gameObject.GetComponent<MeshCollider>().enabled = true;
        }
    }

    // Prepare to close door
    public void OpenDoor()
    {
        if (!turning)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.y -= 90f;

            turning = true; // Start turn
            AudioManager.instance.PlaySFX("Door");
        }
    }

    // Prepare to close door
    public void CloseDoor()
    {
        if (!turning)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.y += 90f;

            turning = true; // Start turn
            AudioManager.instance.PlaySFX("Door");
        }
    }

    // Call to change state of door
    public void Change()
    {
        if (!opened) // Door is closed
        {
            // Open door
            OpenDoor();
            EntranceForAI.enabled = false;
        }
        else // Door is open
        {
            // Close door
            CloseDoor();
            EntranceForAI.enabled = true;
        }
    }
}
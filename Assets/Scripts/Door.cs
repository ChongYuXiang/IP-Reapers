/* Author: Chong Yu Xiang  
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
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            gameObject.GetComponent<MeshCollider>().enabled = false;

            if (currentDuration >= openDuration)
            {
                currentDuration = 0f;
                turning = false;
                transform.eulerAngles = targetRotation;
                opened = !opened;
            }
        }
        else
        {
            gameObject.GetComponent<MeshCollider>().enabled = true;
        }
    }

    //Handles the opening of door
    public void OpenDoor()
    {
        if (!turning)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.y -= 90f;

            turning = true;
        }
    }

    public void CloseDoor()
    {
        if (!turning)
        {
            startRotation = transform.eulerAngles;
            targetRotation = startRotation;
            targetRotation.y += 90f;

            turning = true;
        }
    }

    public void Change()
    {
        if (!opened)
        {
            OpenDoor();
            EntranceForAI.enabled = false;
        }
        else
        {
            CloseDoor();
            EntranceForAI.enabled = true;
        }
    }
}